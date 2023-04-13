using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;

public class PlayerScript : MonoBehaviour
{
    public GameObject playerStatus;

    public void StartCoRoutineShowStatus(string status){
        StartCoroutine(ShowStatus(status));
    }

    public IEnumerator ShowStatus(string status){
        playerStatus.GetComponent<TextMeshPro>().text = status;
        yield return new WaitForSeconds(1);
        playerStatus.GetComponent<TextMeshPro>().text = "";
    }
}
