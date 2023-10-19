using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdontWantAddScript : MonoBehaviour
{
    public GameObject UI;

    private void OnMouseDown()
    {
        UI.SetActive(true);
        gameObject.transform.parent.GetComponent<LevelMNG>().setGameState();


        Destroy(gameObject);
    }
}
