using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : MonoBehaviour, CrewMember.Interactable, CrewMember.Pickupable {

	bool isPickedUp = false;

	// Interactable

	Transform CrewMember.Interactable.IndicatorRoot {
		get { return transform; }
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

