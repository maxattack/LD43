using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrewMember : MonoBehaviour, CrewMember.Interactable {

	// public parameters
	public string description = "Another Redshirt";
	public Transform pivotRoot;
	public Transform cardinalRoot;
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
		string GetDescription();
		bool CanReceiveFocus { get; }
		void OnReceiveFocus(CrewMember crew);
		void OnActionPerformed(CrewMember crew);
		void OnLoseFocus(CrewMember crew);
	}
	internal Interactable focus;

	// pickups
	public interface Pickupable {
		Transform RootTransform { get; }
		Vector2 BoxSize { get; }
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

	public bool GetDropoffLocation(out Vector2 location, out Slot slot) {
		var targetLoc = dropoffRoot.position;


		var hammer = pickup.RootTransform.GetComponentInChildren<Hammer>();
		if (hammer != null) {
			targetLoc.x = Mathf.Floor(targetLoc.x) + 0.5f;
			targetLoc.y = Mathf.Floor(targetLoc.y) + 0.5f;
		}

		location = targetLoc;
		slot = null;
		if (pickup == null)
			return false;

		var sz = pickup.BoxSize;
		var rotation = cardinalRoot.eulerAngles.z;
		var colliders = Physics2D.OverlapBoxAll(targetLoc, sz, rotation, 0xffffff);
		foreach(var c in colliders) {
			var s = c.GetComponent<Slot>();
			if (s && s.IsEmpty) {
				location = s.transform.position;
				slot = s;
				return true;
			} else if (!c.isTrigger) {
				return false;
			}
		}

		return true;
	}

	public bool GetDropoffLocation(out Vector2 location) {
		Slot _;
		return GetDropoffLocation(out location, out _);
	}

	public void TryPutDown() {
		var earlyOut = 
			status != Status.HoldingPickup ||
			pickup == null;
		if (earlyOut)
			return;

		Vector2 dropLoc;
		Slot slot;
		if (GetDropoffLocation(out dropLoc, out slot)) {
			SetStatus(Status.PuttingDown);
			StartCoroutine(DoPutdown(dropLoc, slot));
		}


	}

	// helpers

	void SetStatus(Status aStatus) {
		status = aStatus;
		statusTime = Time.time;
	}
	
	float StatusTimeElapsed {
		get { return Time.time - statusTime; }
	}

	
	IEnumerator DoPickup() {

		body.isKinematic = true;
		body.velocity = Vector2.zero;

		var xf = pickup.RootTransform;

		SlotConnection.Detach(xf);

		var startLoc = xf.position;
		var startRot = xf.rotation;
		while(StatusTimeElapsed < pickupTime) {
			var progress = StatusTimeElapsed / pickupTime;
			xf.position = 
				Util.UnitParabola(progress) * pickupHeight * (-Vector3.forward) +  
				Vector3.Lerp(startLoc, pickupRoot.position, progress);
			//xf.rotation = Quaternion.Slerp(startRot, pickupRoot.rotation, progress);
			yield return null;
		}

		SetStatus(Status.HoldingPickup);

		xf.parent = pickupRoot;
		xf.SnapRotation();

		xf.localPosition = Vector3.zero;

		body.isKinematic = false;

	}

	IEnumerator DoPutdown(Vector2 dropLoc, Slot slot) {

		body.isKinematic = true;
		body.velocity = Vector2.zero;

		var xf = pickup.RootTransform;

		xf.parent = null;
		xf.SnapRotation();

		var startLoc = xf.position;
		while (StatusTimeElapsed < pickupTime) {
			var progress = StatusTimeElapsed / pickupTime;
			xf.position =
				Util.UnitParabola(progress) * pickupHeight * (-Vector3.forward) + 
				Vector3.Lerp(startLoc, dropLoc, progress);
			yield return null;
		}

		xf.position = dropLoc;
		var receiver = pickup;
		pickup = null;
		SetStatus(Status.Idle);

		body.isKinematic = false;

		receiver.OnDropoff(this);

		if (slot != null)
			slot.TryFill(xf);
	}

	// unity events

	void Awake() {
		if (pivotRoot == null)
			pivotRoot = transform;

		body = GetComponent<Rigidbody2D>();
		currentDirection = targetDirection = pivotRoot.eulerAngles.z;
	}

	void Update() {
		currentDirection = Mathf.SmoothDampAngle(currentDirection, targetDirection, ref dirVelocity, rotationSmoothTime);
		pivotRoot.localEulerAngles = new Vector3(0f, 0f, currentDirection);
		cardinalRoot.localEulerAngles = new Vector3(0f, 0f, targetDirection);
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

	// interactable
	Transform Interactable.IndicatorRoot {
		get { return transform; }
	}

	string Interactable.GetDescription() {
		return description;
	}

	bool Interactable.CanReceiveFocus {
		get { return true; }
	}

	void Interactable.OnReceiveFocus(CrewMember crew) {

	}

	void Interactable.OnActionPerformed(CrewMember crew) {

	}

	void Interactable.OnLoseFocus(CrewMember crew) {

	}


}
