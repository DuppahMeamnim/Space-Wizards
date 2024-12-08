using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Singleton

    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance is null)
            {
                Debug.LogError("Game manager is null");
            }

            return _instance;
        }
    }

    #endregion


    [HideInInspector] public bool isGameFail = false;


    private void Awake()
    {
        _instance = this;
    }

    public void FailGame()
    {
        isGameFail = true;
        SceneManager.LoadScene(0);
    }
}