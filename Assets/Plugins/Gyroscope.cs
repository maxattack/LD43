using UnityEngine;
using UnityEngine.UI;

public class Gyroscope : MonoBehaviour {

	public RectTransform rollSlider;
	public RectTransform pitchSlider;
	public float slidePerMeter = 100f;

	void Update () {

		if (Ship.inst) {
			var com = Ship.inst.centerOfMass;
			var noRotation = com.sqrMagnitude < Mathf.Epsilon;
			if (noRotation) {
				rollSlider.anchoredPosition = Vector2.zero;
				pitchSlider.anchoredPosition = Vector2.zero;
			} else {
				rollSlider.anchoredPosition = new Vector2(Mathf.Clamp(slidePerMeter * com.x, -100f, 100f), 0f);
				pitchSlider.anchoredPosition = new Vector2(Mathf.Clamp(slidePerMeter * com.y, -100f, 100f), 0f);
			}

		}

		

	}
}
