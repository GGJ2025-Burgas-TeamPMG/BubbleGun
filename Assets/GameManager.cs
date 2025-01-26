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
        try
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        }
        catch (Exception ex) { };

        var cam = GameObject.Find("MainCamera");
        var ppb = cam.GetComponent<PostProcessingBehaviour>();
        profile = Instantiate(ppb.profile);
        ppb.profile = profile;

        // Fade

        var colorGradingSettings = profile.colorGrading.settings;
        profile.colorGrading.settings = colorGradingSettings;

        DOTween.To(
            (x) => {
                var sss = profile.colorGrading.settings;
                sss.basic.postExposure = x;
                profile.colorGrading.settings = sss;
            }, -30f, 0f, 3).SetEase(Ease.OutCirc);

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
        DOTween.To(
            (x) => {
                var sss = profile.colorGrading.settings;
                sss.basic.postExposure = x;
                profile.colorGrading.settings = sss;
            }, 0f, -40f, 2).SetEase(Ease.InCirc);

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
