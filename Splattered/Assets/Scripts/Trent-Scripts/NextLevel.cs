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

    void Update()
    {
        if ((player.transform.position.x > minXY[0] && player.transform.position.x < maxXY[0]) && player.transform.position.y > minXY[1] && player.transform.position.y < maxXY[1]) {
            loadLevel(levelToLoad);
        }
        else if (onDeath == true) {
            
        }
    }

    void loadLevel(int level) {
        SceneManager.LoadScene(level);
    }
}
