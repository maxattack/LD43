using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour {

	public interface Listener {
		void SlotFilled(Slot slot, CrewMember.Pickupable pickup);
		void SlotEmptied(Slot slot);
	}

	CrewMember.Pickupable pickup;

	public bool TryFill(CrewMember.Pickupable aPickup) {
		if (pickup != null)
			return false;

		pickup = aPickup;
		SlotConnection.GetConnection(pickup).connectedSlot = this;
		var listener = GetComponent<Listener>();
		if (listener != null)
			listener.SlotFilled(this, pickup);
		return true;
	}

	public bool TryEmpty() {
		if (pickup == null)
			return false;

		pickup = null;
		var listener = GetComponent<Listener>();
		if (listener != null)
			listener.SlotEmptied(this);
		return true;
	}

	public bool IsEmpty {
		get {
			return pickup == null;
		}
	}


}

public class SlotConnection : MonoBehaviour {
	internal Slot connectedSlot = null;

	internal static SlotConnection GetConnection(CrewMember.Pickupable pickup) {
		var result = pickup.RootTransform.GetComponent<SlotConnection>();
		if (result == null)
			result = pickup.RootTransform.gameObject.AddComponent<SlotConnection>();
		return result;
	}
}