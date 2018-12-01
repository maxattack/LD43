using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour {

	void Start () {
		
	}

    private void OnTriggerStay2D(Collider2D other)
    {
        //Debug.Log("hey");
        if (other.gameObject.tag == "Tile")
        {
            //Debug.Log("nice");
            other.gameObject.GetComponentInChildren<Rigidbody>().AddForce(Vector3.forward);
            //Destroy(other.gameObject);
            //reduce weight by tile weight
            Destroy(gameObject);
        }
    }

}
