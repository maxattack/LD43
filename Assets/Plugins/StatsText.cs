using UnityEngine;
using UnityEngine.UI;

public class StatsText : MonoBehaviour {

	public Text statsText;

	void LateUpdate () {
		var treasure = "$ " + Ship.inst.GetTotalBooty();
		var thrust = Ship.inst.thrust;
		var totalMass = Mathf.RoundToInt(Ship.inst.mass);
		var balancePenalty = Mathf.RoundToInt(100f * Ship.inst.balancePenalty) + "%";
		var speed = Mathf.RoundToInt(Ship.inst.speed);
		statsText.text = string.Format("{0}\n{1}\n{2}\n{3}\n{4}", treasure, thrust, totalMass, balancePenalty, speed);
	}
}
