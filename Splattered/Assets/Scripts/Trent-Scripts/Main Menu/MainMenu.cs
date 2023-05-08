using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public TextMeshPro theText;

    void Start() {
        Cursor.visible = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        theText.color = Color.red;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        theText.color = Color.white;
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }
}
