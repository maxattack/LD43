using UnityEngine;

public class SpeedDust : MonoBehaviour {

	public float speedToDist = 1f;
	public float rotationRate = 10f;

	float rotRate;

	void Awake() {
		rotRate = Random.Range(-rotationRate, rotationRate);
	}

	void Update() {
		if (!Ship.inst) {
			Destroy(gameObject);
			return;
		}

		transform.Rotate(new Vector3(0f, 0f, rotRate * Time.deltaTime));

		var speed = Ship.inst.speed;
		var offset = speed * speedToDist * Time.deltaTime;
		var pos = transform.position + new Vector3(0, -offset, 0);
		var cam = Camera.main;
		if (cam.WorldToScreenPoint(pos).y < -10f) {
			Destroy(gameObject);
			return;
		} else {
			transform.position = pos;
		}
	}

}
