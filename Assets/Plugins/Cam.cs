using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour {

	public Transform target;
	public float smoothTime = 1f;

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
	}

}
