using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitScript : MonoBehaviour
{
    public string name;
    public float health;
    public float maxHealth;
    public float atackDamage;
    public float abilityPower;
    public float armour;
    public float magicRes;

    public static void BassicAttack(UnitScript atacker, UnitScript target)
    {
        target.health -= atacker.atackDamage;
    }

    public void RestoreHealth(int value){
        health += value;
        if(health > maxHealth){
            health = maxHealth;
        }
    }


}
