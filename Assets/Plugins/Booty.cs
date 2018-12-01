using UnityEngine;

public class Booty : MonoBehaviour {

	public int treasureAmount = 100;

	void Start() {
		if (Ship.inst)
			Ship.inst.booty.Add(this);
	}

	void OnDestroy() {
		if (Ship.inst)
			Ship.inst.booty.Remove(this);
	}

}
