using System;
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
    public TextAsset csvFile; 
    private char lineSeperater = '\n'; 
    private char fieldSeperator = ';'; 
    string[,] boards = new string[9,3];
   

    private void Start()
    {
        Instantiate(titleRowLeaderBoardPrefab, parent.transform);
        string[] records = csvFile.text.Split(lineSeperater);
        string[] fields;        
        
        for (int i = 3, j=0; i < 12; i++, j++) // 9 комфортно входит при портретнои ориентации смартфона
        {
            fields = records[j].Split(fieldSeperator);
            if (PlayerPrefs.HasKey($"Match{i}__Score"))
            {
                boards[j, 0] = $"Mach{i}";
                boards[j, 1] = PlayerPrefs.GetString($"Match{i}_Time");
                boards[j, 2] = PlayerPrefs.GetInt($"Match{i}__Score").ToString();
            }
            else
            {
                boards[j, 0] = fields[0];
                boards[j, 1] = fields[1];
                boards[j, 2] = fields[2];                
            }                       

        }       
        SortMassive();        
    }
    void SortMassive()
    {
        int x;
        string[,] temp = new string[1,3];
        for (int i = 0; i < 9; i++)
        {
            x = i;
            for(int j = i + 1; j < 9; j++)
            {
                if (Int32.Parse(boards[x, 2]) >= Int32.Parse(boards[j, 2]))
                {
                    
                }
                else x = j;

            }
            temp[0, 0] = boards[i,0];
            temp[0, 1] = boards[i, 1];
            temp[0, 2] = boards[i, 2];
            boards[i,0] = boards[x, 0];
            boards[i, 1] = boards[x, 1];
            boards[i, 2] = boards[x, 2];
            boards[x, 0] = temp[0, 0];
            boards[x, 1] = temp[0, 1];
            boards[x, 2] = temp[0, 2];            
                        
        }

        SetBoard();
    }

    void SetBoard()
    {
        for(int j = 0; j < 9; j++)
        {
            var go = Instantiate(rowLeaderBoardPrefab, parent.transform);
            go.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = boards[j, 0];
            go.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text = boards[j, 1];
            go.transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = boards[j, 2];
            if (DataHolder._sceneNumber == 1 && boards[j,0] == "Mach3")
            {
                go.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().color = Color.yellow;
                go.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().color = Color.yellow;
                go.transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().color = Color.yellow;
            }
        }
        
    }

   
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Scene_0");
    }
   
}
