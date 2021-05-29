using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ExitBtn : MonoBehaviour
{
    
    [SerializeField] Scene_0 scene_0;
   

    

    public void BtnPressed()
    {
        scene_0.ShowExitPanel();
    }
}
