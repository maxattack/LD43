using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour {

	public float titleFrequency = 1f;
	public float titleRotation = 20f;

	public Transform titleText;

	void Update () {
		
		var z = titleRotation * Mathf.Sin(Mathf.PI * 2f * Time.time * titleFrequency);
		titleText.eulerAngles = new Vector3(0f, 0f, z);

		if (Time.time > 0.5f && Input.anyKeyDown) {
			SceneManager.LoadScene(1);
		}

	}
}
