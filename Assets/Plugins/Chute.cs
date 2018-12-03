using UnityEngine;

public class Chute : MonoBehaviour, Slot.Listener
{
    void Slot.Listener.SlotFilled(Slot slot, Transform item)
    {
        Destroy(item.gameObject);
		var sfx = GetComponent<AudioSource>();
		if (sfx)
			sfx.Play();
    }

    void Slot.Listener.SlotEmptied(Slot slot, Transform item)
    {
    }

}
