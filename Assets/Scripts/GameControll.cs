using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.UIElements;
using System;

public class GameControll : MonoBehaviour
{
    public UnityEngine.UIElements.Label LevelLabel;
    public ProgressBar levelBar;
    public ProgressBar healthBar;
    public static GameObject inventory;
    public GameObject gear;
    public Camera camera;
    public static GameObject itemTooltip;
    public static GameObject resurectPoint;

    public GameObject player;
    public static GameObject CharacterPanel;


    // Start is called before the first frame update
    void Start()
    {
        resurectPoint = GameObject.FindGameObjectWithTag("ResurectPoint");
        inventory = GameObject.FindWithTag("Inventory");
        player = GameObject.FindWithTag("PlayerCharacter");
        itemTooltip = GameObject.FindWithTag("ItemTooltip");
        CharacterPanel = GameObject.FindGameObjectWithTag("Gear");

        var root = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<UIDocument>().rootVisualElement;

        LevelLabel = root.Q<UnityEngine.UIElements.Label>("Level");
        levelBar = root.Q<ProgressBar>("ExpBar");
        healthBar = root.Q<ProgressBar>("HealthBar");
        
        //levelBar.highValue = player.GetComponent<HeroScript>().maxExp;



    }

    // Update is called once per frame
    void Update()
    {
        GetRightClickOnItem();

        HeroScript playerScript =  player.GetComponent<HeroScript>();
        UnitScript playerUnitScript = player.GetComponent<UnitScript>();

        LevelLabel.text = playerScript.level + " lvl";
        healthBar.value = playerUnitScript.health;
        healthBar.highValue = playerUnitScript.maxHealth;
        healthBar.title = playerUnitScript.health + "/" + playerUnitScript.maxHealth;
        levelBar.title = playerScript.exp + "/" + playerScript.maxExp;
        levelBar.value = playerScript.exp;
        levelBar.highValue = playerScript.maxExp;

        }

        public void GetRightClickOnItem(){
            Vector3 mousePosition = camera.ScreenToWorldPoint(Input.mousePosition);
        try{
            if (Input.GetMouseButtonDown(1)){
                Collider2D targetObject = Physics2D.OverlapPoint(mousePosition);
                if (targetObject.CompareTag("Item")){
                    targetObject.GetComponent<Item>().Use();
                }
            }
        }catch(NullReferenceException e){
        }

        // if (Input.GetKeyDown(KeyCode.I)){
        //     if(!inventory.activeSelf){
        //         inventory.SetActive(true);
        //     }
        //     else{
        //         inventory.SetActive(false);
        //     }
        // }


        if (Input.GetKeyDown(KeyCode.I)){
            var canvas = inventory.transform.parent.GetComponent<Canvas>();
            if(!canvas.enabled){
                canvas.enabled = true;
            }
            else{
                canvas.enabled = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.C)){
            var canvas = gear.transform.parent.GetComponent<Canvas>();
            if(!canvas.enabled){
                canvas.enabled = true;
            }
            else{
                canvas.enabled = false;
            }
        }
    }
}
