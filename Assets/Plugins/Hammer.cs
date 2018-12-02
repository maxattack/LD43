using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour, CrewMember.Pickupable {

    public GameObject InvisibleWall;

	void Start () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag == "Tile")
        {
            //other.gameObject.GetComponentInChildren<Rigidbody>().AddForce(Vector3.forward);
            Instantiate(InvisibleWall, other.transform.position, other.transform.rotation);

            Destroy(other.transform.parent.gameObject);
            
            //Destroy(gameObject.transform.parent.gameObject);
        }
    }

    Transform CrewMember.Pickupable.RootTransform
    {
        get { return transform; }
    }

	Vector2 CrewMember.Pickupable.BoxSize {
		get { return new Vector2(0.5f, 0.5f); }
	}

    void CrewMember.Pickupable.OnPickup(CrewMember crew)
    {
        foreach (var it in GetComponentsInChildren<Collider2D>())
            it.enabled = false;
    }

    void CrewMember.Pickupable.OnActionPerformed(CrewMember crew)
    {
        crew.TryPutDown();
    }

    void CrewMember.Pickupable.OnDropoff(CrewMember crew)
    {
        Debug.Log("firing");
        foreach (var it in GetComponentsInChildren<Collider2D>())
            it.enabled = true;

        Collider2D landingOn = Physics2D.OverlapBox(gameObject.transform.position, Vector2.one * 2, 0f, 0, 0f, 0f);
        Debug.Log(landingOn.name);
    }

}
