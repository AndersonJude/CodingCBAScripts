using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour
{
    public int Role;
    [SerializeField] private int CurrentWeapon;
    [SerializeField] private int[] WeaponCrosshairTypes;
    [SerializeField] private Animator animator;
    private GameObject CurrentWeaponobj;
    [SerializeField]private int InventorySize;

    [SerializeField] private GameObject Cam;

    [SerializeField] private Crosshair crosshair;
    [SerializeField] private Animator[] hittxtanims;
    private int hittxtnum;

    [SerializeField] private PlayerMovement playerMovement;


    public float Inaccuracy;
    public float AccuracyMultiplier;
    public float[] IndividualAccuracyMultiplier;
    public float[] IndividualAccuracyOffset;


    private bool shooting;
    [SerializeField] private LayerMask canShoot;
    [SerializeField] private GameObject[] ShootArea;
    [SerializeField] private LineRenderer[] ShootLine;
    [SerializeField] private float[] TimeBtwShots;
    [SerializeField] private int[] Recoil;
    [SerializeField] private GameObject[] muzzleFlashes;
    [SerializeField] private float[] reloadTime;
    [SerializeField] private int[] dmg;


    [Header("Revolver")]
/*    [SerializeField] private Gradient bulletGradient1;
    [SerializeField] private Gradient bulletGradient2;
    [SerializeField] private Gradient bulletGradient3;
*/    [SerializeField] private ParticleSystem[] revolverShootPars;
    [SerializeField] private int CurrentRevolverShootPars;
    private bool aiming;

    [SerializeField] private int[] Ammo;
    [SerializeField] private int[] MaxAmmo;
    [SerializeField] private SkinnedMeshRenderer[] Skins;
    [SerializeField] private TextMeshProUGUI ammotxt1;
    [SerializeField] private TextMeshProUGUI ammotxt2;

    //private int revolverAmmo = 6;
    //private int LMGAmmo = 50;
    private bool reloading;



    [Header("Networking")]
    [SerializeField] private PhotonView PhotonView;
    [SerializeField] private HealthManager healthManager;



    private bool canLeftClick = true;

    private void Awake()
    {
        if (!PhotonView.IsMine)
        {
            Destroy(this);
        }

    }

    void Start()
    {
        Role = RoomLoader.Role;

        for (int i = 0; i < Skins.Length; i++)
        {
            Skins[i].enabled = false;
        }

        revolverShootPars[0].gameObject.transform.parent.parent.parent = null;
        if (Role == 1)
        {
            CurrentWeapon = 0;
            Skins[2].enabled = true;
            Skins[3].enabled = true;

        }
        else if (Role == 0)
        {
            CurrentWeapon = 1;
            Skins[0].enabled = true;
            Skins[1].enabled = true;

        }
        else if (Role == 2)
        {
            CurrentWeapon = 2;
            Skins[4].enabled = true;
            Skins[5].enabled = true;

        }

        StartCoroutine("UpdateWeapon");
        for (int i = 0; i < ShootArea.Length; i++)
        {
            if (ShootLine[i] != null)
            {
                ShootLine[i].transform.parent = null;
                ShootLine[i].transform.position = Vector3.zero;
                ShootLine[i].transform.rotation = Quaternion.identity;

            }

        }
    }

    void Update()
    {
        if (ShootLine[CurrentWeapon]!=null)
            ShootLine[CurrentWeapon].transform.position -= (ShootLine[CurrentWeapon].GetPosition(0) - ShootLine[CurrentWeapon].GetPosition(1)).normalized * 5f;

        if (Input.GetMouseButtonDown(0))
        {
            if(canLeftClick)
                StartCoroutine("LeftClick");
            //Sword
            if (CurrentWeapon == 0 || CurrentWeapon == 3 || CurrentWeapon == 5)
            {
                animator.SetBool("SwordWinding", true);
            }

            //Revolver
            else if (CurrentWeapon == 1 && !shooting)
            {
                if (Ammo[CurrentWeapon] > 0)
                {
                    StartCoroutine("ShootRevolver");
                }
                else
                {
                    if (!reloading)
                    {
                        StartCoroutine("ReloadRevolver");
                    }
                }
            }






        }
        else if (Input.GetMouseButtonUp(0))
        {
            //Sword
            if (CurrentWeapon == 0 || CurrentWeapon == 3 || CurrentWeapon == 5)
            {
                animator.SetBool("SwordWinding", false);
            }

        }

        //Allows for continous shooting
        if (Input.GetMouseButton(0))
        {
            animator.SetBool("Shooting", !reloading);
            //LMG
            if (CurrentWeapon == 2 && !shooting)
            {
                if (Ammo[CurrentWeapon] > 0)
                {
                    StartCoroutine("ShootRevolver");
                }
                else
                {
                    if (!reloading)
                    {
                        StartCoroutine("ReloadRevolver");
                    }
                }
            }





        }
        else
        {
            animator.SetBool("Shooting", false);
        }











        //------------------------AIMING
        aiming = Input.GetMouseButton(1);
        animator.SetBool("Aiming", aiming);
        if (aiming)
        {
            if (CurrentWeapon != 2)
            {
                crosshair.ChangeCrosshair(0);
                crosshair.MachineGun = false;
            }
            else
                crosshair.MachineGun = true;
            //else crosshair.ChangeCrosshair(1);
            AccuracyMultiplier = 0;
        }
        else
        {
            crosshair.MachineGun = false;
            crosshair.ChangeCrosshair(WeaponCrosshairTypes[CurrentWeapon]);
            AccuracyMultiplier = 5e-05f;
        }
        if (!playerMovement.sliding && aiming)
        {
            playerMovement.Aiming = true;
        }
        else
            playerMovement.Aiming = false;



        if (Input.GetKeyDown(KeyCode.R))
        {
            if (!reloading)
            {
                if (CurrentWeapon == 1)
                {
                    StartCoroutine("ReloadRevolver");
                }
                else if (CurrentWeapon == 2)
                {
                    StartCoroutine("ReloadRevolver");
                }
            }
        }

        /*        if (Input.mouseScrollDelta.y >= 1 && !reloading)
                {
                    if (CurrentWeapon != InventorySize)
                    {
                        CurrentWeapon += 1;
                    }
                    else
                    {
                        CurrentWeapon = 0;
                    }


                    StartCoroutine("UpdateWeapon");
                }
                else if (Input.mouseScrollDelta.y <= -1 && !reloading)
                {
                    if (CurrentWeapon != 0)
                    {
                        CurrentWeapon -= 1;
                    }
                    else
                    {
                        CurrentWeapon = InventorySize;
                    }

                    StartCoroutine("UpdateWeapon");
                }
        */

        //                                                                                                                                                                  Changing weapon

        if ((Input.mouseScrollDelta.y != 0 && !reloading))
        {
            ChangeWeapon();
        }
        else if ((Input.GetKeyDown(KeyCode.Tab) && !reloading))
        {
            ChangeWeapon();
        }


        /*        if (!reloading)
                {
                    if (Input.GetKeyDown(KeyCode.Alpha1))
                    {
                        CurrentWeapon = 0;
                        StartCoroutine("UpdateWeapon");
                    }
                    if (Input.GetKeyDown(KeyCode.Alpha2))
                    {
                        CurrentWeapon = 1;
                        StartCoroutine("UpdateWeapon");
                    }
                    if (Input.GetKeyDown(KeyCode.Alpha3))
                    {
                        CurrentWeapon = 2;
                        StartCoroutine("UpdateWeapon");
                    }
                    if (Input.GetKeyDown(KeyCode.Alpha4))
                    {
                        CurrentWeapon = 3;
                        StartCoroutine("UpdateWeapon");
                    }

                }
        */



    }


    public IEnumerator UpdateWeapon()
    {
        animator.SetInteger("WeaponID", CurrentWeapon);
        animator.SetTrigger("ChangeWeapon");
        animator.transform.parent.GetComponent<Animator>().SetTrigger("ChangeWeapon");


        yield return new WaitForSeconds(0.2f);
        //Selects the chosen weapon
        for (int i = 0; i < transform.childCount; i++)
        {
            if (i == CurrentWeapon)
            {
                CurrentWeaponobj = transform.GetChild(i).gameObject;
                transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        crosshair.ChangeCrosshair(WeaponCrosshairTypes[CurrentWeapon]);
        crosshair.SecondaryMultiplier = IndividualAccuracyMultiplier[CurrentWeapon];
        crosshair.UniqueDisplacement = IndividualAccuracyOffset[CurrentWeapon];
        playerMovement.transform.parent.GetComponent<SyncValues>().playerWeapon = CurrentWeapon;
        UpdateAmmoTXT();
    }


    //This function was originally for when the revolver shoots, but it was easier to make one function that varied for all guns that shoot. Same for ReloadRevolver();
    //Was too lazy to change the name lol.
    public IEnumerator ShootRevolver()
    {
        Ammo[CurrentWeapon]--;
        muzzleFlashes[CurrentWeapon].transform.localEulerAngles = new Vector3(0,0,Random.Range(0,360));
        ShootLine[CurrentWeapon].transform.position = Vector3.zero;
        LineRenderer lineRenderer = ShootLine[CurrentWeapon].GetComponent<LineRenderer>();
        lineRenderer.enabled = true;
        RaycastHit hit;
        Inaccuracy = (crosshair.Velocity * AccuracyMultiplier * IndividualAccuracyMultiplier[CurrentWeapon]) + IndividualAccuracyOffset[CurrentWeapon];


        CamShake.Recoil(Recoil[CurrentWeapon]);
        if (Physics.Raycast(Cam.transform.position, Cam.transform.forward+new Vector3(Random.Range(Inaccuracy,-Inaccuracy), Random.Range(Inaccuracy, -Inaccuracy), Random.Range(Inaccuracy, -Inaccuracy)), out hit, 1000, canShoot)) 
        {
            lineRenderer.SetPosition(0, ShootArea[CurrentWeapon].transform.position);
            lineRenderer.SetPosition(1, hit.point);

            //Checking if the hit object is a player hitbox
            if(hit.collider.transform.gameObject.layer == 7)
            {
                /*                if(hit.collider.transform.parent.parent.GetComponent<SyncValues>().playerWeapon != 4)
                                {
                                }
                                else
                                {
                                }
                */


                //Cool effect when you hit a player
                crosshair.Hit();
                ShowDmg(dmg[CurrentWeapon]);
                //The one line that caused a lot of bother.
                hit.collider.transform.parent.parent.GetComponent<PhotonView>().RPC("RPC_HitPlayer", RpcTarget.All, dmg[CurrentWeapon],PhotonNetwork.LocalPlayer.ActorNumber);
                //Tells a player to take damage when hit.
                
                
                //ShowDmg(0);

            }
            else if (hit.collider.transform.gameObject.layer == 11)
            {
                //Cool effect when you hit a player
                crosshair.Hit();
                //ShowDmg(dmg[CurrentWeapon]);
                //The one line that caused a lot of bother.
                //hit.collider.transform.parent.parent.GetComponent<PhotonView>().RPC("RPC_HitPlayer", RpcTarget.All, dmg[CurrentWeapon]);
                //Tells a player to take damage when hit.


                ShowDmg(0);

            }
            else
            {
                //Hit Object
                // "par" is the variable for the effect played when you shoot something, e.g dust particles
                GameObject par = revolverShootPars[CurrentRevolverShootPars].gameObject;
                par.transform.position = hit.point;
                par.transform.rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);
                //Puts the particles in the right position


                //This "if" statement checks if the collider has a mesh renderer first, preventing the error.
                if (hit.collider.transform.GetComponent<MeshRenderer>() != null)
                {
                    par.GetComponent<ParticleSystemRenderer>().material = hit.collider.transform.GetComponent<MeshRenderer>().material;
                    //The purpose of this line was simply to make the debris particles match the colour of the object fired at.
                }


                par.GetComponent<ParticleSystem>().Play();

            }






            if (CurrentRevolverShootPars + 1 < revolverShootPars.Length)
            {
                CurrentRevolverShootPars++;
            }
            else
            {
                CurrentRevolverShootPars = 0;
            }


        }
        animator.SetTrigger("Click");
        shooting = true;
        /*        lineRenderer.colorGradient = bulletGradient1;
                yield return new WaitForEndOfFrame();
                lineRenderer.colorGradient = bulletGradient2;
                //yield return new WaitForEndOfFrame();
                yield return new WaitForEndOfFrame();
                lineRenderer.colorGradient = bulletGradient3;
                yield return new WaitForEndOfFrame();
                yield return new WaitForEndOfFrame();
                yield return new WaitForEndOfFrame();
                yield return new WaitForEndOfFrame();
                yield return new WaitForEndOfFrame();
        */
        //lineRenderer.enabled = false;
        UpdateAmmoTXT();
        yield return new WaitForSeconds(TimeBtwShots[CurrentWeapon]);
        shooting = false;
    }
    public IEnumerator ReloadRevolver()
    {
        Ammo[CurrentWeapon] = 0;
        reloading = true;
        animator.SetTrigger("Reload");
        animator.SetBool("Reloading", true);
        yield return new WaitForSeconds(reloadTime[CurrentWeapon]);
        Ammo[CurrentWeapon] = MaxAmmo[CurrentWeapon];
        //Debug.Log(Ammo[CurrentWeapon]);
        animator.SetBool("Reloading", false);
        UpdateAmmoTXT();
        yield return new WaitForSeconds(0.2f);
        reloading = false;
    }






    public IEnumerator LeftClick()
    {
        animator.SetBool("LeftClick", true);

        yield return new WaitForEndOfFrame();
        animator.SetBool("LeftClick", false);

        canLeftClick = false;

        yield return new WaitForSeconds(0.2f);

        canLeftClick = true;






    }

    public void ShowDmg(int damage)
    {
        hittxtanims[hittxtnum].SetInteger("num", Random.Range(2, 5));
        hittxtanims[hittxtnum].SetTrigger("hit");
        hittxtanims[hittxtnum].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = damage.ToString();
        hittxtanims[hittxtnum].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = damage.ToString();

        hittxtnum++;
        if (hittxtnum == hittxtanims.Length)
        {
            hittxtnum = 0;
        }

    }



    public void ChangeWeapon()
    {
        if (Role == 1)
        {
            if (CurrentWeapon == 0)
                CurrentWeapon = 4;

            else
                CurrentWeapon = 0;
        }
        else if (Role == 0)
        {


            if (CurrentWeapon == 1)
                CurrentWeapon = 5;

            else
                CurrentWeapon = 1;

        }
        else if (Role == 2)
        {
            if (CurrentWeapon == 2)
                CurrentWeapon = 3;

            else
                CurrentWeapon = 2;
        }


        StartCoroutine("UpdateWeapon");

    }
    public void UpdateAmmoTXT()
    {


        if (CurrentWeapon == 1 || CurrentWeapon ==2)
        {
            ammotxt1.text = Ammo[CurrentWeapon].ToString() + "/" + MaxAmmo[CurrentWeapon].ToString();
            ammotxt2.text = Ammo[CurrentWeapon].ToString() + "/" + MaxAmmo[CurrentWeapon].ToString();
            ammotxt1.transform.parent.GetChild(0).GetComponent<Image>().enabled = true;
            ammotxt1.transform.parent.GetChild(3).GetComponent<RawImage>().enabled = true;
        }
        else
        {
            ammotxt1.transform.parent.GetChild(0).GetComponent<Image>().enabled = false;
            ammotxt1.transform.parent.GetChild(3).GetComponent<RawImage>().enabled = false;
            ammotxt1.text = "";
            ammotxt2.text = "";

        }


    }
    /*    public void HitPlayer(int view,int dmg)
        {
            Debug.Log(view);
            Debug.Log(PhotonNetwork.LocalPlayer.ActorNumber);
            if (PhotonNetwork.LocalPlayer.ActorNumber == view)
            {
                healthManager.LoseHealth(dmg);
            }
        }
    */

}
