using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBox : MonoBehaviour
{
    [SerializeField] private int Dmg;
    [SerializeField] private WeaponManager WeaponManager;


    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.gameObject.layer == 7)
        {
            Debug.Log("HIIIT PALYERA");
            other.transform.parent.parent.GetComponent<PhotonView>().RPC("RPC_HitPlayer", RpcTarget.All, Dmg,PhotonNetwork.LocalPlayer.ActorNumber);
            WeaponManager.ShowDmg(Dmg);
        }
    }


}
