using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MolotovProjectile : MonoBehaviour
{
    public GameObject Head;
    private Rigidbody rb;
    [SerializeField] private float throwVelocity;
    [SerializeField] private GameObject MolotovFire;

    public void Throw()
    {
        transform.parent = null;
        rb = GetComponent<Rigidbody>();
        transform.position = Head.transform.position;
        rb.transform.position = Head.transform.position;
        rb.velocity = Vector3.zero;
        transform.rotation = Quaternion.identity;
        rb.AddForce(Head.transform.forward*throwVelocity,ForceMode.Impulse);
    }
    private void OnCollisionEnter(Collision collision)
    {
        PhotonNetwork.Instantiate("MolotovFire", transform.position, Quaternion.identity);
        gameObject.SetActive(false);

    }


}
