using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using TMPro;

public class HeroScript : MonoBehaviour
{
    public int exp;
    public int maxExp = 100;
    public int level = 0;

    public int[] levelsExp = {100, 250, 500, 1000, 1750, 3000};

    public GameObject playerStatus;

    public int itemSlots = 0;

    public GameObject deathMessage;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(exp >= maxExp)
        {
            LevelUp();
        }
    }

    public void Death()
    {
        StartCoroutine(ShowStatus("Death...", Color.red));
        exp -= (int)(maxExp * 0.1f);
        if(exp < 0)
        {
            exp = 0;
        }
        gameObject.transform.position = new Vector3(GameControll.resurectPoint.transform.position.x, GameControll.resurectPoint.transform.position.y);
        gameObject.GetComponent<UnitScript>().health = 1;
        StartCoroutine(ShowDeathMessage());
    }
    public void GetExp(int xp)
    {
        exp += xp;
        StartCoroutine(ShowStatus("+" + xp + "xp", Color.green));
    }

    public void LevelUp()
    {
        exp -= maxExp;
        level++;
        maxExp = levelsExp[level];
        StartCoroutine(ShowStatus("Level Up", Color.green));
    }

    public IEnumerator ShowStatus(string status, Color color){
        playerStatus.GetComponent<TextMeshPro>().color = color;
        playerStatus.GetComponent<TextMeshPro>().text = status;
        yield return new WaitForSeconds(1);
        playerStatus.GetComponent<TextMeshPro>().text = "";
    }
    
    public IEnumerator ShowDeathMessage()
    {
        deathMessage.SetActive(true);
        gameObject.GetComponent<MovementScript>().enabled = false;
        yield return new WaitForSeconds(2);
        deathMessage.SetActive(false);
        gameObject.GetComponent<MovementScript>().enabled = true;
    }


}
