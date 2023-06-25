// Trent Lucas
// 25 June 2023

// Loads the next level
// Stops timer to not run during loading times
////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NextLevel : MonoBehaviour
{

    public GameObject player;
    public int levelToLoad;
    public Vector2 minXY;
    public Vector2 maxXY;
    public bool onDeath;
    public Timer timer;

    // Checks if player has reached the finish line
    void Update()
    {
        if ((player.transform.position.x > minXY[0] && player.transform.position.x < maxXY[0]) && player.transform.position.y > minXY[1] && player.transform.position.y < maxXY[1]) {
            loadLevel(levelToLoad);
        }
        else if (onDeath == true) {
            
        }
    }

    // load X level
    void loadLevel(int level) {
        // null case checker
        if (timer != null) {
            timer.StopTimer();
        }
        SceneManager.LoadScene(level);
    }

    // End on screen timer
    void OnDisable() {
        if (timer != null) {
            PlayerPrefs.SetFloat("time", Mathf.Round(timer.currentTime * 100f) / 100f);
        }
    }
}
