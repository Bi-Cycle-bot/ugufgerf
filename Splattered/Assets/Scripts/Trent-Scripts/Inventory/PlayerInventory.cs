using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{

    [Header("General")]
    public List<itemType> inventoryList;
    public int selectedItem = -1;
    public int totalTools = 2;

    [Space(20)]
    [Header("Items")]
    public GameObject item1;
    public GameObject item2;
    public GameObject item3;

    public Camera cam;

    private Dictionary<itemType, GameObject> itemSetActive = new Dictionary<itemType, GameObject>();
    private Dictionary<itemType, GameObject> itemInstantiate = new Dictionary<itemType, GameObject>();

    public Image[] inventorySlotImage = new Image[2];
    public Image[] inventoryBackgroundImage = new Image[2];



    void Start()
    {
        itemSetActive.Add(itemType.Gun, item1);
        itemSetActive.Add(itemType.Grenade, item2);
        // itemSetActive.Add(itemType.Skill1, item3);

        itemInstantiate.Add(itemType.Gun, item1);
        itemInstantiate.Add(itemType.Grenade, item2);
        // itemInstantiate.Add(itemType.Skill1, item3);

        selectedItem = 0;
        item1.SetActive(false);
        item2.SetActive(false);
        // item3.SetActive(false);
        NewItemSelected();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Alpha1) && inventoryList.Count > 0)
        {
            if (selectedItem != 0) {
                unequipTools();
                selectedItem = 0;
                NewItemSelected();
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && inventoryList.Count > 1)
        {
            if (selectedItem != 1) {
                unequipTools();
                selectedItem = 1;
                NewItemSelected();
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && inventoryList.Count > 2)
        {
            if (selectedItem != 2) {
                unequipTools();
                selectedItem = 2;
                NewItemSelected();
            }
        }

        if (Input.mouseScrollDelta.y < 0) {
            unequipTools();
            selectedItem--;
            if (selectedItem < 0) {
                selectedItem = totalTools - 1;
            }
            NewItemSelected();
        } else if (Input.mouseScrollDelta.y > 0) {
            unequipTools();
            selectedItem++;
            if (selectedItem > (totalTools - 1)) {
                selectedItem = 0;
            }
            NewItemSelected();
        }

        for (int i = 0; i < 2; i++)
        {
            if (i < inventoryList.Count)
            {
                inventorySlotImage[i].sprite = itemSetActive[inventoryList[i]].GetComponent<Item>().itemScriptableObject.item_sprite;
            }
            else
            {
                inventorySlotImage[i].sprite = null;
            }
        }

        int a = 0;
        foreach (Image image in inventoryBackgroundImage)
        {
            if (a == selectedItem)
            {
                image.color = new Color32(145, 255, 126, 255);
            }
            else
            {
                image.color = new Color32(219, 219, 219, 255);
            }
            a++;
        }
    }

    private void unequipTools() {
        if (selectedItem == 0) {
            Tool toolScript = item1.GetComponent<Tool>();
            toolScript.unequip();
        } else if (selectedItem == 1) {
            Tool toolScript = item2.GetComponent<Tool>();
            toolScript.unequip();
        } else if (selectedItem == 2) {
            Tool toolScript = item3.GetComponent<Tool>();
            toolScript.unequip();
        }
    }

    private void NewItemSelected()
    {
        // item1.SetActive(false);
        // item2.SetActive(false);
        // item3.SetActive(false);

        GameObject selectedItemGameObject = itemSetActive[inventoryList[selectedItem]];
        if (selectedItem == 0) {
            Tool toolScript = item1.GetComponent<Tool>();
            toolScript.equip();
        } else if (selectedItem == 1) {
            Tool toolScript = item2.GetComponent<Tool>();
            toolScript.equip();
        } else if (selectedItem == 2) {
            Tool toolScript = item3.GetComponent<Tool>();
            toolScript.equip();
        }
        selectedItemGameObject.SetActive(true);
    }

}

public interface IPickable
{
    void PickItem();
}