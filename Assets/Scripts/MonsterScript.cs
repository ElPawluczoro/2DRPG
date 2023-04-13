using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterScript : MonoBehaviour
{
    public int expGiven;
    public List<Drop> dropList;
    public GameObject inventory;


    // Start is called before the first frame update
    void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Inventory");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Death()
    {
        HeroScript player = GameObject.FindGameObjectWithTag("PlayerCharacter").GetComponent<HeroScript>();
        player.GetExp(expGiven);
        Drop();

    }

    public void Drop(){
        var inv = inventory.GetComponent<Inventory>();
        /*        var itemToAdd = dropList[Random.Range(0, dropList.Count - 1)];
                inv.AddItem(itemToAdd.item);*/
        /*int itemRoll = Random.Range(1, 100);
        int dropValue = 0;
        foreach(Drop drop in dropList)
        {
            dropValue += drop.chance;
        }
        if(dropValue >= itemRoll)
        {
            foreach(Drop drop in dropList)
            {
                dropValue -= drop.chance;
                if(drop.chance <= 0)
                {
                    inv.AddItem(drop.item);
                }
                break;
            }
        }*/

        foreach(Drop drop in dropList)
        {
            if(Random.Range(1,100) <= drop.chance)
            {
                inv.AddItem(drop.item);
            }
        }

    }

}
