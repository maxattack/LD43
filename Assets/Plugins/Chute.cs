using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chute : MonoBehaviour, Slot.Listener
{
    void Slot.Listener.SlotFilled(Slot slot, CrewMember.Pickupable pickup)
    {
        Destroy(pickup.RootTransform.gameObject);
    }

    void Slot.Listener.SlotEmptied(Slot slot)
    {
        // slot cannot be emptied
    }

}
