using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShouldMoveForward : MonoBehaviour
{
    private Platform platformScript;
    private EnemyBehaviour parentScript;
    // Start is called before the first frame update
    void Start()
    {
        platformScript = GetComponent<Platform>();
        parentScript = transform.parent.GetComponent<EnemyBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        //platformScript = GetComponent<Platform>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        platformScript = collision.gameObject.GetComponent<Platform>();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (platformScript != null)
        {
            parentScript.Move();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        platformScript = null;
        transform.parent.Rotate(0, 180, 0);
        if(parentScript.facing == EnemyBehaviour.Facing.Left)
        {
            parentScript.facing = EnemyBehaviour.Facing.Right;
        }
        else
        {
            parentScript.facing = EnemyBehaviour.Facing.Left;
        }

    }
}
