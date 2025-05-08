using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MolotovFire : MonoBehaviour
{
    private float scale;
    private void Awake()
    {
        scale = 1;
    }
    private void Update()
    {
        scale-=Time.deltaTime*0.15f;
        transform.localScale = new Vector3(scale, scale, scale);
        if(scale < 0)
        {
            Destroy(transform.parent.gameObject);
        }
    }

}
