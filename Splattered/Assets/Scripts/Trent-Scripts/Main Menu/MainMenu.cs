// Trent Lucas
// 25 June 2023

// Main Menu class
// Contains functions to control the user interactions
// and visuals in the main menu screen
///////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    // Make cursor visible
    void Start()
    {
        Cursor.visible = true;
    }

    // Changes the scene when the play button is pressed
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }
}
