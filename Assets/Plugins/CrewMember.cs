using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrewMember : MonoBehaviour {

	public Transform pivot;
	public float targetSpeed = 5f;
	public float rotationSmoothTime = 0.1f;

	internal float currentDirection = 0f;
	internal float targetDirection = 0f;
	internal float dirVelocity = 0f;
	internal Rigidbody2D body;

	void Awake() {
		if (pivot == null)
			pivot = transform;

		body = GetComponent<Rigidbody2D>();
		currentDirection = targetDirection = pivot.eulerAngles.z;
	}

	public void Accelerate(Vector2 inputDir) {
		var force = targetSpeed * body.drag;
		body.AddForce(inputDir * force, ForceMode2D.Force);

		targetDirection = Mathf.Rad2Deg * Mathf.Atan2(inputDir.y, inputDir.x);
	}

	void Update() {
		currentDirection = Mathf.SmoothDampAngle(currentDirection, targetDirection, ref dirVelocity, rotationSmoothTime);
		pivot.localEulerAngles = new Vector3(0f, 0f, currentDirection);
	}



}
