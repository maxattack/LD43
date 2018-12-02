using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : MonoBehaviour, CrewMember.Interactable, CrewMember.Pickupable {

	public string description = "Gold Doubloons";
	BoxCollider2D box;

	bool isPickedUp = false;

	void Awake() {
		box = GetComponent<BoxCollider2D>();
	}

	// Interactable

	Transform CrewMember.Interactable.IndicatorRoot {
		get { return transform; }
	}

	string CrewMember.Interactable.GetDescription() {
		var booty = GetComponent<Booty>();
		var mass = GetComponent<ShipMass>();

		var result = description;

		if (booty)
			result += ("\nBooty: $" + booty.treasureAmount);
		if (mass)
			result += ("\nMass: " + mass.ReadableMass + "kg");

		return result;
	}

	bool CrewMember.Interactable.CanReceiveFocus {
		get { return !isPickedUp; }
	}

	void CrewMember.Interactable.OnReceiveFocus(CrewMember crew) {
	}

	void CrewMember.Interactable.OnActionPerformed(CrewMember crew) {
		crew.TryPickup(this);
	}

	void CrewMember.Interactable.OnLoseFocus(CrewMember crew) {
	}

	// Pickupable

	Transform CrewMember.Pickupable.RootTransform {
		get { return transform; }
	}

	Vector2 CrewMember.Pickupable.BoxSize {
		get { return box.size; }
	}

	void CrewMember.Pickupable.OnPickup(CrewMember crew) {
		isPickedUp = true;
		foreach(var it in GetComponentsInChildren<Collider2D>())
			it.enabled = false;
	}

	void CrewMember.Pickupable.OnActionPerformed(CrewMember crew) {
		crew.TryPutDown();
	}

	void CrewMember.Pickupable.OnDropoff(CrewMember crew) {
		isPickedUp = false;
		foreach (var it in GetComponentsInChildren<Collider2D>())
			it.enabled = true;
	}

}

