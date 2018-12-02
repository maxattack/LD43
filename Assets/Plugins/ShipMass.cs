using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMass : MonoBehaviour {

	public const float MassScale = 1f;
	public float mass = 1f;

	void Start () {
		if (Ship.inst) {
			Ship.inst.masses.Add(this);
		}
	}

	void OnDestroy() {
		if (Ship.inst)
			Ship.inst.masses.Remove(this);
		
	}

	public int ReadableMass {
		get { return Mathf.RoundToInt(mass * MassScale); }

	}


}
