using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelection : MonoBehaviour
{
    private Vector3 OriginalSize;
    private Vector3 TargetSize;

    private Vector3 OriginalPos;
    private Vector3 TargetPos;
    private float posOffset;


    public bool Selecting;
    public int RoleNum;

    private void Awake()
    {
        OriginalSize = transform.localScale;
        TargetSize = transform.localScale;


        OriginalPos = transform.localPosition;
        TargetPos = transform.localPosition;
    }
    private void OnMouseEnter()
    {
        TargetSize = OriginalSize;
        posOffset = 10;
    }
    private void OnMouseExit()
    {
        TargetSize = OriginalSize;
        posOffset = 0;
    }

    private void OnMouseDown()
    {
        TargetSize = OriginalSize * 0.8f;
    }

    private void OnMouseUp()
    {
        TargetSize = OriginalSize;
        for (int i = 0; i < GameObject.FindObjectsOfType<CharacterSelection>().Length; i++)
        {
            if (GameObject.FindObjectsOfType<CharacterSelection>()[i]!= this)
                GameObject.FindObjectsOfType<CharacterSelection>()[i].Selecting = false;



        }
        Selecting = !Selecting;
        if (Selecting)
        {
            RoomLoader.Role = RoleNum;
        }
        else
        {
            RoomLoader.Role = Random.Range(0,2);
        }
    }


    private void Update()
    {
        if (Selecting)
        {
            TargetPos = OriginalPos + new Vector3(0, 60+posOffset, 0);
            
        }
        else
        {
            TargetPos = OriginalPos+new Vector3(0,posOffset,0);
        }




        transform.localScale = Vector3.Lerp(transform.localScale, TargetSize, 0.1f);
        transform.localPosition = Vector3.Lerp(transform.localPosition, TargetPos, 0.1f);

    }
}
