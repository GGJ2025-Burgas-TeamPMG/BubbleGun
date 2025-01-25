using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleElevatorScript : MonoBehaviour
{
    public float floatingSpeed = 1;
    public float floatingTime = 1;
    public Rigidbody2D myRigidbody;
    private bool isPlayerLevitating = false;
    private bool isPlayerOnElevator = false;
    private float timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(this.isPlayerLevitating)
        {
            this.timer += Time.deltaTime;
            //Debug.Log($"{timer:f4}");
            if(this.timer > floatingTime)
            {
                myRigidbody.gravityScale = 1;
                //myRigidbody.velocity = Vector2.zero;
                this.isPlayerLevitating = false;
                this.timer = 0;
                Debug.Log("Player left elevator");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!isPlayerOnElevator)
        {
            Debug.Log("Collided with bubble elevator");
            if(collision.gameObject == myRigidbody.gameObject)
            {
                Debug.Log("Collided with bubble elevator and is the player");
                myRigidbody.gravityScale = 0;
                this.isPlayerLevitating = true;
                myRigidbody.velocity += Vector2.up * floatingSpeed;
                isPlayerOnElevator = true;
            }
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Exited bubble elevator");
        if(collision.gameObject == myRigidbody.gameObject)
        {
            Debug.Log("Exited bubble elevator and is the player");
            isPlayerOnElevator = false;
        }
    }
}
