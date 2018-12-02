using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thruster : MonoBehaviour, Slot.Listener {

	public GameObject flame;

	internal FuelCell cell;

	public bool IsThrusting {
		get { return flame.activeInHierarchy; }
	}

	void Awake() {
		flame.SetActive(false);	
	}

	void Start() {
		if (Ship.inst)
			Ship.inst.thrusters.Add(this);
	}

	void OnDestroy() {
		if (Ship.inst)
			Ship.inst.thrusters.Remove(this);
	}

	void Slot.Listener.SlotFilled(Slot slot, Transform item) {
		var fuelCell = item.GetComponent<FuelCell>();
		if (fuelCell) {
			cell = fuelCell;
		}
	}

	void Slot.Listener.SlotEmptied(Slot slot, Transform item) {
		cell = null;
	}

	void Update() {
		var thrust = cell && cell.HasFuel;
		if (thrust) {
			if (!flame.activeInHierarchy)
				flame.SetActive(true);

			cell.DrainFuel(Time.deltaTime);

		} else {
			if (flame.activeInHierarchy)
				flame.SetActive(false);
		}

	}

}
