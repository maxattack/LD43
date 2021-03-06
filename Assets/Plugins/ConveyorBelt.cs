﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{

    public float Strength = 3f;
    private Material myMat;
    public bool Active = true;

    void Start()
    {
        myMat = GetComponent<MeshRenderer>().materials[0];
        StartCoroutine("UpdateMaterialOffset");
    }

    IEnumerator UpdateMaterialOffset()
    {
        while (Active)
        {
            myMat.SetTextureOffset("_MainTex", new Vector2(0, myMat.GetTextureOffset("_MainTex").y + 0.1f));
            yield return new WaitForSeconds(0.05f);
        }
    }

    public void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.GetComponent<Rigidbody2D>() != null && Active)// && GetComponent<Collider2D>().bounds.Contains(collider.gameObject.transform.position))
        {
            collider.GetComponent<Rigidbody2D>().AddForce(transform.up * Strength * collider.GetComponent<Rigidbody2D>().drag, ForceMode2D.Force);

        }
    }
}
