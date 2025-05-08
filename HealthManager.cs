using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
using System.Linq;
public class HealthManager : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI txt1;
    [SerializeField] private TextMeshProUGUI txt2;
    [SerializeField] int HP = 100;
    [SerializeField] Animator takeDamagefx;
    [SerializeField] PlayerMovement Player;
    private bool ded;

    public bool IsOnFire;
    [SerializeField] private Image fireOverlay;

    private float timeSinceLastHit;
    private float originalRegenSpeed = 3;
    private float currentRegenSpeed;
    private float timeSinceLastRegen;


    private Coroutine coroutine;

    public int playerResponsible;

    public int Kills;


    //private List<PhotonView> views = new List<PhotonView>();
    public PhotonView[] views;
    //private int[] kills;
    private void Awake()
    {
        if (!transform.parent.parent.GetComponent<PhotonView>().IsMine)
        {
            Destroy(this);
        }
        IsOnFire = false;
        fireOverlay.enabled = false;
        playerResponsible = -12;
        
    }

    void Update()
    {
        if (HP < 1 && !ded)
        {
            StartCoroutine("Die");
        }

/*        if (Input.GetKeyDown(KeyCode.L))
        {
            LoseHealth(40);
        }
*/

        if (HP < 100)
        {
            if ((Time.time - timeSinceLastHit) > 5)
            {
                if ((Time.time - timeSinceLastRegen) > currentRegenSpeed)
                {
                    RegenHP();
                    if(currentRegenSpeed>0.3f)
                        currentRegenSpeed -= 0.2f;
                }


            }
        }
    }

    public void LoseHealth(int damage,int responsible)
    {
        if (!ded)
        {
            playerResponsible = responsible;
            Debug.Log("LKose healt");
            HP -= damage;
            healthSlider.value = HP;
            txt1.text = HP.ToString();
            txt2.text = HP.ToString();
            if (takeDamagefx != null)
                takeDamagefx.SetTrigger("Hit");
            timeSinceLastHit = Time.time;
            currentRegenSpeed = originalRegenSpeed;
        }
    }
    private IEnumerator Die()
    {
        if (takeDamagefx != null)
            takeDamagefx.SetBool("Dead", true);
        ded = true;
        if (coroutine != null)
            StopCoroutine(coroutine);
        IsOnFire = false;
        fireOverlay.enabled = false;

        yield return new WaitForEndOfFrame();
        HP = 100;
        healthSlider.value = HP;
        txt1.text = HP.ToString();
        txt2.text = HP.ToString();
        Player.StartCoroutine("DEAD");

        yield return new WaitForSeconds(2);
        ded = false;
        if (takeDamagefx != null)
            takeDamagefx.SetBool("Dead", false);

        //Debug.Log(playerResponsible);
        
        for (int i = 0; i < GameObject.FindObjectsOfType<SyncValues>().Length; i++)
        {
            //Debug.Log(GameObject.FindObjectsOfType<SyncValues>()[i].GetComponent<PhotonView>().name);
            views[i] = GameObject.FindObjectsOfType<SyncValues>()[i].GetComponent<PhotonView>();
            //Debug.Log(views[i]);
            //views.SetValue(GameObject.FindObjectsOfType<SyncValues>()[i].GetComponent<PhotonView>(), i);
        }
        //views.Sort();

        //Debug.Log(views);
        if (playerResponsible != -12)
        {
            for (int i = 0; i < views.Length; i++) 
            {
                //Debug.Log("H"+views[i].gameObject.name);
                if (views[i] != null)
                {
                    if (views[i].OwnerActorNr == playerResponsible)
                    {
                        views[i].RPC("RPC_GiveKill", RpcTarget.All);
                    }
                }
            }
            //PhotonNetwork.PlayerList[playerResponsible]
        }
        playerResponsible = -12;

        //SceneManager.LoadScene("Menu");
    }


    public void STARTFIRE(bool WasInflictedByOtherPlayer, int playerID)
    {
        if(coroutine != null)
            StopCoroutine(coroutine);
        coroutine = StartCoroutine(SetOnFire(WasInflictedByOtherPlayer, playerID));
    }

    public IEnumerator SetOnFire(bool WasInflictedByOtherPlayer,int playerID)
    {
        
        IsOnFire = true;
        fireOverlay.enabled = true;
        for (int i = 0; i < 10; i++)
        {
            LoseHealth(5,playerResponsible);
            yield return new WaitForSeconds(0.5f);
        }
        IsOnFire = false;
        fireOverlay.enabled = false;
    }



    public void RegenHP()
    {
        timeSinceLastRegen = Time.time;
        HP += 1;
        healthSlider.value = HP;
        txt1.text = HP.ToString();
        txt2.text = HP.ToString();

    }

    public void UpdateKills()
    {
        //Debug.Log("dshukjfiyjuhadsgfiylahds");
        transform.parent.parent.GetComponent<SyncValues>().playerKills = Kills;
    }
}
