using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static Inventory;

public class Inventory : MonoBehaviour
{

    public Camera camera;
    public static List<Transform> itemSlots = new List<Transform>();
    public InventorySlot itemSlot;
    public GameObject inventorySpace;
    public GameObject backpacksSpace;
    public static List<Transform> backpacksSlots = new List<Transform>();

    public static HeroScript playerCharacterHeroScript;

    static int numberOfItemSLots = 0;

    [HideInInspector] public delegate void DManageItemSlots();
    [HideInInspector] public event DManageItemSlots eManageItemSlots;

    public void MManageItemSlots()
    {
        if(eManageItemSlots != null)
        {
            eManageItemSlots();
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        eManageItemSlots += ManageItemSlots;
        //playerCharacterHeroScript = GameObject.FindGameObjectWithTag("PlayerCharacter").GetComponent<HeroScript>();
        playerCharacterHeroScript = GameObject.FindGameObjectWithTag("PlayerCharacter").GetComponent<HeroScript>();

        SetItemSlots();
        UpdateItemSlots();
        UpdateBacpackSlots();

    }

    void OnEnable(){
        UpdateItemSlots();
    }

    public void Update()
    {
        StartCoroutine(manageItems());

        if(gameObject.transform.parent.GetComponent<Canvas>().enabled == true)
        {
            UpdateBacpackSlots();
        }

    }

    public IEnumerator manageItems()
    {
        yield return new WaitForSeconds(0.02f);
        ManageItemSlots();
    }

    public void ManageItemSlots()
    {
        UpdateItemSlots();
        var invSpace = gameObject.transform.GetChild(0).GetChild(0);
        if (invSpace.childCount > playerCharacterHeroScript.itemSlots)
        {
            var toDestroy = new List<Transform>();
            for (int i = 0; i < invSpace.childCount; i++)
            {
                if (invSpace.GetChild(i).childCount == 0)
                {
                    toDestroy.Add(invSpace.GetChild(i));
                    if (invSpace.childCount - toDestroy.Count == GameObject.FindGameObjectWithTag("PlayerCharacter").GetComponent<HeroScript>().itemSlots)
                    {
                        foreach (Transform t in toDestroy)
                        {
                            Destroy(t.gameObject);
                        }
                        break;
                    }
                }
            }
        }
        else if (invSpace.childCount < playerCharacterHeroScript.itemSlots)
        {
            int amount = playerCharacterHeroScript.itemSlots - invSpace.childCount;
            for (int i = 0; i < amount; i++)
            {
                Instantiate(itemSlot, invSpace);
            }
        }
    }


    public void CalculateItemSlots()
    {
        for (int i = 0; i < backpacksSpace.transform.childCount; i++)
        {
            if (backpacksSpace.transform.GetChild(i).childCount > 0)
            {
                numberOfItemSLots += backpacksSpace.transform.GetChild(i).GetChild(0).GetComponent<Item>().itemSlots;
            }
        }
    }

    public void UpdateItemSlots() {
/*        int itemS = GameObject.FindGameObjectWithTag("PlayerCharacter").GetComponent<HeroScript>().itemSlots;
        if (numberOfItemSLots != itemS) {
            if (itemS - numberOfItemSLots > 0)
            {
                for (int i = 0; i < playerCharacterHeroScript.itemSlots - numberOfItemSLots; i++)
                {
                    Instantiate(itemSlot, this.inventorySpace.transform);
                }
            }
            else
            {
                if(IsAvaiableEnoughSlotsToDelete())
                {
                    int slotsToDelete = (playerCharacterHeroScript.itemSlots - numberOfItemSLots) * -1;
                    int deleted = 0;
                    foreach (Transform itemslot in itemSlots)
                    {
                        if(itemslot.transform.childCount == 0)
                        {
                            Destroy(itemslot);
                            deleted++;
                        }
                        if(deleted == slotsToDelete)
                        {
                            break;
                        }
                    }
                }
            }
        }*/
        itemSlots.Clear();
        var inventorySpace = transform.GetChild(0).transform.GetChild(0);
        for(int i = 0; i < inventorySpace.transform.childCount; i++){
            itemSlots.Add(inventorySpace.transform.GetChild(i));
        }
    }

    public void SetItemSlots()
    {
        for (int i = 0; i < backpacksSpace.transform.childCount; i++)
        {
            if (backpacksSpace.transform.GetChild(i).childCount != 0)
            {
                var playerCharacterHeroScript2 = GameObject.FindGameObjectWithTag("PlayerCharacter").GetComponent<HeroScript>();
                var itemaaa = backpacksSpace.transform.GetChild(i).GetChild(0).GetComponent<Item>();
                playerCharacterHeroScript2.itemSlots += itemaaa.itemSlots;

            }
        }
        for (int i = 0; i < GameObject.FindGameObjectWithTag("PlayerCharacter").GetComponent<HeroScript>().itemSlots; i++) Instantiate(itemSlot, this.inventorySpace.transform);
    }

    public bool IsAvaiableEnoughSlotsToDelete(Item backpack)
    {
        if (GameObject.FindGameObjectWithTag("PlayerCharacter").GetComponent<HeroScript>().itemSlots > backpack.itemSlots && NumberOfFreeSlots() - 1 >= backpack.itemSlots)
        {
            return true;
        }
        else return false;
    }

    public static bool IsAvaiableBackpackSlot()
    {
        GameControll.inventory.GetComponent<Inventory>().UpdateBacpackSlots();
        foreach (var backpack in backpacksSlots)
        {
            if(backpack.childCount == 0)
            {
                return true;
            }
        }
        return false;
    }

    public int NumberOfFreeSlots()
    {
        var freeSlots = 0;
        for(int i = 0; i < itemSlots.Count; i++)
        {
            if (itemSlots[i].childCount == 0)
            {
                freeSlots++;
            }
        }
        return freeSlots;
    }


    public void UpdateBacpackSlots()
    {
        backpacksSlots.Clear();
        var backpacksSpace = transform.GetChild(1).GetChild(0);
        for (int i =0; i < backpacksSpace.transform.childCount; i++)
        {
            backpacksSlots.Add(backpacksSpace.transform.GetChild(i));
        }
    }

    public static void ModifyItemSlots(Item backpack, bool add)
    {
        int mod;
        if (add) mod = 1;
        else mod = -1;
        playerCharacterHeroScript.itemSlots += backpack.itemSlots * mod;

    }

    public void AddItem(Item item){
        foreach(Transform itemslot in itemSlots)
        {
            if(itemslot.childCount == 0){
                var itemToAdd = Instantiate(item, itemslot);
                //itemToAdd.transform.SetParent(itemslot);
                //item.transform.parent = itemslot;
                break;
            }
        }
    }

    public void AddBackpack(Item backpack)
    {
        foreach (Transform backpackSlot in backpacksSlots)
        {
            if (backpackSlot.childCount == 0)
            {
                var itemToAdd = Instantiate(backpack, backpackSlot);
                break;
            }
        }
    }

}
