using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{

    public Inventory inventory;

    public void OnDrop(PointerEventData eventData){
        if(transform.childCount == 0){
            GameObject dropped = eventData.pointerDrag;
            Item item = dropped.GetComponent<Item>();
            if(item.itemKind == ItemKind.BACKPACK)
            {
                if (item.parentAfterDrag.transform.tag == "BackpackSlot" && inventory.IsAvaiableEnoughSlotsToDelete(item))
                {
                    item.parentAfterDrag = transform;
                    Inventory.ModifyItemSlots(item, false);
                }
                else item.parentAfterDrag = transform;
            }
            else item.parentAfterDrag = transform;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
