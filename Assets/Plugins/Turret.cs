using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour, Slot.Listener {

    public GameObject gun;

    internal FuelCell cell;

    public bool IsShooting
    {
        get { return gun.activeInHierarchy; }
    }

    void Start()
    {
        if (EnemyShip.inst)
            EnemyShip.inst.turrets.Add(this);
    }

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

    void Slot.Listener.SlotEmptied(Slot slot, Transform item)
    {
        cell = null;
    }

    void Update()
    {
        var thrust = cell && cell.HasFuel;
        if (thrust)
        {
            if (!gun.activeInHierarchy) {
                gun.SetActive(true);
				var sfx = GetComponent<AudioSource>();
				if (sfx && Time.time > 0.2f)
					sfx.Play();
			}

            cell.DrainFuel(Time.deltaTime);

        }
        else
        {
            if (gun.activeInHierarchy)
                gun.SetActive(false);
        }

    }
}
