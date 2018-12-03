using UnityEngine;

public class SpeedDust : MonoBehaviour {

	public float speedToDist = 1f;
	public float rotationRate = 10f;

	float rotRate;

	// for the prefab, this points at the next available instance, for
	// instances this points back at the prefab
	SpeedDust next = null;

	public void Create(Vector3 pos) {
		if (next != null) {
			var result = next;
			next = next.next;

			result.gameObject.SetActive(true);
			result.transform.position = pos;
			result.next = this;


		} else {

			var result = Instantiate(this, pos, Quaternion.identity);
			result.hideFlags = HideFlags.HideInHierarchy;
			result.next = this;

		}
	}

	public void Release() {
		if (next == null) {
			Destroy(gameObject);
		} else {
			gameObject.SetActive(false);
			var prefab = next;
			next = prefab.next;
			prefab.next = this;
		}
	}

	void Awake() {
		rotRate = Random.Range(-rotationRate, rotationRate);
	}

	void Update() {
		if (!Ship.inst) {
			Release();
			return;
		}

		transform.Rotate(new Vector3(0f, 0f, rotRate * Time.deltaTime));

		var speed = Ship.inst.speed;
		var offset = speed * speedToDist * Time.deltaTime;
		var pos = transform.position + new Vector3(0, -offset, 0);
		var cam = Camera.main;
		if (cam.WorldToScreenPoint(pos).y < -10f) {
			Release();
			return;
		} else {
			transform.position = pos;
		}
	}

}
