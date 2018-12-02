using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thruster : MonoBehaviour, Slot.Listener {

	public GameObject flame;

	internal FuelCell cell;

	void Awake() {
		flame.SetActive(false);	
	}

	void Slot.Listener.SlotFilled(Slot slot, CrewMember.Pickupable pickup) {
		var fuelCell = pickup.RootTransform.GetComponent<FuelCell>();
		if (fuelCell) {
			cell = fuelCell;
		}
	}

	void Slot.Listener.SlotEmptied(Slot slot) {
		cell = null;
	}

	void Update() {
		var thrust = cell && cell.HasFuel;
		if (thrust) {
			if (!flame.active)
				flame.SetActive(true);

			cell.DrainFuel(Time.deltaTime);

		} else {
			if (flame.active)
				flame.SetActive(false);
		}

	}

}
