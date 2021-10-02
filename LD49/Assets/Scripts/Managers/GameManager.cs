using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private float startLevelDelay = 1f;
    void Start()
    {
        UIFullscreenFade.main.FadeIn(delegate
        {
            Invoke("StartLevel", startLevelDelay);
        });
    }
    
    public void StartLevel() {
        GenerateRoad.main.Begin();
        UIFullscreenFade.main.FadeOut(delegate
        {
            CameraManager.main.SetUp();
        });
    }

}
