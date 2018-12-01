using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {

	public Transform indicatorRoot;
	SpriteRenderer indicatorSprite;

	int crewIndex = 0;
	List<CrewMember> crewMembers = new List<CrewMember>();


	KeyCode? mostRecentKeycode;
	KeyCode[] WASD = new KeyCode[4] { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D };

	public bool IsCrewMemberSelected { get { return crewIndex < crewMembers.Count; } }
	public CrewMember SelectedCrewMember { get { return crewMembers[crewIndex]; } }

	void Awake() {
		indicatorSprite = indicatorRoot.GetComponentInChildren<SpriteRenderer>();
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

			for (int it=0; it<WASD.Length; ++it) {
				if (Input.GetKeyDown(WASD[it])) {
					mostRecentKeycode = WASD[it];
					break;
				}
			}

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

		if (crewMembers.Count > 1) {
			if (Input.GetKeyDown(KeyCode.LeftShift)) {
				crewIndex = (crewIndex + crewMembers.Count - 1) % crewMembers.Count;
			} else if (Input.GetKeyDown(KeyCode.RightShift)) {
				crewIndex = (crewIndex + 1) % crewMembers.Count;
			}
		}


		if (IsCrewMemberSelected) {
			transform.position = SelectedCrewMember.transform.position;
			if (Input.GetKeyDown(KeyCode.Space))
				SelectedCrewMember.TryAction();
		} 

		var focus = IsCrewMemberSelected ? SelectedCrewMember.focus : null;
		if (focus == null) {
			indicatorSprite.enabled = false;
		} else {
			indicatorSprite.enabled = true;
			indicatorRoot.position = focus.IndicatorRoot.position;
		}

	}



}
