using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ProgressBar : MonoBehaviour
{
    public Image mask;
    public Text lifeText;
    public HealthSystem healthSystem;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void setHealthSystem(HealthSystem healthSystem)
    {
        this.healthSystem = healthSystem;
    }

    // Update is called once per frame
    void Update()
    {
        GetCurrentFill();
    }

    void GetCurrentFill ()
    {
        Debug.Log(healthSystem.currentHealth);
        float fillAmount = (float)healthSystem.currentHealth / (float)healthSystem.health;
        mask.fillAmount = fillAmount;
        lifeText.text = (int) healthSystem.currentHealth + "/" + healthSystem.health;
    }
}
