using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chute : MonoBehaviour, Slot.Listener
{
    void Slot.Listener.SlotFilled(Slot slot, Transform item)
    {
        Destroy(item.gameObject);
    }

    void Slot.Listener.SlotEmptied(Slot slot)
    {
        // slot cannot be emptied
    }

}
