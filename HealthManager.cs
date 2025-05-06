using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
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

    private void Awake()
    {
        if (!transform.parent.parent.GetComponent<PhotonView>().IsMine)
        {
            Destroy(this);
        }
        IsOnFire = false;
        fireOverlay.enabled = false;

    }

    void Update()
    {
        if (HP < 1 && !ded)
        {
            Debug.LogWarning("DIE");
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

    public void LoseHealth(int damage)
    {
        if (!ded)
        {
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
            LoseHealth(5);
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


}
