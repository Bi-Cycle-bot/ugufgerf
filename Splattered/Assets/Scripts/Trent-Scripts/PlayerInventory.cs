using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{

    [Header("General")]
    public List<itemType> inventoryList;
    public int selectedItem;

    [Space(20)]
    [Header("Items")]
    public GameObject item1;
    public GameObject item2;
    public GameObject item3;

    public Camera cam;

    private Dictionary<itemType, GameObject> itemSetActive = new Dictionary<itemType, GameObject>();
    private Dictionary<itemType, GameObject> itemInstantiate = new Dictionary<itemType, GameObject>();

    public Image[] inventorySlotImage = new Image[3];
    public Image[] inventoryBackgroundImage = new Image[3];



    void Start()
    {
        itemSetActive.Add(itemType.Gun, item1);
        itemSetActive.Add(itemType.Grenade, item2);
        itemSetActive.Add(itemType.Skill1, item3);

        itemInstantiate.Add(itemType.Gun, item1);
        itemInstantiate.Add(itemType.Grenade, item2);
        itemInstantiate.Add(itemType.Skill1, item3);

        NewItemSelected();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Alpha1) && inventoryList.Count > 0)
        {
            selectedItem = 0;
            NewItemSelected();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && inventoryList.Count > 1)
        {
            selectedItem = 1;
            NewItemSelected();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && inventoryList.Count > 2)
        {
            selectedItem = 2;
            NewItemSelected();
        }

        for (int i = 0; i < 3; i++)
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

    private void NewItemSelected()
    {
        item1.SetActive(false);
        item2.SetActive(false);
        item3.SetActive(false);

        GameObject selectedItemGameObject = itemSetActive[inventoryList[selectedItem]];
        selectedItemGameObject.SetActive(true);
    }

}

public interface IPickable
{
    void PickItem();
}