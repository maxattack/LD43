using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {

	public Transform selectionRect;
	public Color badDropColor = Color.red;
	public Color okDropColor = Color.white;
	SpriteRenderer selectionSprite;

	public Transform indicatorRoot;
	SpriteRenderer indicatorSprite;
	TextMesh detailsText;

	int crewIndex = 0;
	List<CrewMember> crewMembers = new List<CrewMember>();


	KeyCode? mostRecentKeycode;
	KeyCode[] WASD = new KeyCode[4] { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D };

	public bool IsCrewMemberSelected { get { return crewIndex < crewMembers.Count; } }
	public CrewMember SelectedCrewMember { get { return crewMembers[crewIndex]; } }

	void Awake() {
		indicatorSprite = indicatorRoot.GetComponentInChildren<SpriteRenderer>();
		detailsText = indicatorRoot.GetComponentInChildren<TextMesh>();

		selectionSprite = selectionRect.GetComponentInChildren<SpriteRenderer>();
	}

	void Start() {
		crewMembers.AddRange(GameObject.FindObjectsOfType<CrewMember>());
	}

	Vector2 KeyToDirection(KeyCode key) {
		switch(key) {
		case KeyCode.W: return new Vector2(0, 1);
		case KeyCode.A: return new Vector2(-1, 0);
		case KeyCode.S: return new Vector2(0, -1);
		case KeyCode.D: return new Vector2(1, 0);
		default: return Vector2.zero;
		}
	}

	void FixedUpdate() {

		if (crewIndex < crewMembers.Count) {
			var crewMember = crewMembers[crewIndex];

			var input = Vector2.zero;

			if (mostRecentKeycode.HasValue) {
				if (Input.GetKey(mostRecentKeycode.Value)) {
					input = KeyToDirection(mostRecentKeycode.Value);
				} else {
					mostRecentKeycode = null;
				}
			}
			if (!mostRecentKeycode.HasValue) {
				for(int it=0; it<WASD.Length; ++it) {
					if (Input.GetKey(WASD[it])) {
						input = KeyToDirection(WASD[it]);
						break;
					}

				}
			}

			if (input.sqrMagnitude > Mathf.Epsilon) {
				crewMember.Accelerate(input);
			}
		}
	}

	void Update() {

		if (Time.time > 0.25f && Input.GetKeyDown(KeyCode.Escape))
			Application.Quit();

		for (int it = 0; it < WASD.Length; ++it) {
			if (Input.GetKeyDown(WASD[it])) {
				mostRecentKeycode = WASD[it];
				break;
			}
		}



		if (crewMembers.Count > 1) {
			var sfx = GetComponent<AudioSource>();
			if (Input.GetKeyDown(KeyCode.Q)) {
				crewIndex = (crewIndex + crewMembers.Count - 1) % crewMembers.Count;
				if (sfx) sfx.Play();
			} else if (Input.GetKeyDown(KeyCode.E)) {
				crewIndex = (crewIndex + 1) % crewMembers.Count;
				if (sfx) sfx.Play();
			}
		}


		if (IsCrewMemberSelected) {
			transform.position = SelectedCrewMember.transform.position;
			if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.Space))
				SelectedCrewMember.TryAction();
		} 

		var focus = IsCrewMemberSelected ? SelectedCrewMember.focus : null;
		if (focus == null) {
			if (indicatorSprite.enabled) {
				indicatorSprite.enabled = false;
				detailsText.text = "";
			}
		} else {
			indicatorSprite.enabled = true;
			detailsText.text = focus.GetDescription();
			indicatorRoot.position = focus.IndicatorRoot.position;

		}

		var pickup = IsCrewMemberSelected ? SelectedCrewMember.pickup : null;
		if (pickup == null) {
			selectionSprite.enabled = false;
		} else {
			selectionSprite.enabled = true;
			Vector2 dropLoc;
			selectionSprite.color = SelectedCrewMember.GetDropoffLocation(out dropLoc) ?
				okDropColor : badDropColor;
			selectionRect.position = dropLoc;
			var size = pickup.BoxSize;
			selectionRect.localScale = new Vector3(size.x, size.y, 1f);
		}

	}



}
