using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public enum Facing
    {
        Left,
        Right
    }

    public Facing facing = Facing.Left;
    public float moveSpeed = 5f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Move()
    {
        //Debug.Log("Moving");
        if(this.facing == Facing.Left)
        {
            transform.position += Vector3.left * moveSpeed * Time.deltaTime;
            Debug.Log("Moving left");
        }
        else
        {
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;
            Debug.Log("Moving right");
        }
    }
}
