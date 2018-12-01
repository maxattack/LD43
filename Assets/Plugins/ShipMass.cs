using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMass : MonoBehaviour {

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

}
