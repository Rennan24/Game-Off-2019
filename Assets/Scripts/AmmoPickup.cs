using System;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    public PlayerWeaponType Type;
    public SimpleAudioEvent PickupSound;
    public int Amount;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Is not a player return
        if (!other.CompareTag("Player"))
            return;

        GameManager.Player.AmmoPickedUp(this);
        SFXManager.Inst.PlayAtPoint(transform.position, PickupSound);
        Destroy(gameObject);
    }
}
