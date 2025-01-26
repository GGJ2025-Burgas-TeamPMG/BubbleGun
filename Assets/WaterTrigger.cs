using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTrigger : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var pl = collision.GetComponent<SoapBurst>();
        pl?.BurstIntoBubbles(false);
        pl.GetComponent<SpriteRenderer>().DOFade(0, 1);
    }
}
