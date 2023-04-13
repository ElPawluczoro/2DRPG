using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class MovementScript : MonoBehaviour
{
    public GameObject playerCharacter;
    public float speed;
    public GameObject battlePanel;
    public GameObject gameControll;
    public Camera mainCamera;

    public GameObject testDungeon;

    public static bool stop;


    // Start is called before the first frame update
    void Start()
    {
        playerCharacter.transform.position = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if(!stop){
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            {
                playerCharacter.transform.position += new Vector3(0, speed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            {
                playerCharacter.transform.position -= new Vector3(0, speed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                playerCharacter.transform.position -= new Vector3(speed * Time.deltaTime, 0);
            }
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                playerCharacter.transform.position += new Vector3(speed * Time.deltaTime, 0);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("dungeonTeleport"))
        {
            GameObject dungeon = Instantiate(collision.gameObject.GetComponent<DungeonTeleportScript>().dungeon);
            dungeon.transform.position = new Vector3(15, 15, 0);


            playerCharacter.transform.position = dungeon.transform.position;

            //playerCharacter.transform.position = collision.gameObject.GetComponent<DungeonTeleportScript>().dungeon.transform.position;
        }

        if (collision.gameObject.CompareTag("enemy") && BattleUiScript.monster == null)
        {

            gameControll.GetComponent<CameraFollowScript>().enabled = false;
            stop = true;
            battlePanel.transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, 0);

            BattleUiScript.playerCharacter = GameObject.FindWithTag("PlayerCharacter");
            BattleUiScript.monster = collision.gameObject;

            battlePanel.SetActive(true);

            BattleUiScript battle = battlePanel.GetComponent<BattleUiScript>();

            battle.playerSpriteHolder.gameObject.GetComponent<SpriteRenderer>().sprite = playerCharacter.gameObject.GetComponent<SpriteRenderer>().sprite;
            battle.enemySpriteHolder.gameObject.GetComponent<SpriteRenderer>().sprite = BattleUiScript.monster.GetComponent<SpriteRenderer>().sprite;

        }
    }




}
