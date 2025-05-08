using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPCevents : MonoBehaviour
{

    [SerializeField] private HealthManager health;



    [PunRPC]
    public void RPC_HitPlayer(int dmg,int id)
    {
        //Debug.Log(id);
        //weaponManager.HitPlayer(view,dmg);
        health.LoseHealth(dmg,id);
/*        if (PhotonNetwork.LocalPlayer.ActorNumber == view)
        {
            healthManager.LoseHealth(50);
        }
*/    }




    [PunRPC]
    public void RPC_GiveKill()
    {
        if(health != null)
        {
            health.Kills += 1;
            //Debug.LogError("AAHHHHHH");
            Debug.Log(health.Kills);
            health.UpdateKills();
        }
    }

}
