using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class FPS : MonoBehaviour
{
    private void Update()
    {
        GetComponent<TextMeshProUGUI>().text = (1 / Time.deltaTime).ToString();
    }
}
