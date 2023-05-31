using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthManager : MonoBehaviour
{

    [SerializeField] private Slider healthBar;
    //[SerializeField] private Image healthFill;
    private GameObject boss;
    private SpawnBoss spawnScript;
    private RabbitBoss bossScript;

    // Start is called before the first frame update
    void Start()
    {
        
        boss = GameObject.Find("Final Boss Spawner");
        spawnScript = boss.GetComponent<SpawnBoss>();
        //bossScript = spawnScript.bossCopy[0];
        //SetInitialHealth(bossScript.maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if(spawnScript.spawned){
            bossScript = spawnScript.bossCopy[0];
            SetInitialHealth(bossScript.maxHealth);
            UpdateHealth(bossScript.currentHealth);
        }
    }



    public void SetInitialHealth(float maxHealth)
    {
        healthBar.maxValue = maxHealth;
        healthBar.value = maxHealth;
    }

    public void UpdateHealth(float cHealth)
    {
        healthBar.value = cHealth;
        //Debug.Log("health value: " + playerScript.health);
    }
}
