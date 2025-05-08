using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
public class PlayerMovement : MonoBehaviour
{
    private bool Dead;
    private Rigidbody rb;
    public float Speed;
    public Vector3 DesiredMoveDir;
    public Vector3 FINALMoveDir;
    [SerializeField]private GameObject Cam;
    [SerializeField] private GameObject CamMask;
    public GameObject Feet;
    public bool Moving;
    public float SpeedMultiplier;
    public float MaxVelocity;
    [SerializeField] private Animator ArmsAnim;
    [SerializeField] private Transform[] SpawnPoints;
    private float TargetMaxVel;
    [SerializeField] private GameObject playernametextthing;

    [Header("Jumping")]
    private bool Jumping;
    public bool CanGround;
    public bool IsGrounded;
    public float JumpForce;
    public float JumpForceMultiplier;
    public LayerMask GroundLayerMask;
    private float TimeOnJump;
    [SerializeField] private float JumpTime;

    [Header("Sliding")]
    public bool sliding;
    [SerializeField] private ParticleSystem skidPars;
    private bool startSliding;
    [SerializeField] private float slideSpeed;
    [SerializeField] private Animator headSlide;
    [SerializeField] private Animator[] bodySlide;
    [SerializeField] private MeshCollider normCol;
    [SerializeField] private CapsuleCollider slideCol;
    public bool Aiming;

    private bool promisedSlide;

    [Header("Camera")]
    private float desiredFOV;


    [Header("Slopes")]
    public bool onSlope;
    public int maxSlope;


    private SyncValues syncValues;

/*    [Header("PlayerInfo")]
*//*    public int playerRole;
*//*    public int playerWeapon;
*/


    void Awake()
    {
        Application.targetFrameRate = 60;
        desiredFOV = 70;
        rb = GetComponent<Rigidbody>();
        //Cam = Camera.main.gameObject;
        CanGround = true;
        

        if (!transform.parent.GetComponent<PhotonView>().IsMine)
        {
            //Destroy(transform.parent.GetComponent<RPCevents>());
            Destroy(this);
            Destroy(transform.GetChild(6).GetComponent<HealthManager>());
        }
        else
        {
            Destroy(transform.GetChild(0).gameObject);
            Destroy(transform.GetChild(6).GetComponent<Collider>());
        }
        syncValues = transform.parent.GetComponent<SyncValues>();

        for (int i = 0; i < SpawnPoints.Length; i++)
        {
            SpawnPoints[i] = GameObject.Find("SpawnPoints").transform.GetChild(i);
        }
        rb.gameObject.transform.position = SpawnPoints[Random.Range(0, SpawnPoints.Length)].position;

        playernametextthing.SetActive(false);

    }
    private void Start()
    {
        //Screen.SetResolution(640, 480, true);
        
        //Debug.Log(Screen.currentResolution);
    }
    void Update()
    {
        if (!Dead)
        {
            DesiredMoveDir = (transform.right * Input.GetAxisRaw("Horizontal")) + (transform.forward * Input.GetAxisRaw("Vertical")).normalized;


            //Check if player is moving
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                Moving = true;
                for (int i = 0; i < bodySlide.Length; i++)
                {
                    bodySlide[i].SetBool("Walking", true);
                    syncValues.playerWalking = true;
                }
            }
            else
            {
                Moving = false;
                for (int i = 0; i < bodySlide.Length; i++)
                {
                    bodySlide[i].SetBool("Walking", false);
                    syncValues.playerWalking = false;
                }
            }

            if (Moving && IsGrounded && !sliding)
            {
                ArmsAnim.SetBool("Walking", true);
                syncValues.playerWalking = true;
            }
            else
            {
                ArmsAnim.SetBool("Walking", false);
                syncValues.playerWalking = false;
            }

        }


        if (!Aiming)
        {
            ArmsAnim.SetBool("Aiming", false);
        }
        else
        {
            ArmsAnim.SetBool("Aiming", true);
        }


        //Checking if player is on the ground
        RaycastHit hit;
        if (Physics.Raycast(Feet.transform.position, Vector3.down, out hit, 0.4f, GroundLayerMask))
        {
            if (CanGround)
            {
                //OnTouchGround
                IsGrounded = true;
                Speed = 160;
                CancelJump();
                ArmsAnim.SetBool("Jumping", false);
                if (promisedSlide && !Dead&&!Aiming)
                {
                    StartSlide();
                }

                //Sprinting
                if (!sliding)
                {
                    //Running Values
                    SpeedMultiplier = 1.8f;
                    TargetMaxVel = MaxVelocity * 1.5f;
                    if (!Aiming)
                    {
                        desiredFOV = 70;
                    }
                    else
                    {
                        desiredFOV = 45;
                    }



                }
            }




            //Slope Management:
            if(Vector3.Angle(Vector3.up, hit.normal)!=0&& Vector3.Angle(Vector3.up, hit.normal) < maxSlope)
            {
                onSlope = true;
                rb.useGravity = false;
            }
            else
            {
                onSlope = false;
                rb.useGravity = true;
            }

            FINALMoveDir = Vector3.ProjectOnPlane(DesiredMoveDir, hit.normal);
        }
        else
        {
            rb.useGravity = true;
            FINALMoveDir = DesiredMoveDir;
            IsGrounded = false;
            TargetMaxVel = MaxVelocity * 4f;
            ArmsAnim.SetBool("Jumping", true);
            //If in air give player less control (less speed)
            Speed = 80;

        }


        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (IsGrounded && !Dead)
                StartCoroutine("Jump");
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            CancelJump();
        }


        //Sliding
        if (Input.GetKeyDown(KeyCode.LeftShift)&&Moving)
        {
            if (IsGrounded)
            {
                if(!Aiming)
                    StartSlide();
            }
            else
            {
                if(!Aiming)
                    promisedSlide = true;
            }
        }
        else if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            CancelSlide();
        }


        if (Jumping && (Time.time - TimeOnJump) < JumpTime)
        {
            //JUMPS
            rb.AddForce(new Vector3(DesiredMoveDir.x, JumpForceMultiplier, DesiredMoveDir.z) * Mathf.Abs((Time.time - TimeOnJump) - JumpTime) * JumpForce*Time.deltaTime, ForceMode.Force);
            //Debug.Log(Mathf.Abs((Time.time - TimeOnJump) - JumpTime));
        }
        
        transform.eulerAngles = new Vector3(0, Cam.transform.eulerAngles.y, 0);


    }
    private void FixedUpdate()
    {
        if (!Dead)
        {
            rb.maxLinearVelocity = Mathf.Lerp(rb.maxLinearVelocity, TargetMaxVel, 0.6f);

            //Moves player
            if (!sliding)
            {
                //REGULAR MOVEMENT
                rb.AddForce(0.016f * Speed * SpeedMultiplier * FINALMoveDir, ForceMode.VelocityChange);
                rb.drag = 4;
            }
            else if (sliding && !startSliding &&!Aiming)
            {
                startSliding = true;
                rb.drag = 1f;
                StartSlide();
            }
            else
            {
                //Gives less control while sliding
                rb.AddForce(0.016f * Speed * SpeedMultiplier * 0.05f * FINALMoveDir, ForceMode.VelocityChange);
            }








        }
        Cam.GetComponent<Camera>().fieldOfView = Mathf.Lerp(Cam.GetComponent<Camera>().fieldOfView, desiredFOV, 0.3f);
        CamMask.GetComponent<Camera>().fieldOfView = Cam.GetComponent<Camera>().fieldOfView;
    }
    public IEnumerator Jump()
    {
/*        if (sliding)
        {
            PrivateJumpForce = JumpForce*2;
        }
        else
        {
            PrivateJumpForce = JumpForce;
        }
*/      Jumping = true;
        IsGrounded = false;
        CanGround = false;
        TargetMaxVel = MaxVelocity * 40f;
        CancelSlide();
        TimeOnJump = Time.time;
        yield return new WaitForSeconds(0.04f);
        CanGround = true;
    }
    private void CancelJump()
    {
        Jumping = false;
    }
    private void StartSlide()
    {
        //Sliding Values
        sliding = true;
        SpeedMultiplier = 2.3f;
        TargetMaxVel = MaxVelocity * 2f;
        skidPars.Play();
        desiredFOV = 90;
        headSlide.SetBool("Sliding", sliding);

        for (int i = 0; i < bodySlide.Length; i++)
        {
            bodySlide[i].SetBool("Sliding", sliding);
        }
        promisedSlide = false;
/*        normCol.enabled = false;
        slideCol.enabled = true;
*/


        rb.AddForce(FINALMoveDir * Speed * SpeedMultiplier * slideSpeed, ForceMode.VelocityChange);
        TargetMaxVel = MaxVelocity * 100f;
        //Debug.Log("MOVE");
    }
    private void CancelSlide()
    {
        sliding = false;
        skidPars.Stop();
        startSliding = false;
        headSlide.SetBool("Sliding", sliding);
        for (int i = 0; i < bodySlide.Length; i++)
        {
            bodySlide[i].SetBool("Sliding", sliding);
        }
        //desiredFOV = 70;
        /*        normCol.enabled = true;
                slideCol.enabled = false;
        */

    }



    public IEnumerator DEAD()
    {
        Dead = true;
        rb.velocity = Vector3.zero;
        //Cam.transform.localPosition -= new Vector3 (0, 5, 5);
        //Cam.transform.parent.GetComponent<CamRotation>().StartCoroutine("Die");
        rb.gameObject.transform.position = SpawnPoints[Random.Range(0,SpawnPoints.Length)].position;
        yield return new WaitForSeconds(2);

        Dead = false;
    }




}
