using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController instance;
    public int currentHealth;
    public int maxHealth;

    private void Awake()
    {
        instance = this;
    }

    public void updateHealth()
    {
        UIController.instance.healthSlider.maxValue = maxHealth;
        UIController.instance.healthSlider.value = currentHealth;
        UIController.instance.healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();
    }
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        updateHealth();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DamagePlayer()
    {
        currentHealth--;

        if(currentHealth<= 0)
        {
            PlayerController.instance.gameObject.SetActive(false);

            UIController.instance.deathScreen.SetActive(true);
        }

        updateHealth();
    }
}
