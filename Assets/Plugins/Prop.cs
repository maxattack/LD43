using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : MonoBehaviour, CrewMember.Interactable {

	public SpriteRenderer indicator;

	public enum Status {
		Idle,
		PickingUp,
		PickedUp,
		PuttingDown
	}

	Status status = Status.Idle;

	bool CrewMember.Interactable.CanReceiveFocus {
		get {
			return status == Status.Idle;
		}
	}

	void CrewMember.Interactable.OnReceiveFocus(CrewMember crew) {
		indicator.enabled = true;
	}

	void CrewMember.Interactable.OnLoseFocus(CrewMember crew) {
		indicator.enabled = false;
	}

	void CrewMember.Interactable.OnActionPerformed(CrewMember crew) {
		transform.parent = crew.pickup;
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;
		status = Status.PickedUp;
	}

}
