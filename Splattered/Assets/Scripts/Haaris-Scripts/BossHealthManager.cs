using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthManager : MonoBehaviour
{

    [SerializeField] private Slider healthBar;
    //[SerializeField] private Image healthFill;
    private GameObject boss;
    //private PlayerMovement bossScript;
    //private PlayerMovementData bossDataScript;

    // Start is called before the first frame update
    void Start()
    {
        boss = GameObject.FindGameObjectWithTag("Boss");
        //bossScript = player.GetComponent<PlayerMovement>();
        //bossDataScript = player.GetComponent<PlayerMovementData>();

        //SetInitialHealth(bossDataScript.maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        //UpdateHealth(bossScript.health);
    }



    public void SetInitialHealth(float maxHealth)
    {
        healthBar.maxValue = maxHealth;
        healthBar.value = maxHealth;
    }

    public void UpdateHealth(float currentHealth)
    {
        healthBar.value = currentHealth;
        //Debug.Log("health value: " + playerScript.health);
    }
}
