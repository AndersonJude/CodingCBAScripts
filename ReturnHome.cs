using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ReturnHome : MonoBehaviour
{

    private float timeHeld;
    public float ogpos;
    public float newpos;
    public GameObject ui;
    public Slider Slider;
    private void Update()
    {
        if (Input.GetKey(KeyCode.Backspace))
        {
            //Debug.Log("YEAH");
            timeHeld += Time.deltaTime;
        }
        else if (timeHeld > 0)
        {
            timeHeld -= Time.deltaTime;
        }


        if (timeHeld > 0)
        {
            ui.transform.localPosition = new Vector3(Mathf.Lerp(ui.transform.localPosition.x, newpos, 0.1f), 430, 0);
        }
        else
        {
            ui.transform.localPosition = new Vector3(Mathf.Lerp(ui.transform.localPosition.x, ogpos, 0.1f), 430, 0);
        }

        Slider.value = timeHeld;

        if(timeHeld > 2)
        {
            //PhotonNetwork.LeaveRoom();
            PhotonNetwork.Disconnect();
        }

    }


}
