using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Scripting;

public class BubbleElevator : MonoBehaviour
{
    public Vector2 releaseVelocity = Vector2.zero;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == GameManager.player.gameObject)
        {
            GetComponent<Collider2D>().enabled = false;
            GetComponentInParent<DoTweenPathFollow>().Follow();

            // Pin player to self
            var pl = GameManager.player;
            pl.aerialControl = 0;

            var ft = pl.AddComponent<FollowTarget>();
            ft.SetTarget(transform, offset: Vector3.zero);
        }
    }

    public void OnBalloonPooped()
    {
        var ft = GameManager.player.GetComponent<FollowTarget>();
        Destroy(ft);
        GameManager.player.rb.velocity = releaseVelocity;
        GameManager.player.ResetControls(resetAerialControl: true);
    }
}
