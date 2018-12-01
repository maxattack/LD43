using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machine : MonoBehaviour {

    private bool Active = false;

    public GameObject activeMesh;

    void Start() {
        Deactivate();
    }

    public void Activate()
    {
        activeMesh.SetActive(true);
    }

    public void Deactivate()
    {
        activeMesh.SetActive(false);
    }
}
