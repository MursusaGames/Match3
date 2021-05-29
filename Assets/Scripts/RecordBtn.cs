using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordBtn : MonoBehaviour
{
    [SerializeField] Scene_0 scene_0;
    public void LoadRecords()
    {
        scene_0.LoadLeaderBoardScene();
    }
}
