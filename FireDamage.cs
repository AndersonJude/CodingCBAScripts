using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDamage : MonoBehaviour
{
    [SerializeField] private bool WasOtherPlayer;
    public int OtherPlayerID;
    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<HitBox>().healthManager != null)
        {
            HealthManager healthManager = other.GetComponent<HitBox>().healthManager;
            if (!healthManager.IsOnFire)
                healthManager.STARTFIRE(WasOtherPlayer, OtherPlayerID);
        }

    }
}
