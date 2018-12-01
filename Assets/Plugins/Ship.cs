using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour {

	public static Ship inst;
	public List<ShipMass> masses;
	internal Vector2 centerOfMass;


	void Awake() {
		inst = this;
	}

	void OnDestroy() {
		if (inst == this)
			inst = null;
	}

	void Update() {
		centerOfMass = ComputeCenterOfMass();
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

}
