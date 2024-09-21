using System.Collections;
using System.Collections.Generic;
using Fusion;
using GNW2.GameManager;
using UnityEngine;

public class StartGameUI : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject otherButton;
    // Start is called before the first frame update
    public void OnClickHost()
    {
        if (gameManager._runner == null)
        {

            gameManager.StartGame(GameMode.Host);
            gameObject.SetActive(false);
            otherButton.SetActive(false);
        }
    }
    public void OnClickJoin()
    {
        if (gameManager._runner == null)
        {

            gameManager.StartGame(GameMode.Client);
            gameObject.SetActive(false);
            otherButton.SetActive(false);
        }
    }
}
