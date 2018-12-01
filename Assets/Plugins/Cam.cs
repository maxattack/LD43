using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour {

	public Transform target;
	public float smoothTime = 1f;
	public float tiltPerMeter = 5f;

	Vector2 targetPos;
	Vector2 velocity = Vector3.zero;

	void Awake() {
		targetPos = transform.position;
	}

	void LateUpdate() {
		if (target)
			targetPos = target.position;

		var position = transform.position;
		position.x = Mathf.SmoothDamp(position.x, targetPos.x, ref velocity.x, smoothTime);
		position.y = Mathf.SmoothDamp(position.y, targetPos.y, ref velocity.y, smoothTime);
		transform.position = position;

		if (Ship.inst) {
			var com = Ship.inst.centerOfMass;
			var noRotation = com.sqrMagnitude < Mathf.Epsilon;
			if (noRotation) {
				transform.rotation = Quaternion.identity;
			} else {
				var offset = com.magnitude;
				var axis = new Vector3(-com.y, com.x, 0f) / offset;
				var angle = offset * tiltPerMeter;
				transform.rotation = Quaternion.AngleAxis(angle, axis);

			}
		}
	}

}
