using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    
    private float health=100f;
    public float currentHealth=100f;
    private int damageTaken = 20;
    private int healTaken = 15;
    private float cooldown = 3; //time the player has to wait out of light for healing
    private float cdTimer; //cd timer
    private GameObject enemy;

    // Start is called before the first frame update
    void Start()
    {

        if (this.name == "Player")
        {
            enemy = GameObject.Find("Player2");
        }
        else
        {
            enemy = GameObject.Find("Player");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GetDamage()
    {
        if (/*enemy.GetComponent<PlayerLight>().visibleEnemies.Contains(this.GetComponent<Transform>()) &&*/ currentHealth>0) //In light
        {
            cdTimer = 0;
            currentHealth -= Time.deltaTime * damageTaken;
            
        }
        else if(currentHealth>0 && currentHealth<health)
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
            
        }
     
    }
}
