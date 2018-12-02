using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelBase : MonoBehaviour {

    public Machine[] PoweringMachines;

    public void StartFueling()
    {
        foreach (Machine m in PoweringMachines)
        {
            m.Activate();
        }
    }

    public void StopFueling()
    {
        foreach (Machine m in PoweringMachines)
        {
            m.Deactivate();
        }
    }
}
