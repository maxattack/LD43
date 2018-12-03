using UnityEngine;

public class Booty : MonoBehaviour {

	public int treasureAmount = 100;

	void Start() {
		if (Ship.inst) {
			Ship.inst.booty.Add(this);
			var prop = GetComponent<Prop>();
			if (prop)
				prop.description = Ship.inst.GetBootyName(prop.description);
		}
	}

	void OnDestroy() {
		if (Ship.inst)
			Ship.inst.booty.Remove(this);
	}

}
