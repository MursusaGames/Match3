using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Data;

[System.Serializable]
public class BoardSettings
{
    public int xSize, ySize;
    public Ball ballGO;
    public List<Sprite> ballSprites;
}
public class GamePlay : MonoBehaviour
{
    [Header("Игровая доска")]
    public BoardSettings boardSetting;
    [SerializeField] GameObject exitPanel;
    [SerializeField] Canvas canvas;
    int moves = 3;
    int score = 0;
    int hiScore;
    bool isHiScore = false;
    [SerializeField] Text scoreText;
    [SerializeField] Text movesText;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject firstPanel;



    private void Start()
    {

        firstPanel.SetActive(false);
        HideExitMenu();
        Boards.instance.SetBoard(boardSetting.xSize, boardSetting.ySize, boardSetting.ballGO, boardSetting.ballSprites);
        UpdateMoves();
        UpdateScore();
        if (PlayerPrefs.HasKey("First") == false)
        {
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            firstPanel.SetActive(true);
        }
    }
    public void FirstPanelOff()
    {
        firstPanel.SetActive(false);
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
    }
    void UpdateScore()
    {
        scoreText.text = "Очков: " + score;
    }
    void UpdateMoves()
    {
        movesText.text = "Ходов: " + moves;
    }


    private void Update()
    {
        
        if(moves == 0)
        {
            if (isHiScore)
            {
                SetHiScore();
                return;
            }
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            Boards.instance.isGamePlayed = false;
            gameOverPanel.SetActive(true);
        }
    }
    

    void SetHiScore()
    {

        SceneManager.LoadScene("LeaderBoard");
        
    }
    //добавляем очки

    public void AddScore(int i)
    {
        score += i;
        UpdateScore();
        if (PlayerPrefs.HasKey("Match3_Score"))
        {
            hiScore = PlayerPrefs.GetInt("Match3_Score");
            if(score > hiScore)
            {
                isHiScore = true;
                PlayerPrefs.SetInt("Match3_Score", score);
                PlayerPrefs.SetString("Match3_Time", System.DateTime.Now.ToString());
            }
        }
        else
        {
            isHiScore = true;
            PlayerPrefs.SetInt("Match3_Score", score);
            PlayerPrefs.SetString("Match3_Time", System.DateTime.Now.ToString());
        }
    }
    //добавляем ходы
    public void AddMoves(int i)
    {
        moves += i;
        UpdateMoves();
    }
    //убавляем ход
    public void MinusMoves()
    {
        moves -= 1;
        UpdateMoves();
    }
    #region Buttons
    public void ShowExitPanel()
    {
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        Time.timeScale = 0;
        exitPanel.SetActive(true);
    }
    public void HideExitMenu()
    {
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        Time.timeScale = 1;
        exitPanel.SetActive(false);
    }
    public void ExitGame()
    {
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        Time.timeScale = 1;
        SceneManager.LoadScene("Scene_0");
    }
    #endregion

}
