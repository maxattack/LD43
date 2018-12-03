using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour {

	public static Ship inst;

	public const int ThrustMultiplier = 10000;

	public float maxBalancePenaltyMeters = 10f;
	public float thrustHalflife = 60f;

	internal List<Booty> booty = new List<Booty>();
	internal List<ShipMass> masses = new List<ShipMass>();
	internal List<Thruster> thrusters = new List<Thruster>();
	internal Vector2 centerOfMass;
	internal float balancePenalty;
	internal float speed;
	internal float mass;
	internal float realthrust;
	internal float thrust;

	public List<string> bootyNames;
	public List<string> crewNames;
	internal int bootyIdx = 0;
	internal int crewIdx = 0;

	internal string GetBootyName(string def) {
		if (bootyIdx >= bootyNames.Count)
			return def;
		var it = bootyIdx;
		++bootyIdx;
		return bootyNames[it];
	}

	internal string GetCrewName(string def) {
		if (crewIdx >= crewNames.Count)
			return def;
		crewIdx = (crewIdx + 1) % crewNames.Count;
		return crewNames[crewIdx];
	}

	float startTime = 0f;

	void Awake() {
		inst = this;
		bootyNames.Sort((a, b) => Random.value > 0.5f ? 1 : -1);
		crewNames.Sort((a, b) => Random.value > 0.5f ? 1 : -1);
		startTime = Time.time;
	}

	void OnDestroy() {
		if (inst == this)
			inst = null;
	}

	float lastThrustUpdateTime = -999f;

	void Update() {
		centerOfMass = ComputeCenterOfMass();
		balancePenalty = centerOfMass.magnitude / maxBalancePenaltyMeters;
		mass = ShipMass.MassScale * GetTotalMass();
		
		var t = (Time.time - startTime) / thrustHalflife;
		var thrustMulti = ThrustMultiplier / (1f + t);
		realthrust = GetThrustCount() * thrustMulti;
		speed = mass > Mathf.Epsilon ? (1f - balancePenalty) * realthrust / mass : 0f;

		if (Time.time - lastThrustUpdateTime > 1f) {
			thrust = realthrust;
			lastThrustUpdateTime = Time.time;
		}

	}

	Vector2 ComputeCenterOfMass() {
		var origin = transform.XY();
		var accum = Vector2.zero;
		var weight = 0f;
		foreach(var mass in masses) {
			if (mass.mass > Mathf.Epsilon) {
				weight += mass.mass;
				accum += mass.mass * (mass.transform.XY() - origin);
			}
		}

		return weight > Mathf.Epsilon ? 
			accum / weight : 
			Vector2.zero;

	}

	public float GetTotalMass() {
		var result = 0f;
		for(int it=0; it<masses.Count; ++it)
			result += masses[it].mass;
		return result;
	}

	public int GetTotalBooty() {
		var result = 0;
		for(int it=0; it<booty.Count; ++it)
			result += booty[it].treasureAmount;
		return result;
	}

	public int GetThrustCount() {
		var result = 0;
		for(int it=0; it<thrusters.Count; ++it) {
			if (thrusters[it].IsThrusting)
				++result;
		}
		return result;
	}


}
