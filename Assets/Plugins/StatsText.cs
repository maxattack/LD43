using UnityEngine;
using UnityEngine.UI;

public class StatsText : MonoBehaviour {

	public Text statsText;

	void LateUpdate () {
		var mass = ShipMass.MassScale * Ship.inst.GetTotalMass();
		var totalMass = Mathf.RoundToInt(mass);
		var treasure = Ship.inst.GetTotalBooty();
		var thrust = 10000 * Ship.inst.GetThrustCount();

		var speed = mass > Mathf.Epsilon ? 
			Mathf.RoundToInt(thrust / mass) : 0f;

		statsText.text = string.Format("{0}\n{1}\n{2}\n${3}", thrust, totalMass, speed, treasure);
	}
}
