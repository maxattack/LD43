using UnityEngine;

[RequireComponent(typeof(Prop))]
public class FuelCell : MonoBehaviour {

    public float maxFuelLevel = 5f; // measured in seconds
    public Transform fuelBar;

	internal float fuelLevel;

	public bool HasFuel { get { return fuelLevel > 0f; } }
	public float FuelRatio { get { return Mathf.Clamp01(fuelLevel / maxFuelLevel); } }

    void Awake() {
        fuelLevel = maxFuelLevel;
    }

	public void DrainFuel(float amount) {
		if (fuelLevel > 0f) {
			fuelLevel -= amount;
			fuelBar.transform.localScale = new Vector3(transform.localScale.x, FuelRatio, 1f);
			if (fuelLevel <= 0f) {
				GetComponent<Prop>().description += "(Spent)";
			}
		}
	}

}
