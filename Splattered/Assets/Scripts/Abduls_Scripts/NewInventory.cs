using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewInventory : MonoBehaviour
{
    //  Public Methods
    public List<GameObject> tools;
    public GameObject equippedMask;

    // Private Methods
    private int toolSize;
    private int currentlyEquipped = 1;
    private List<Transform> backgrounds = new List<Transform>();
    private RectTransform equipMaskRectTrans;

    // Start is called before the first frame update
    void Start() {
        toolSize = tools.Count;
        for (int i = 1; i <= toolSize; i++) {
            backgrounds.Add(transform.Find("Background_" + i));
        }
        for (int i = 1; i < toolSize; i++) {
            tools[i].SetActive(false);
        }
        equipMaskRectTrans = equippedMask.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update() {
        // KeyPresses
        for (int i = (int)KeyCode.Alpha0; i <= (int)KeyCode.Alpha9; i++) {
            if (Input.GetKeyDown((KeyCode)i)) {
                int realNum = i - 48;
                if (realNum <= toolSize && realNum != 0) {
                    equipTool(realNum);
                    break;
                }
            }
        }
    }

    private void equipTool(int tool) {
        if (currentlyEquipped == tool) { return; }
        Tool toolScript = tools[currentlyEquipped - 1].GetComponent<Tool>();
        toolScript.unequip();
        currentlyEquipped = tool;
        toolScript = tools[currentlyEquipped - 1].GetComponent<Tool>();
        toolScript.equip();
        equippedMask.transform.SetParent(backgrounds[currentlyEquipped - 1]);
        equipMaskRectTrans.anchoredPosition = new Vector2(0, 0);
    }
}
