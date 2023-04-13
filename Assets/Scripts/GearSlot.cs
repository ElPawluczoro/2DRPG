using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GearSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData){

        if(transform.childCount == 0)
        {
            GameObject dropped = eventData.pointerDrag;
            Item item = dropped.GetComponent<Item>();
            if(item.itemKind == ItemKind.GEAR){
                item.parentAfterDrag = transform;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
