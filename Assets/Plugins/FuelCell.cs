using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelCell : MonoBehaviour {

    public float MaxFuelLevel = 5f;
    private float FuelLevel = 5f;
    public GameObject FuelBar;
    bool spendingFuel = false;
    public float spendRate = 0.1f;
    private FuelBase currentBase;

    void Start()
    {
        FuelLevel = MaxFuelLevel;
    }

	public void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<FuelBase>() != null)
        {
            currentBase = other.GetComponent<FuelBase>();
            Transform restTransform = other.GetComponent<FuelBase>().StartFueling();
            transform.parent.transform.position = restTransform.position;
            transform.parent.transform.rotation = restTransform.rotation;

            spendingFuel = true;
            StartCoroutine("LoseFuel");
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (currentBase != null && other.GetComponent<FuelBase>() == currentBase)
        {
            currentBase.StopFueling();
            currentBase = null;

            spendingFuel = false;
            StopAllCoroutines();
        }
    }

    IEnumerator LoseFuel()
    {
        while(spendingFuel)
        {
            if(FuelLevel <= 0)
            {
                spendingFuel = false;
                currentBase.StopFueling();
            }

            FuelLevel -= spendRate;

            FuelBar.transform.localScale = new Vector3(transform.localScale.x, FuelLevel/ MaxFuelLevel, 1);

            yield return new WaitForSeconds(0.5f);
        }
        
    }

}
