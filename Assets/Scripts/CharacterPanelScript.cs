using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using TMPro;

public class CharacterPanelScript : MonoBehaviour
{

    public List<Transform> gearSlots;

    public TMP_Text characterNameGM;
    public TMP_Text statsGM;
    private static UnitScript heroScript;


    // Start is called before the first frame update
    void Start()
    {
        heroScript = GameObject.FindGameObjectWithTag("PlayerCharacter").GetComponent<UnitScript>();
        UpdateGearSlots();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.parent.GetComponent<Canvas>().enabled)
        {
            UpdateStats();
        }
    }

    public void UpdateGearSlots(){
        gearSlots.Clear();
        var gearSpace = transform.GetChild(0).transform.GetChild(0);
        for(int i = 0; i < gearSpace.transform.childCount; i++){
            gearSlots.Add(gearSpace.transform.GetChild(i));
        }
    }

    public void AddItem(Item item){
        foreach(Transform gearSlot in gearSlots){
            if(gearSlot.childCount == 0){
                var itemToAdd = Instantiate(item, gearSlot);
                ModifyStatsFromGear(item, true);
                break;
            }
        }
    }

    public bool IsFreeGearSlot()
    {
        foreach(var gearSlot in gearSlots)
        {
            if(gearSlot.childCount == 0)
            {
                return true;
            }
        }
        return false;
    }

    public static void ModifyStatsFromGear(Item item, bool add)
    {
        int plusOrMinus;
        if (add) plusOrMinus = 1;
        else plusOrMinus = -1;
        if (item.healthBonus > 0) { heroScript.maxHealth += item.healthBonus * plusOrMinus; heroScript.health += item.healthBonus * plusOrMinus; }
        if (item.atackDamageBonus > 0)  heroScript.atackDamage += item.atackDamageBonus * plusOrMinus;
        if (item.abilityPowerBonus > 0) heroScript.abilityPower += item.abilityPowerBonus * plusOrMinus;
        if (item.armourBonus > 0) heroScript.armour += item.armourBonus * plusOrMinus;
        if (item.magicResBonus > 0) heroScript.magicRes += item.magicResBonus * plusOrMinus;
    }

    public void UpdateStats()
    {
        characterNameGM.text = heroScript.name;
        statsGM.text = 
            "Health " + heroScript.health + "/" + heroScript.maxHealth + "\n" +
            "Atack Damage " + heroScript.atackDamage + "\n" +
            "Ability Power " + heroScript.abilityPower + "\n" +
            "Armour " + heroScript.armour + "\n" +
            "Magic Resistance" + heroScript.magicRes + "\n";
    }



}
