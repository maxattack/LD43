﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour {

	public Transform item;

	public interface Listener {
		void SlotFilled(Slot slot, Transform item);
		void SlotEmptied(Slot slot, Transform item);
	}

	void Start() {
		if (item)
			DoConnect(item);
	}

	public bool TryFill(Transform aItem) {
		if (item != null)
			return false;

		DoConnect(aItem);
		return true;
	}

	void DoConnect(Transform t) {
		item = t;
		SlotConnection.GetConnection(item).connectedSlot = this;
		var listener = GetComponent<Listener>();
		if (listener != null)
			listener.SlotFilled(this, item);
	}

	public bool TryEmpty() {
		if (!item)
			return false;

		DoDisconnect(item);
		return true;
	}

	internal void DoDisconnect(Transform t) {
		item = null;
		var listener = GetComponent<Listener>();
		if (listener != null)
			listener.SlotEmptied(this, t);
	}

	public bool IsEmpty {
		get {
			return !item;
		}
	}


}

public class SlotConnection : MonoBehaviour {
	internal Slot connectedSlot = null;

	internal static SlotConnection GetConnection(Transform t) {
		var result = t.GetComponent<SlotConnection>();
		if (result == null)
			result = t.gameObject.AddComponent<SlotConnection>();
		return result;
	}

	internal static void Detach(Transform t) {
		var connection = t.GetComponent<SlotConnection>();
		if (connection && connection.connectedSlot) {
			connection.connectedSlot.TryEmpty();
			connection.connectedSlot = null;
		}
	}

	void OnDestroy() {
		if (connectedSlot != null && connectedSlot.item == this) {
			connectedSlot.DoDisconnect(transform);
		}
	}
}