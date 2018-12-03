using UnityEngine;

public class Music : MonoBehaviour {

	static Music inst;

	private void Awake() {
		if (inst) {
			Destroy(gameObject);
		} else {
			inst = this;
			DontDestroyOnLoad(gameObject);
		}
	}

	private void OnDestroy() {
		if (inst == this)
			inst = null;
	}

}
