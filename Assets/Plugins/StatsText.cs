using UnityEngine;
using UnityEngine.UI;

public class StatsText : MonoBehaviour {

	public Text statsText;

	void LateUpdate () {
		var mass = ShipMass.MassScale * Ship.inst.GetTotalMass();
		var totalMass = Mathf.RoundToInt(mass);
		var treasure = "$ " + Ship.inst.GetTotalBooty();
		var thrust = 10000 * Ship.inst.GetThrustCount();
		var com = Ship.inst.centerOfMass;

		var bp = com.magnitude / Ship.inst.maxBalancePenaltyMeters;
		var balancePenalty = Mathf.RoundToInt(100f * bp) + "%";

		var speed = mass > Mathf.Epsilon ? 
			Mathf.RoundToInt((1f - bp) * thrust / mass) : 0f;

		statsText.text = string.Format("{0}\n{1}\n{2}\n{3}\n{4}", treasure, thrust, totalMass, balancePenalty, speed);
	}
}
