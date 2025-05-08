using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderboardManager : MonoBehaviour
{

    public PhotonView view;
    private SyncValues[] SyncValues;
    private int[] playerActorNumbers;
    private int[] Kills;
    private string[] Names;

    private int First;
    private int Second;
    private int Third;


    private string FirstName;
    private string SecondName;
    private string ThirdName;

    private int FirstKills;
    private int SecondKills;
    private int ThirdKills;


    public TextMeshProUGUI first;
    public TextMeshProUGUI second;
    public TextMeshProUGUI third;
    private void Awake()
    {
        if (!view.IsMine)
        {
            Destroy(this);
        }
    }
    void Start()
    {
        InvokeRepeating("UpdateBoard", 2,2);
    }



    private void UpdateBoard()
    {
        SyncValues = GameObject.FindObjectsOfType<SyncValues>();
        

        //Logic Hell: 
        //I wrote this code very late at night and was tired :(
        for (int i = 0; i < SyncValues.Length; i++)
        {
            if (SyncValues[i].playerKills > FirstKills)
            {
                //First = playerActorNumbers[i];
                FirstKills = SyncValues[i].playerKills;
                FirstName = SyncValues[i].playerName;
            }
        }
        for (int i = 0; i < SyncValues.Length; i++)
        {
            if (SyncValues[i].playerKills < FirstKills && SyncValues[i].playerKills > SecondKills)
            {
                SecondKills = SyncValues[i].playerKills;
                SecondName = SyncValues[i].playerName;
            }
        }
        for (int i = 0; i < SyncValues.Length; i++)
        {
            if (SyncValues[i].playerKills < SecondKills && SyncValues[i].playerKills > ThirdKills)
            {
                ThirdKills = SyncValues[i].playerKills;
                ThirdName = SyncValues[i].playerName;
            }
        }

        if (FirstKills == 0)
        {
            first.text = "#1:";
        }
        else
            first.text = "#1:" + FirstName + " - " + FirstKills + " Kills";
        if (SecondKills == 0)
        {
            second.text = "#2:";
        }
        else
            second.text = "#2:" + SecondName + " - " + SecondKills + " Kills";
        if (ThirdKills == 0)
        {
            third.text = "#3:";
        }
        else
            third.text = "#3:" + ThirdName + " - " + ThirdKills + " Kills";


    }

}
