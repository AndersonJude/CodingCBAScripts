using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CamShake
{
    public static Animator CamAnim;


    public static void Recoil(int i)
    {
        if (i == 1)
        {
            CamAnim.SetTrigger("Recoil1");
        }
        else if (i == 2)
        {
            CamAnim.SetTrigger("Recoil2");
        }
    }


}
