using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class CamRotation : MonoBehaviour
{
    public Vector2 MousePos;
    public Vector3 DesiredRot;
    [SerializeField]private Vector3 FullDesiredRot;
    public Vector3 DesiredRotVelocity;
    [SerializeField]Vector3 PlayerRotation;
    public float LookSpeed;
    [SerializeField] private float xVel;
    [SerializeField] private float yVel;
    [SerializeField] public Transform Head;
    private Transform target;
    [SerializeField] private GameObject CamMask;
    public float speed;
    private GameObject Player;
    private void Awake()
    {
        if (!transform.parent.GetComponent<PhotonView>().IsMine)
        {
            Destroy(transform.parent.GetChild(2).GetChild(2).gameObject.GetComponent<Camera>());
            Destroy(gameObject);
        }

    }
    void Start()
    {
        CamShake.CamAnim = transform.GetChild(0).GetComponent<Animator>();
        Player = transform.parent.GetChild(0).gameObject;
        target = Head;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        DesiredRot = new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"),0);

        Vector3 dirplusrot = transform.localEulerAngles+DesiredRot;
        //Debug.Log(dirplusrot.x);


        if (dirplusrot.x > 80&&dirplusrot.x < 180)
        {
            transform.localEulerAngles = new Vector3(80, dirplusrot.y, 0);
        }
        else if (dirplusrot.x < 280 && dirplusrot.x > 180)
        {
            transform.localEulerAngles = new Vector3(280, dirplusrot.y, 0);
        }
        else
        {
            transform.localEulerAngles = dirplusrot;
        }
        //transform.eulerAngles = Vector3.MoveTowards(transform.eulerAngles, dirplusrot, 0.5f);

        transform.position = Vector3.MoveTowards(transform.position, target.position, (transform.position - target.position).sqrMagnitude * speed * Time.deltaTime);
        CamMask.transform.rotation = transform.rotation;

    }
    
    private void spares()
    {
        //FullDesiredRot += DesiredRot*Time.deltaTime*LookSpeed;
        //transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, DesiredRot,0.2f);
        //Debug.Log(transform.localEulerAngles + DesiredRot);
        /*
        if (dirplusrot.x < -80)
        {
            transform.localEulerAngles = new Vector3(-79, dirplusrot.y, 0);
            Debug.Log("1");
        }
        else if (xVel ==1000009)
        {
            Debug.Log(dirplusrot.x);
            transform.localEulerAngles = new Vector3(79, dirplusrot.y, 0);
            Debug.Log("2");
        }
        else
        {
            transform.localEulerAngles = dirplusrot;
            Debug.Log("3");
        }
*/
        /*        else
                        {
                            transform.eulerAngles = new Vector3(Mathf.Clamp(transform.eulerAngles.x, -80, 80), transform.eulerAngles.y, transform.eulerAngles.z);
                        }
                */
        //transform.eulerAngles = Vector3.SmoothDamp(transform.eulerAngles,DesiredRot,ref DesiredRotVelocity,LookSpeed);

        //transform.position = Vector3.Lerp(transform.position, Head.position, 0.9f);

        //PlayerRotation = new Vector3(Mathf.SmoothDampAngle(transform.eulerAngles.x,FullDesiredRot.x,ref xVel,0.1f), Mathf.SmoothDampAngle(transform.eulerAngles.y, FullDesiredRot.y, ref yVel, 0.1f), 0);
        //transform.eulerAngles = PlayerRotation;

    }
/*
    public IEnumerator Die()
    {
        
        CamMask.GetComponent<Camera>().enabled = false;
        target = DeadHead.transform;
        //DeadHead.transform.parent = null;
        DeadHead.GetComponent<Collider>().enabled = true;
        DeadHead.GetComponent<Rigidbody>().useGravity = true;
        DeadHead.transform.position = Head.position;
        DeadHead.GetComponent<Rigidbody>().position = Head.position;
        DeadHead.transform.rotation = Head.rotation;
        DeadHead.GetComponent<Rigidbody>().AddForce((new Vector3(Random.Range(-0.1f,0.1f), Random.Range(-1.1f, 1.1f), Random.Range(-0.1f, 0.1f))+Player.GetComponent<Rigidbody>().velocity)*100,ForceMode.Impulse);
        yield return new WaitForSeconds(5);

        CamMask.GetComponent<Camera>().enabled = true;
        DeadHead.GetComponent<Collider>().enabled = false;
        DeadHead.GetComponent<Rigidbody>().useGravity = false;
        DeadHead.transform.parent = Head;
        DeadHead.transform.localPosition = Vector3.zero;
        DeadHead.transform.localRotation = Quaternion.identity;
        target = Head;
    }
*/}
