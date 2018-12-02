using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour, Slot.Listener {

    public GameObject gun;

    internal FuelCell cell;

    void Awake()
    {
        gun.SetActive(false);
    }

    void Slot.Listener.SlotFilled(Slot slot, Transform item)
    {
        var fuelCell = item.GetComponent<FuelCell>();
        if (fuelCell)
        {
            cell = fuelCell;
        }
    }

    void Slot.Listener.SlotEmptied(Slot slot)
    {
        cell = null;
    }

    void Update()
    {
        var thrust = cell && cell.HasFuel;
        if (thrust)
        {
            if (!gun.active)
                gun.SetActive(true);

            cell.DrainFuel(Time.deltaTime);

        }
        else
        {
            if (gun.active)
                gun.SetActive(false);
        }

    }
}
