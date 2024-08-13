using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum ClearType
{
    None,
    Fail,
    Clear
};

public class GameEndManager : MonoBehaviour
{
    public static GameEndManager Instance;

    [Header("Input")]
    [SerializeField] private InputDataSO input;

    private ClearType clearType;

    private void Awake()
    {
        Instance = this;

        clearType = ClearType.None;

        input = input.Init();
        input.OnMouseLeftButtonDownEvt += NextGame;
    }

    private void Start()
    {
        int sceneIdx = PlayerPrefs.GetInt("NextSceneIdx");
        GameObject stageObj = Resources.Load<GameObject>($"Stages/Stage_{sceneIdx}");

        Instantiate(stageObj, Vector3.zero, Quaternion.identity);
    }

    public void GameClearUI()
    {
        clearType = ClearType.Clear;
    }

    public void GameFailUI()
    {
        clearType = ClearType.Fail;
    }

    private void NextGame()
    {
        if (clearType == ClearType.None) return;

        int nextSceneIdx = clearType switch
        {
            ClearType.Clear => PlayerPrefs.GetInt("NextSceneIdx") + 1,
            ClearType.Fail => PlayerPrefs.GetInt("NextSceneIdx"),
            _ => 0
        };
        PlayerPrefs.SetInt("NextSceneIdx", nextSceneIdx);

        SceneManager.LoadScene("Game");
    }
}
