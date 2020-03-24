using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    //Public Variables
    public float health=100f;
    public float currentHealth;
    public float cooldown = 3; //time the player has to wait out of light for healing

    public int damageTaken = 20;

    public bool takingDamage;

    //Private Variables
    private int healTaken = 15;
    
    public float cdTimer; //cd timer

    private GameObject enemy;

    // Start is called before the first frame update
    void Awake()
    {
        currentHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        if (cdTimer > 0)
        {
            cdTimer -= Time.deltaTime;
        }
        else
        {
            if (currentHealth < health)
            {
                currentHealth += healTaken * Time.deltaTime;
                if (currentHealth > health)
                {
                    currentHealth = health;
                }
            }
        }
    }

    public void GetDamage()
    {
        Debug.Log("Llamado en " + gameObject.name);
        cdTimer = 0;
        currentHealth -= Time.deltaTime * damageTaken;

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }

        if (/*enemy.GetComponent<PlayerLight>().visibleEnemies.Contains(this.GetComponent<Transform>()) &&*/ currentHealth>0) //In light
        {
            
        }
        /*else if(currentHealth>0 && currentHealth<health)
        {
            if (cdTimer < 1)
            {
                cdTimer += Time.deltaTime / cooldown;
            }
            else
            {
                currentHealth += Time.deltaTime * healTaken;
                if (currentHealth > health) currentHealth = health;
            }
            
        }*/
    }

    public void SetTakingDamage(bool b)
    {
        takingDamage = b;
    }

    public bool GetTakingDamage() { return takingDamage; }

    public void SetMaximumCDTimer()
    {
        cdTimer = cooldown;
    }
}
