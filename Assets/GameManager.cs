using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;
using UnityEngine.Profiling;
using UnityEngine.SceneManagement;
using Water2DTool;

[DefaultExecutionOrder(-20000)]
public class GameManager : MonoBehaviour
{
    PostProcessingProfile profile;

    public static Player player;
    public static GameManager Instance { get; private set; }
    private void Start()
    {
        Instance = this;

        var cam = GameObject.Find("MainCamera");
        profile = cam.GetComponent<PostProcessingBehaviour>().profile;
        try
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        }
        catch (Exception ex) { };
        
        // Fade
        profile = Instantiate(profile);
        var colorGradingSettings = profile.colorGrading.settings;
        colorGradingSettings.basic.postExposure = -40;
        DOTween.To(() => colorGradingSettings.basic.postExposure, (x) => colorGradingSettings.basic.postExposure = x, 0, 2).SetEase(Ease.InCirc);

        // game init
        var ft = Camera.main.GetComponent<SmoothFollow>();
        ft.target = player.transform;
    }

    bool playerWasKilled = false;
    public void KillPlayer()
    {
        if(playerWasKilled) return;
        playerWasKilled = true;
        StartCoroutine(ReapawnRoutine());
        var colorGradingSettings = profile.colorGrading.settings;
        DOTween.To(() => colorGradingSettings.basic.postExposure, (x) => colorGradingSettings.basic.postExposure = x, -40, 1);
    }

    public IEnumerator ReapawnRoutine()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetSceneAt(0).buildIndex);
    }



    public void LoadLevel(int sceneId)
    {
        SceneManager.LoadScene(sceneId);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
