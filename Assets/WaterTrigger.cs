using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class WaterTrigger : MonoBehaviour
{ 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var sb = collision.GetComponent<SoapBurst>();
        sb?.BurstIntoBubbles(false);
        sb?.GetComponent<SpriteRenderer>()?.DOFade(0, 1);

        var pl = collision.GetComponent<Player>();
        if (pl != null) GameManager.Instance.KillPlayer();
    }
}
