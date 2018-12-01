using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {

	int crewIndex = 0;
	List<CrewMember> crewMembers = new List<CrewMember>();

	void Start() {
		crewMembers.AddRange(GameObject.FindObjectsOfType<CrewMember>());
	}

	void FixedUpdate() {

		if (crewIndex < crewMembers.Count) {
			var crewMember = crewMembers[crewIndex];
			var input = Vector2.zero;
			if (Input.GetKey(KeyCode.W))
				input.y = 1f;
			else if (Input.GetKey(KeyCode.S))
				input.y = -1f;
			if (Input.GetKey(KeyCode.A))
				input.x = -1f;
			else if (Input.GetKey(KeyCode.D))
				input.x = 1f;

			if (input.sqrMagnitude > Mathf.Epsilon) {
				crewMember.Accelerate(input);
			}
		}
	}

	void Update() {

		if (crewMembers.Count > 1) {
			if (Input.GetKeyDown(KeyCode.LeftShift)) {
				crewIndex = crewIndex = (crewIndex + crewMembers.Count - 1) % crewMembers.Count;
			} else if (Input.GetKeyDown(KeyCode.RightShift)) {
				crewIndex = crewIndex = (crewIndex + 1) % crewMembers.Count;
			}
		}

		if (crewIndex < crewMembers.Count) {
			var crewMember = crewMembers[crewIndex];
			transform.position = crewMember.transform.position;
		}
	}
}
