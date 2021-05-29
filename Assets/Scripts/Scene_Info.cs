using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Info : MonoBehaviour
{
    private const string INSTAGRAMM_URL = "https://www.instagram.com/foranj_farm_games/";
    public void LoadMainScene()
    {
        SceneManager.LoadScene("Scene_0");
    }

    public void LoadInstagramm()
    {
        Application.OpenURL(INSTAGRAMM_URL);
    }
}
