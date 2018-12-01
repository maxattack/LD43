using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrewMember : MonoBehaviour {

	public float targetSpeed = 5f;
	internal Rigidbody2D body;

	void Awake() {
		//body = GetComponent<Rigidbody>();
		body = GetComponent<Rigidbody2D>();
	}

	public void Accelerate(Vector2 inputDir) {
		var force = targetSpeed * body.drag;
		body.AddForce(inputDir * force, ForceMode2D.Force);
	}



}
