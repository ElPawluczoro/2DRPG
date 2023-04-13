using System.Collections;
using System.Collections.Generic;
//using UnityEditor.UIElements;
using UnityEngine;

public class CameraFollowScript : MonoBehaviour
{
    public GameObject mainCamera;
    private GameObject playerCharacter;

    private float xPlayerPos;
    private float yPlayerPos;
    private float xCameraPos;
    private float yCameraPos;

    // Start is called before the first frame update
    void Start()
    {
        playerCharacter = GameObject.FindGameObjectWithTag("PlayerCharacter");
        //mainCamera.transform.position = playerCharacter.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        xPlayerPos = playerCharacter.transform.position.x;
        yPlayerPos = playerCharacter.transform.position.y;
        xCameraPos = mainCamera.transform.position.x;
        yCameraPos = mainCamera.transform.position.y;

        if (mainCamera.transform.position.x != playerCharacter.transform.position.x || mainCamera.transform.position.y != playerCharacter.transform.position.y)
        {
            mainCamera.transform.position += (new Vector3(xPlayerPos, yPlayerPos, 0) - new Vector3(xCameraPos, yCameraPos, 0)) * Time.deltaTime;
        }
    }
}
