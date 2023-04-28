using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour, IInventory
{

    public int Money { get => _money; set => _money = value; }

    private int _money = 0;

    [Header("General")]
    public List<itemType> inventoryList;
    public int selectedItem;
    public float playerReach;
    [SerializeField] GameObject throwItem_gameObject;

    [Space(20)]
    [Header("Keys")]
    [SerializeField] KeyCode throwItemKey;
    [SerializeField] KeyCode pickItemKey;

    [Space(20)]
    [Header("Keys")]
    [SerializeField] GameObject gun_item;
    [SerializeField] GameObject grenade_item;
    [SerializeField] GameObject skill1_item;

    [Space(20)]
    [Header("Item prefabs")]
    [SerializeField] GameObject gun_prefab;
    [SerializeField] GameObject grenade_prefab;
    [SerializeField] GameObject skill1_prefab;

    [SerializeField] Camera cam;
    [SerializeField] GameObject pickUpItem_gameObject;

    private Dictionary<itemType, GameObject> itemSetActive = new Dictionary<itemType, GameObject>() { };
    private Dictionary<itemType, GameObject> itemInstantiate = new Dictionary<itemType, GameObject>() { };

    [SerializeField] Image[] inventorySlotImage = new Image[4];
    [SerializeField] Image[] inventoryBackgroundImage = new Image[4];
    [SerializeField] Sprite emptySlotSprite;



    void Start()
    {
        itemSetActive.Add(itemType.Gun, gun_item);
        itemSetActive.Add(itemType.Grenade, grenade_item);
        itemSetActive.Add(itemType.Skill1, skill1_item);

        itemInstantiate.Add(itemType.Gun, gun_prefab);
        itemInstantiate.Add(itemType.Grenade, grenade_prefab);
        itemInstantiate.Add(itemType.Skill1, skill1_prefab);

        NewItemSelected();
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, playerReach))
        {
            Debug.Log("Touched");
            IPickable item = hitInfo.collider.GetComponent<IPickable>();
            if (item != null)
            {
                Debug.Log("Touched2");
                pickUpItem_gameObject.SetActive(true);
                if (Input.GetKey(pickItemKey))
                {
                    inventoryList.Add(hitInfo.collider.GetComponent<ItemPickable>().itemScriptableObject.item_type);
                    item.PickItem();
                }

            }
            else
            {
                pickUpItem_gameObject.SetActive(false);
            }
        }
        else
        {
            pickUpItem_gameObject.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) && inventoryList.Count > 0)
        {
            selectedItem = 0;
            NewItemSelected();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && inventoryList.Count > 1)
        {
            selectedItem = 1;
            NewItemSelected();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && inventoryList.Count > 2)
        {
            selectedItem = 2;
            NewItemSelected();
        }

        if (Input.GetKeyDown(throwItemKey) && inventoryList.Count > 1)
        {
            Instantiate(itemInstantiate[inventoryList[selectedItem]], position: throwItem_gameObject.transform.position, new Quaternion());
            inventoryList.RemoveAt(selectedItem);

            if (selectedItem != 0)
            {
                selectedItem -= 1;
            }
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
                inventorySlotImage[i].sprite = emptySlotSprite;
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
        gun_item.SetActive(false);
        grenade_item.SetActive(false);
        skill1_item.SetActive(false);

        GameObject selectedItemGameObject = itemSetActive[inventoryList[selectedItem]];
        selectedItemGameObject.SetActive(true);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Gun")
        {
            Debug.Log("A");
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.tag == "Grenade")
        {
            Debug.Log("B");
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.tag == "Skill")
        {
            Debug.Log("C");
            Destroy(collision.gameObject);
        }
    }
}

public interface IPickable
{
    void PickItem();
}
