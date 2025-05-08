using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MolotovEvent : MonoBehaviour
{


    public GameObject Molotov;


    public void ThrowMolotov()
    {
        Molotov.SetActive(true);
        Molotov.GetComponent<MolotovProjectile>().Throw();
    }
}
