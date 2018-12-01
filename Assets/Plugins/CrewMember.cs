using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrewMember : MonoBehaviour {

	// public parameters
	public Transform pivotRoot;
	public Transform pickupRoot;
	public Transform dropoffRoot;
	public Transform indicatorRoot;
	public float targetSpeed = 5f;
	public float rotationSmoothTime = 0.1f;
	public float pickupTime = 0.5f;
	public float pickupHeight = 0.5f;

	// cached components
	internal Rigidbody2D body;

	// pivot properties
	internal float currentDirection = 0f;
	internal float targetDirection = 0f;
	internal float dirVelocity = 0f;

	// state machine 
	public enum Status {
		Idle,
		PickingUp,
		HoldingPickup,
		PuttingDown
	}
	internal Status status = Status.Idle;
	float statusTime;

	// interactables
	public interface Interactable {
		Transform IndicatorRoot { get; }
		bool CanReceiveFocus { get; }
		void OnReceiveFocus(CrewMember crew);
		void OnActionPerformed(CrewMember crew);
		void OnLoseFocus(CrewMember crew);
	}
	internal Interactable focus;

	// pickups
	public interface Pickupable {
		Transform RootTransform { get; }
		void OnPickup(CrewMember crew);
		void OnActionPerformed(CrewMember crew);
		void OnDropoff(CrewMember crew);
	}
	internal Pickupable pickup;

	// controller methods

	public void Accelerate(Vector2 inputDir) {
		var force = targetSpeed * body.drag;
		body.AddForce(inputDir * force, ForceMode2D.Force);
		targetDirection = Mathf.Rad2Deg * Mathf.Atan2(inputDir.y, inputDir.x);
	}

	public void TryAction() {
		if (focus != null && status == Status.Idle)
			focus.OnActionPerformed(this);

		if (pickup != null && status == Status.HoldingPickup)
			pickup.OnActionPerformed(this);
	}

	public void TryPickup(Pickupable aPickup) {
		var earlyOut = 
			status != Status.Idle ||
			pickup != null;
		if (earlyOut)
			return;

		if (focus != null) {
			var receiver = focus;
			focus = null;
			receiver.OnLoseFocus(this);
		}

		pickup = aPickup;
		SetStatus(Status.PickingUp);

		pickup.OnPickup(this);
		StartCoroutine(DoPickup());
	}

	public void TryPutDown() {
		var earlyOut = 
			status != Status.HoldingPickup ||
			pickup == null;
		if (earlyOut)
			return;

		SetStatus(Status.PuttingDown);
		StartCoroutine(DoPutdown());

	}

	// helpers

	void SetStatus(Status aStatus) {
		status = aStatus;
		statusTime = Time.time;
	}
	
	float StatusTimeElapsed {
		get { return Time.time - statusTime; }
	}

	static float UnitParabola(float x) {
		float xx = 1f - x - x;
		return 1f - xx * xx;
	}

	IEnumerator DoPickup() {

		body.isKinematic = true;
		body.velocity = Vector2.zero;

		var xf = pickup.RootTransform;
		var startLoc = xf.position;
		var startRot = xf.rotation;
		while(StatusTimeElapsed < pickupTime) {
			var progress = StatusTimeElapsed / pickupTime;
			xf.position = 
				UnitParabola(progress) * pickupHeight * (-Vector3.forward) +  
				Vector3.Lerp(startLoc, pickupRoot.position, progress);
			//xf.rotation = Quaternion.Slerp(startRot, pickupRoot.rotation, progress);
			yield return null;
		}

		SetStatus(Status.HoldingPickup);
		xf.parent = pickupRoot;
		xf.localPosition = Vector3.zero;

		body.isKinematic = false;

	}

	IEnumerator DoPutdown() {

		body.isKinematic = true;
		body.velocity = Vector2.zero;

		var xf = pickup.RootTransform;
		xf.parent = null;
		var startLoc = xf.position;
		var endLoc = dropoffRoot.position;
		while (StatusTimeElapsed < pickupTime) {
			var progress = StatusTimeElapsed / pickupTime;
			xf.position = 
				UnitParabola(progress) * pickupHeight * (-Vector3.forward) + 
				Vector3.Lerp(startLoc, endLoc, progress);
			yield return null;
		}

		xf.position = endLoc;
		var receiver = pickup;
		pickup = null;
		SetStatus(Status.Idle);

		body.isKinematic = false;

		receiver.OnDropoff(this);
	}

	// unity events

	void Awake() {
		if (pivotRoot == null)
			pivotRoot = transform;

		body = GetComponent<Rigidbody2D>();
		currentDirection = targetDirection = pivotRoot.eulerAngles.z;
	}

	void Update() {
		var shouldRotate = status == Status.Idle || status == Status.HoldingPickup;
		if (shouldRotate) {
			currentDirection = Mathf.SmoothDampAngle(currentDirection, targetDirection, ref dirVelocity, rotationSmoothTime);
			pivotRoot.localEulerAngles = new Vector3(0f, 0f, currentDirection);
		}

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
			pickup != null ||
			prop == null || 
			prop == focus || 
			!prop.CanReceiveFocus;
		if (earlyOut)
			return;

		if (focus != null)
			focus.OnLoseFocus(this);

		focus = prop;
		prop.OnReceiveFocus(this);
	}

	void OnTriggerExit2D(Collider2D collision) {
		var prop = collision.GetComponent<Interactable>();
		if (prop != null && prop == focus) {
			focus = null;
			prop.OnLoseFocus(this);
		}
	}
}
