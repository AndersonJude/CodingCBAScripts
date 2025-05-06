using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPCevents : MonoBehaviour
{

    [SerializeField] private HealthManager health;



    [PunRPC]
    public void RPC_HitPlayer(int dmg)
    {
        Debug.Log("CALLED");
        //weaponManager.HitPlayer(view,dmg);
        health.LoseHealth(dmg);
/*        if (PhotonNetwork.LocalPlayer.ActorNumber == view)
        {
            healthManager.LoseHealth(50);
        }
*/    }

}
