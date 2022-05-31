using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using static InventorySlot;

public class Inventory : MonoBehaviour
{
    #region Singleton
    public static Inventory instance = null;

    void Awake()
    {
        if (instance == null) instance = this;
        else if (instance == this) {
            Debug.LogWarning("Multiple inventories!");
            Destroy(gameObject); 
        }      
    }
    #endregion

    InventoryUI inventoryUI;
    public List<Collectable> items = new List<Collectable>();
    public int space = 20;
    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;
    [SerializeField]
    private GameObject collectableGO;
    [SerializeField]
    private GameObject allCollectables;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        inventoryUI = InventoryUI.instance;
    }

    public bool Add(Collectable item)
    {
        if (space < items.Count) {
            Debug.Log("No space in inventory!");
            return false;
        }
        else {
            Collectable collectable = item;
            items.Add(collectable);
            inventoryUI.Add(collectable);
        };

        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();

        return true;
    }

    public List<Item> GetInventoryInItems() {

        List<Item> items = new List<Item>();
        foreach (var item in this.items)
            items.Add(item.item);
        return items;
    }

    public void Remove(Collectable item)
    {
        items.Remove(item);
        Debug.Log($"item removed {item}");
        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
    }

    public void Remove(Item item)
    {
        var itemToDelete = items.Where(x => x.item == item).FirstOrDefault();
        items.Remove(itemToDelete);

        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
    }

    public void DropItem(Collectable item)
    {
        GameObject dropItem = Instantiate(
                collectableGO,
                new Vector3(PlayerScript.instance.transform.position.x, PlayerScript.instance.transform.position.y, 1),
                PlayerScript.instance.transform.rotation,
                allCollectables.transform);

        dropItem.GetComponent<Collectable>().item = item.item;
        dropItem.GetComponent<Collectable>().bodySpritePart = item.bodySpritePart;
        dropItem.GetComponent<Collectable>().equipType = item.equipType;
        dropItem.GetComponent<Collectable>().buyPrice = item.buyPrice;
        dropItem.GetComponent<Collectable>().sellPrice = item.sellPrice;
        dropItem.GetComponent<Collectable>().isConsumable = item.isConsumable;
        dropItem.GetComponent<SpriteRenderer>().sprite = item.item.icon;


        //var a = Collectables.instance.collectables.Where(x => x.GetComponent<Collectable>()._name == item._name).First();
        //var objectToActivate = allCollectables.GetComponentsInChildren<GameObject>().Where(x => x.GetComponent<Collectable>() == item).FirstOrDefault();
        //Debug.Log(objectToActivate);
        //objectToActivate.SetActive(true);
    }
}
