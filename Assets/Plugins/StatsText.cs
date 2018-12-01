using UnityEngine;
using UnityEngine.UI;

public class StatsText : MonoBehaviour {

	public Text statsText;

	void LateUpdate () {
		var totalMass = Mathf.RoundToInt(100f * Ship.inst.GetTotalMass());
		var treasure = Ship.inst.GetTotalBooty();
		statsText.text = string.Format("{0}\n{1}\n{2}\n${3}", totalMass, 0, 0, treasure);
	}
}
