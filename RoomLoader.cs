using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public static class RoomLoader
{
    public static string Name = "Player:"+(Random.Range(1,100000).ToString());
    public static int Role = Random.Range(0,3);
    private static TMP_InputField inputField;


    public static void ChangedName()
    {
        if(inputField == null)
        {
            inputField = GameObject.Find("PlayerName").GetComponent<TMP_InputField>();
        }
        Name = inputField.text;
        Debug.Log(Name);
    }


}
