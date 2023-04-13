using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public enum EUnitAction 
{
    BASIC_ATACK, SKILL
}

public class BattleUiScript : MonoBehaviour
{

    public Button basicAtackButton;
    public ProgressBar playerHealth;
    public ProgressBar enemyHealth;

    public static VisualElement playerSprite;

    public static GameObject playerCharacter;
    public static GameObject monster;

    public GameObject playerSpriteHolder;
    public GameObject enemySpriteHolder;

    public UnitScript playerUnitScript;
    public UnitScript enemyUnitscript;

    public GameObject battlePanel;
    public GameObject gameControll;



    void OnEnable()
    //void Start()
    {
        playerUnitScript = playerCharacter.GetComponent<UnitScript>();
        enemyUnitscript = monster.GetComponent<UnitScript>();

        var root = GetComponent<UIDocument>().rootVisualElement;

        basicAtackButton = root.Q<Button>("BasicAtack");
        playerHealth = root.Q<ProgressBar>("PlayerHealthBar");
        enemyHealth = root.Q<ProgressBar>("EnemyHealthBar");

        playerHealth.title = playerUnitScript.name;
        playerHealth.highValue = playerUnitScript.maxHealth;
        playerHealth.value = playerUnitScript.health;

        enemyHealth.title = enemyUnitscript.name;
        enemyHealth.highValue = enemyUnitscript.maxHealth;

        //basicAtackButton.clicked += PlayerAction(EUnitAction.BASIC_ATACK);
        basicAtackButton.RegisterCallback<ClickEvent>(ev => PlayerAction(EUnitAction.BASIC_ATACK));

    }

    // Update is called once per frame
    void Update()
    {
        playerHealth.value = playerUnitScript.health;
        enemyHealth.value = enemyUnitscript.health;

        if (enemyUnitscript.health <= 0 || playerUnitScript.health <= 0)
        {
            BattleEnd();
        }
    }

    public void MonsterAction(EUnitAction monsterAction){
        if(monsterAction == EUnitAction.BASIC_ATACK){
            MonsterBasicAtack();
        }
    }

    public void MonsterBasicAtack(){
        UnitScript.BassicAttack(enemyUnitscript, playerUnitScript);
    }

    public void PlayerAction(EUnitAction playerAction){
        if(playerAction == EUnitAction.BASIC_ATACK){
            PlayerBasicAtack();
        }
        if(playerUnitScript.health > 0) 
        {
            MonsterAction(EUnitAction.BASIC_ATACK);
        }
    }

    public void PlayerBasicAtack()
    {
        UnitScript.BassicAttack(playerUnitScript, enemyUnitscript);
    }

    public void BattleEnd()
    {
        battlePanel.SetActive(false);
        gameControll.GetComponent<CameraFollowScript>().enabled = true;
        if (playerUnitScript.health <= 0)
        {
            playerCharacter.GetComponent<HeroScript>().Death();
        }
        else if (enemyUnitscript.health <= 0)
        {
            monster.GetComponent<MonsterScript>().Death();
            Destroy(monster);
        }
        monster = null;
        MovementScript.stop = false;
    }

}
