using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-20000)]
public class GameManager : MonoBehaviour
{
    public static Player player;
    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        Instance = this;
    }

    void Start()
    {
        // game init
        var ft = Camera.main.GetComponent<SmoothFollow>();
        ft.target = player.transform;
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
