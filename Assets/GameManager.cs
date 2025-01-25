using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

   
}
