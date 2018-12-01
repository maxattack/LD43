using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelBase : MonoBehaviour {

    public Machine[] PoweringMachines;
    public Transform CellPosition;

    void Start()
    {
        CellPosition.position = CellPosition.TransformPoint(CellPosition.position);
    }

    public Transform StartFueling()
    {
        foreach (Machine m in PoweringMachines)
        {
            m.Activate();
        }

        return CellPosition;
    }

    public void StopFueling()
    {
        foreach (Machine m in PoweringMachines)
        {
            m.Deactivate();
        }
    }
}
