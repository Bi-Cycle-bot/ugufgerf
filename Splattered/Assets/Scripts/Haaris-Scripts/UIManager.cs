using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    [SerializeField] private Slider healthBar;
    //[SerializeField] private Image healthFill;
    private GameObject player;
    private PlayerMovement playerScript;
    private PlayerMovementData playerDataScript;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerMovement>();
        playerDataScript = player.GetComponent<PlayerMovementData>();

        SetInitialHealth(playerDataScript.maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHealth(playerScript.health);
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
