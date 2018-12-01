using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrewMember : MonoBehaviour {

	public Transform pivot;
	public Transform pickup;
	public float targetSpeed = 5f;
	public float rotationSmoothTime = 0.1f;

	internal float currentDirection = 0f;
	internal float targetDirection = 0f;
	internal float dirVelocity = 0f;
	internal Rigidbody2D body;

	public interface Interactable {
		bool CanReceiveFocus { get; }
		void OnReceiveFocus(CrewMember crew);
		void OnActionPerformed(CrewMember crew);
		void OnLoseFocus(CrewMember crew);

	}

	public interface Pickupable {
		Transform RootTransform { get; }
		void OnPickup(CrewMember crew);
		void OnDropoff(CrewMember crew, Vector2 location);
	}

	internal Interactable interactableInFocus;


	public void Accelerate(Vector2 inputDir) {
		var force = targetSpeed * body.drag;
		body.AddForce(inputDir * force, ForceMode2D.Force);

		targetDirection = Mathf.Rad2Deg * Mathf.Atan2(inputDir.y, inputDir.x);
	}

	public void TryAction() {
		if (interactableInFocus != null) {
			interactableInFocus.OnActionPerformed(this);
		}
	}

	void Awake() {
		if (pivot == null)
			pivot = transform;

		body = GetComponent<Rigidbody2D>();
		currentDirection = targetDirection = pivot.eulerAngles.z;
	}

	void Update() {
		currentDirection = Mathf.SmoothDampAngle(currentDirection, targetDirection, ref dirVelocity, rotationSmoothTime);
		pivot.localEulerAngles = new Vector3(0f, 0f, currentDirection);
	}


	void OnTriggerEnter2D(Collider2D collision) {
		TryFocus(collision);
	}

	void OnTriggerStay2D(Collider2D collision) {
		TryFocus(collision);
	}

	void TryFocus(Collider2D collision) {
		var prop = collision.GetComponent<Interactable>();
		var earlyOut = 
			prop == null || 
			prop == interactableInFocus || 
			!prop.CanReceiveFocus;
		if (earlyOut)
			return;

		if (interactableInFocus != null)
			interactableInFocus.OnLoseFocus(this);

		interactableInFocus = prop;
		prop.OnReceiveFocus(this);
	}

	void OnTriggerExit2D(Collider2D collision) {
		var prop = collision.GetComponent<Interactable>();
		if (prop != null && prop == interactableInFocus) {
			prop.OnLoseFocus(this);
			interactableInFocus = null;
		}
	}
}
