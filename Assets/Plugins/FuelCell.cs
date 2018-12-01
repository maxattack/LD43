using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelCell : MonoBehaviour {

    public float FuelLevel = 5f;

	public void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<FuelBase>() != null)
        {
            Transform restTransform = other.GetComponent<FuelBase>().StartFueling();
            transform.parent.transform.position = restTransform.position;
            transform.parent.transform.rotation = restTransform.rotation;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<FuelBase>() != null)
        {
            other.GetComponent<FuelBase>().StopFueling();
        }
    }

}
