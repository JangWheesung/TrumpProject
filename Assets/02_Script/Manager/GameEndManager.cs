using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

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
    [Header("UI")]
    [SerializeField] private GameObject uiRoot;
    [SerializeField] private TMP_Text stageText;
    [SerializeField] private TMP_Text headlineText;
    [SerializeField] private TMP_Text messageText;
    [Header("Value")]
    [SerializeField] private string[] successMessages;
    [SerializeField] private string[] failMessages;
    [SerializeField] private int lastStageIdx;

    private ClearType clearType;

    private void Awake()
    {
        PlayerPrefs.SetInt("NextSceneIdx", 0);
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

        stageText.text = $"Stage_{sceneIdx}";
    }

    public void GameClearUI()
    {
        clearType = ClearType.Clear;

        uiRoot.SetActive(true);
        headlineText.text = "Yun is Death";
        messageText.text = GetRandomMessage(successMessages);
    }

    public void GameFailUI()
    {
        clearType = ClearType.Fail;

        uiRoot.SetActive(true);
        headlineText.text = "Trump is Alive";
        messageText.text = GetRandomMessage(failMessages);
    }

    private string GetRandomMessage(string[] messages)
    {
        int idx = Random.Range(0, messages.Length);
        return messages[idx];
    }

    private void NextGame()
    {
        if (clearType == ClearType.None) return;

        input.Dispose();

        int nowStage = PlayerPrefs.GetInt("NextSceneIdx");

        if (nowStage >= lastStageIdx)
        {
            SceneManager.LoadScene("Intro");
            return;
        }

        int nextSceneIdx = clearType switch
        {
            ClearType.Clear => nowStage + 1,
            ClearType.Fail => nowStage,
            _ => 0
        };
        PlayerPrefs.SetInt("NextSceneIdx", nextSceneIdx);

        SceneManager.LoadScene("Game");
    }

    private void OnDestroy()
    {
        input.Dispose();
    }
}
