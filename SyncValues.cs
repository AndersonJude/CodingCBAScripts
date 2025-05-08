using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;

public class SyncValues : MonoBehaviour, IPunObservable
{

    public TextMeshProUGUI nameField;
    public GameObject[] roles;
    public GameObject[] slideRoles;
    [Header("PlayerInfo")]
    public string playerName;
    public int playerRole;
    public int playerWeapon;
    public bool playerWalking;
    public int playerKills;


    private Animator LeftLeg;
    [SerializeField] private Animator[] characterAnims;

    private void Start()
    {
        playerWalking = false;
        if (GetComponent<PhotonView>().IsMine)
        {
            playerName = RoomLoader.Name;
            nameField.text = playerName;
            nameField.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = playerName;

            playerRole = RoomLoader.Role;
            //LeftLeg = roles[playerRole].transform.GetChild(0).GetChild(1).GetComponent<Animator>();

            for (int i = 0; i < slideRoles.Length; i++)
            {
                slideRoles[i].SetActive(false);
            }
            slideRoles[playerRole].SetActive(true);
            if(playerRole == 0)
            {
                slideRoles[3].SetActive(true);
                slideRoles[4].SetActive(true);
                slideRoles[5].SetActive(true);

            }

        }
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(playerName);
            stream.SendNext(playerRole);
            stream.SendNext(playerWeapon);
            //stream.SendNext(playerWalking);
            stream.SendNext(playerKills);
        }
        if (stream.IsReading)
        {
            playerName = (string)stream.ReceiveNext();
            nameField.text = playerName;
            nameField.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = playerName;
            playerRole = (int)stream.ReceiveNext();

            for (int i = 0; i < roles.Length; i++)
            {
                roles[i].SetActive(false);
            }
            roles[playerRole].SetActive(true);
            
            playerWeapon = (int)stream.ReceiveNext();


            //playerWalking = (bool)stream.ReceiveNext();
            /*            if (LeftLeg == null)
                        {
                            LeftLeg = roles[playerRole].transform.GetChild(0).GetChild(1).GetComponent<Animator>();
                        }
                        Debug.Log(playerWalking);
                        if (playerWalking)
                        {
                            LeftLeg.SetBool("Walking", true);
                        }
            */            //WHY IS IT NOT WORKING------------------------------------------------------------------------------------------||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||




            characterAnims[playerRole].SetInteger("num", playerWeapon);

            playerKills = (int)stream.ReceiveNext();

        }
    }

    

    




}
