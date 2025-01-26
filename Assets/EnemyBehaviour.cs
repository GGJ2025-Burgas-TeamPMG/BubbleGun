using System.Collections;
using System.Collections.Generic;
using System.Data;
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
    public int health = 3;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ShouldDie();
    }

    public void Move()
    {
        // Called from ShouldMoveForward.cs
        if(this.facing == Facing.Left)
        {
            transform.position += Vector3.left * moveSpeed * Time.deltaTime;
        }
        else
        {
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;
        }
    }

    [ContextMenu("Die")]
    public void ShouldDie()
    {
        if(health<=0)
        {
            var bursts = GetComponentsInChildren<SoapBurst>();
            foreach(var b in bursts)
                b.BurstIntoBubbles(false);
            Destroy(transform.parent.gameObject);
        }

    }
}
