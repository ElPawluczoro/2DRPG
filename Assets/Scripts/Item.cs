using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
//using static UnityEditor.Progress;

public enum ItemKind
{
    GEAR, CONSUMABLE, BACKPACK
}

public class Item : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image image;
    public Transform parentAfterDrag;
    public Camera camera;

    public UnitScript playerUnitScript;
    public HeroScript heroScript;

    public GameObject inventory;
    public GameObject gear;
    public GameObject backpacks;

    //item stats
    public string itemName;

    public float healthBonus;
    public float atackDamageBonus;
    public float abilityPowerBonus;
    public float armourBonus;
    public float magicResBonus;

    public int itemSlots;


    //item properties
    public bool destroyOnUse;
    public int restoreHealth;

    public ItemKind itemKind;


    public bool isDragingModeOn = false;

    void OnEnable(){
        image = this.GetComponent<Image>();
        camera = Camera.main;
        playerUnitScript = GameObject.FindGameObjectWithTag("PlayerCharacter").GetComponent<UnitScript>();
        heroScript = GameObject.FindGameObjectWithTag("PlayerCharacter").GetComponent<HeroScript>();
    }

    void Start(){
        inventory = GameObject.FindGameObjectWithTag("Inventory");
        gear = GameObject.FindGameObjectWithTag("Gear");
    }

    void Update(){
        if (Input.GetMouseButtonDown(0)){
            isDragingModeOn = true;
        }
        if (Input.GetMouseButtonUp(0)){
            isDragingModeOn = false;
        }
    }

    private bool fromGearSlot;


    public void OnBeginDrag(PointerEventData eventData){
        if(isDragingModeOn){
            if (gameObject.transform.parent.tag == "GearSlot")
            {
                fromGearSlot = true;
            }
            parentAfterDrag = transform.parent;
            transform.SetParent(transform.root);
            transform.SetAsLastSibling();
            image.raycastTarget = false;

            DisableItemToolTip();
        }
    }
    public void OnDrag(PointerEventData eventData){
        if(isDragingModeOn){
            Vector3 cameraPosition = camera.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(cameraPosition.x, cameraPosition.y, 0);
        }
    }
    public void OnEndDrag(PointerEventData eventData){
        if(isDragingModeOn){
            transform.SetParent(parentAfterDrag);
            image.raycastTarget = true;
            if (fromGearSlot && parentAfterDrag.tag == "ItemSlot")
            {
                CharacterPanelScript.ModifyStatsFromGear(gameObject.GetComponent<Item>(), false);
            }
        }
    }

    public void OnMouseOver()
    {
        if (!isDragingModeOn)
        {
            Vector3 cameraPosition = camera.ScreenToWorldPoint(Input.mousePosition);
            GameControll.itemTooltip.transform.parent.GetComponent<Canvas>().enabled = true;
            var tooltiopRect = GameControll.itemTooltip.GetComponent<RectTransform>().rect;
            GameControll.itemTooltip.transform.position = new Vector3(cameraPosition.x + tooltiopRect.width / 2 * 0.01f + 0.01f, cameraPosition.y - tooltiopRect.height / 2 * 0.01f - 0.05f, 0);
            GameControll.itemTooltip.transform.Find("Name").GetComponent<TMP_Text>().text = this.itemName;
            string propertiesText = "";
            if (itemKind == ItemKind.CONSUMABLE) propertiesText += "Consumable\n";
            if (itemKind == ItemKind.GEAR) propertiesText += "Gear\n";
            if (itemKind == ItemKind.BACKPACK) propertiesText += "Backpack\n" + "+" + itemSlots + " itemslots\n";
            if (restoreHealth > 0) propertiesText += "Restore " + restoreHealth + "health\n";
            if (healthBonus > 0) propertiesText += "+" + healthBonus + " Health\n";
            if (atackDamageBonus > 0) propertiesText += "+" + atackDamageBonus + " Atack Damage\n";
            if (abilityPowerBonus > 0) propertiesText += "+" + abilityPowerBonus + " Ability Power\n";
            if (armourBonus > 0) propertiesText += "+" + armourBonus + " Armour\n";
            if (magicResBonus > 0) propertiesText += "+" + magicResBonus + " Magic Resist\n";
            GameControll.itemTooltip.transform.Find("Properties").GetComponent<TMP_Text>().text = propertiesText;
        }
    }

    public void OnMouseExit()
    {
        DisableItemToolTip();
    }

    public void DisableItemToolTip()
    {
        GameControll.itemTooltip.transform.parent.GetComponent<Canvas>().enabled = false;
    }

    public void Use()
    {
        if(restoreHealth > 0){
            playerUnitScript.RestoreHealth(restoreHealth);
        }

        if(itemKind == ItemKind.BACKPACK)
        {
            if(gameObject.transform.parent.tag == "BackpackSlot" && inventory.GetComponent<Inventory>().IsAvaiableEnoughSlotsToDelete(this))
            {
                inventory.GetComponent<Inventory>().AddItem(gameObject.GetComponent<Item>());
                Inventory.ModifyItemSlots(gameObject.GetComponent<Item>(), false);
                Destroy(gameObject);
                inventory.GetComponent<Inventory>().UpdateItemSlots();
                GameControll.itemTooltip.transform.parent.GetComponent<Canvas>().enabled = false;
            }
            else if (gameObject.transform.parent.tag == "ItemSlot" && Inventory.IsAvaiableBackpackSlot())
            {
                inventory.GetComponent<Inventory>().AddBackpack(gameObject.GetComponent<Item>());
                Inventory.ModifyItemSlots(gameObject.GetComponent<Item>(), true);
                Destroy(gameObject);
                inventory.GetComponent<Inventory>().UpdateItemSlots();
                GameControll.itemTooltip.transform.parent.GetComponent<Canvas>().enabled = false;
            }
        }

        if(itemKind == ItemKind.GEAR){
            if(gameObject.transform.parent.tag == "GearSlot")
            {
                inventory.GetComponent<Inventory>().AddItem(gameObject.GetComponent<Item>());
                CharacterPanelScript.ModifyStatsFromGear(gameObject.GetComponent<Item>(), false);
                Destroy(gameObject);
                GameControll.itemTooltip.transform.parent.GetComponent<Canvas>().enabled = false;
            }
            else if(gameObject.transform.parent.tag == "ItemSlot" && GameControll.CharacterPanel.GetComponent<CharacterPanelScript>().IsFreeGearSlot())
            {
                gear.GetComponent<CharacterPanelScript>().AddItem(gameObject.GetComponent<Item>());
                Destroy(gameObject);
                GameControll.itemTooltip.transform.parent.GetComponent<Canvas>().enabled = false;
            }

        }

        if(destroyOnUse){
            Destroy(gameObject);
            GameControll.itemTooltip.transform.parent.GetComponent<Canvas>().enabled = false;
        }
    }


}
