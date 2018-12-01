using UnityEngine;

public static class Util {

	public static float UnitParabola(float x) {
		float xx = 1f - x - x;
		return 1f - xx * xx;
	}


	public static void SnapRotation(this Transform t, float snapDegrees = 90f) {
		var degrees = t.localEulerAngles.z;
		var degreesFixed = Mathf.Round(degrees / snapDegrees) * snapDegrees;
		t.localEulerAngles = new Vector3(0f, 0f, degreesFixed);

	}

	public static Vector2 XY(this Transform t) {
		return t.position;
	}

}
