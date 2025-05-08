using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncWalking : MonoBehaviour
{
    [SerializeField] private Animator[] Animators;
    private bool walking;
    private bool checkWalking;
    private Vector3 lastPos;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private GameObject player;



    private void Start()
    {
        InvokeRepeating("updatewalk", 0.1f, 1);
    }
    private void Update()
    {


    }

    private void updatewalk()
    {
        if (walking && !checkWalking)
        {
            checkWalking = true;
            for (int i = 0; i < Animators.Length; i++)
            {
                Animators[i].SetBool("Walking", true);
            }
        }
        else if (!walking && checkWalking)
        {
            checkWalking = false;
            for (int i = 0; i < Animators.Length; i++)
            {
                Animators[i].SetBool("Walking", false);
            }

        }

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1, layerMask))
        {
            if (player.transform.position != lastPos)
            {
                walking = true;
            }
            else
            {
                walking = false;
            }
        }
        else { walking = false; }
        lastPos = player.transform.position;

    }


}
