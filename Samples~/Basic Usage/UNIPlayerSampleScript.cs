using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UNIHper;
using UniRx;

public class UNIPlayerSampleScript : SceneScriptBase
{
    // Called once after scene is loaded
    private void Start() { }

    // Called per frame after Start
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Managements.Config.Get<UNIPlayer>().PlayVideo("v1");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Managements.Config.Get<UNIPlayer>().PlayVideo("v2");
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            Managements.Config.Get<UNIPlayer>().Back2Idle();
        }
    }

    // Called when scene is unloaded
    private void OnDestroy() { }

    // Called when application is quit
    private void OnApplicationQuit() { }
}
