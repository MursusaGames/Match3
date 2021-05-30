using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_0 : MonoBehaviour
{
    [SerializeField] GameObject exitPanel;

    public void ShowExitPanel()
    {
        exitPanel.SetActive(true);
    }
    public void HideExitPanel()
    {
        exitPanel.SetActive(false);
    }
    private void Start()
    {
        HideExitPanel();
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene("GamePlay");
    }

    public void LoadInfoScene()
    {
        SceneManager.LoadScene("Info");
    }

    public void LoadLeaderBoardScene()
    {
        DataHolder._sceneNumber = 0;
        SceneManager.LoadScene("LeaderBoard");
    }

    public void ExitFromGame()
    {
        Application.Quit();
    }
}