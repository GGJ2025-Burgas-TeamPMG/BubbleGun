using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class End_Level : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == GameManager.player.gameObject)
        {
            Debug.Log("End Level");
            SceneManager.LoadScene(1);
        }

    }


}
