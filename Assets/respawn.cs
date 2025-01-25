using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class respawn : MonoBehaviour
{
    GameObject Player;
    void Collision(Collision collision)
    {
        float x = Player.transform.position.x;
        float y = Player.transform.position.y;
        if (collision.gameObject.name == "Player")
        {
            Player.transform.position = new Vector2(x, y);
        }
    }
}
