using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;

public class LeaderBoard : MonoBehaviour
{
    [SerializeField] GameObject parent;
    [SerializeField] GameObject rowLeaderBoardPrefab;
    [SerializeField] GameObject titleRowLeaderBoardPrefab;
   

    private void Start()
    {
        Instantiate(titleRowLeaderBoardPrefab, parent.transform);
        
#if UNITY_ANDROID && !UNITY_EDITOR
        string path = Path.Combine(Application.streamingAssetsPath, + "/LeaderBoard.csv");
        WWW reader = new WWW(path);
        while(!reader.isDone) { }
        StreamReader strReader = new StreamReader(path);
#endif
#if UNITY_EDITOR
        StreamReader strReader = new StreamReader(Application.streamingAssetsPath + "/LeaderBoard.csv");
#endif

        //загружаем строки в нужном количестве
        float result = 0;
        Resolution[] resolutions = Screen.resolutions;

        foreach (var res in resolutions)
        {
            result += (float)res.width / (float)res.height;
           
        }
       

        if(result > 1.5f) //смотрим смартфон или планшет (16:9 или 5:4) 1.5 приблизительная величина
        {
            for(int i = 0; i < 9; i++) // 9 комфортно входит при портретнои ориентации смартфона
            {
                string dataString = strReader.ReadLine();
                var dataValues = (dataString.Split(';'));


                if (PlayerPrefs.HasKey("Match3_Score") && i == 0)
                {
                    var go = Instantiate(rowLeaderBoardPrefab, parent.transform);
                    go.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "Mach3";
                    go.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text = PlayerPrefs.GetString("Match3_Time");
                    go.transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = PlayerPrefs.GetInt("Match3_Score").ToString();
                    
                }
                else
                {
                    var go = Instantiate(rowLeaderBoardPrefab, parent.transform);
                    go.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = dataValues[0];
                    go.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text = dataValues[1];
                    go.transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = dataValues[2];
                }

                
            }
            
        }
        else
        {
            for (int i = 0; i < 5; i++) // 5 комфортно входит при использовании планшета
            {
                Instantiate(rowLeaderBoardPrefab, parent.transform);
            }
        }

    }
    
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Scene_0");
    }
   
}
