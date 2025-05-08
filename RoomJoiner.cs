using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomJoiner : MonoBehaviour
{
    private void Awake()
    {
        //Screen.SetResolution(640, 480, true);
    }

    public void QuickPlay()
    {
        StartCoroutine("LoadSceneTransition");
    }

    public IEnumerator LoadSceneTransition()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("SampleScene");
    }


    public void EditName()
    {
        RoomLoader.ChangedName();
    }

}
