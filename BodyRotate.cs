using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyRotate : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private CamRotation camRotation;
    private float RotSpeed;
    [SerializeField] private float IdleRotSpeed;
    [SerializeField] private float MoveRotSpeed;
    void Start()
    {
        if (!transform.parent.GetComponent<PhotonView>().IsMine)
        {
            //Destroy(transform.GetChild(2).GetChild(2).gameObject);
            Destroy(gameObject);
        }

    }

    void Update()
    {
        RotSpeed = playerMovement.Moving ? MoveRotSpeed:IdleRotSpeed;

        transform.eulerAngles = new Vector3(0, Mathf.LerpAngle(transform.eulerAngles.y, camRotation.transform.eulerAngles.y, RotSpeed), 0);



        /*        if (playerMovement.Moving)
                {
                    transform.eulerAngles = new Vector3(0, camRotation.transform.eulerAngles.y, 0);
                    //LastTurnPoint = transform.eulerAngles.y;

                }
                else
                {
                    *//*           
                                if ((180 - Mathf.Abs(Mathf.Abs(LastTurnPoint - camRotation.transform.eulerAngles.y) - 180))>MaxTurn)
                                {
                                    UpdateRot();
                                }
                    *//*
                }
            }
            private void UpdateRot()
            {
                DesiredYRot = camRotation.transform.eulerAngles.y;
                //transform.eulerAngles = new Vector3 (0, camRotation.transform.eulerAngles.y, 0);
                //LastTurnPoint = transform.eulerAngles.y;
            }
                */
    }
    private void LateUpdate()
    {
        transform.position = camRotation.transform.position;
    }
}
