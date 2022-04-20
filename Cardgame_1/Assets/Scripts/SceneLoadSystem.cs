using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadSystem : MonoBehaviour
{
    public void ToBattleScene()
    {
        SceneManager.LoadScene("BattleScene");

    }
    public void ToDeckScene()
    {
        SceneManager.LoadScene("BuildDeck");
    }

    public void ToStoreScene()
    {
        SceneManager.LoadScene("Card Store");
    }
}
