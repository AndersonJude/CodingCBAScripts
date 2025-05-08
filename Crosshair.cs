using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{

    public int CrosshairType;
    public float SecondaryMultiplier;
    [SerializeField] GameObject[] BasicCrosshairs;
    [SerializeField] Rigidbody rb;
    [SerializeField] Animator hitAnim;
    [SerializeField] PhotonView PhotonView;
    public float Velocity;
    [SerializeField] private float BasicMultiplier;
    [SerializeField] private float BasicDisplacement;
    public float UniqueDisplacement;
    public bool MachineGun;
    
    
    private void Start()
    {
        if (PhotonView.IsMine)
        {
            ChangeCrosshair(0);
        }
    }
    void Update()
    {
        if (MachineGun)
            Velocity = Mathf.Lerp(Velocity, 0, 0.2f);
        else
            Velocity = Mathf.Lerp(Velocity, rb.velocity.sqrMagnitude, 0.2f);
        if (CrosshairType == 0)
        {
        }
        else if (CrosshairType == 1)
        {



        }
        else if (CrosshairType == 2)
        {
            //Velocity = rb.velocity.sqrMagnitude;
            for (int i = 0; i < BasicCrosshairs.Length; i++)
            {
                BasicCrosshairs[i].transform.localPosition = new Vector2(0, (Velocity * BasicMultiplier * SecondaryMultiplier) + (BasicDisplacement+UniqueDisplacement*30000));
            }



        }

    }



    public void ChangeCrosshair(int crosshair)
    {
        CrosshairType = crosshair;

        if (CrosshairType != 0)
        {
            transform.GetChild(0).GetComponent<Image>().enabled = true;
        }

        if (CrosshairType == 0)
        {
            for (int i = 0; i < BasicCrosshairs.Length; i++)
            {
                BasicCrosshairs[i].GetComponent<Image>().enabled = false;
            }
            transform.GetChild(0).GetComponent<Image>().enabled = false;

        }
        else if (CrosshairType == 1)
        {
            /*            for (int i = 0; i < BasicCrosshairs.Length; i++)
                        {
                            BasicCrosshairs[i].GetComponent<Image>().enabled = true;
                        }
            */
            for (int i = 0; i < BasicCrosshairs.Length; i++)
            {
                BasicCrosshairs[i].GetComponent<Image>().enabled = false;
            }

        }
        else if (CrosshairType == 2)
        {
            for (int i = 0; i < BasicCrosshairs.Length; i++)
            {
                BasicCrosshairs[i].GetComponent<Image>().enabled = true;
            }
        }


    }

    public void Hit()
    {
        hitAnim.SetTrigger("Hit");
    }

}
