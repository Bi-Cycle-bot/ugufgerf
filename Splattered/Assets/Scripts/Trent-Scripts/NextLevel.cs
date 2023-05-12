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

    void Update()
    {
        if (player.transform.position.x > 20) {
            loadLevel(levelToLoad);
        }
    }

    void loadLevel(int level) {
        SceneManager.LoadScene(level);
    }
}
