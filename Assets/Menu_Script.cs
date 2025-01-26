using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu_Script : MonoBehaviour
{
    // void NextLevel(){
    //     SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    // }


    void LevelSelect(){
        SceneManager.LoadScene(1);
    }

    void Quit(){
        //Application.Quit();
        EditorApplication.Exit(0);
    }
}
