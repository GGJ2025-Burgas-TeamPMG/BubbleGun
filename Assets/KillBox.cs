using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class KillBox : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == GameManager.player.gameObject)
        {
            var sb = collision.GetComponent<SoapBurst>();
        sb?.BurstIntoBubbles(false);
        sb?.GetComponent<SpriteRenderer>()?.DOFade(0, 1);

        var pl = collision.GetComponent<Player>();
        if (pl != null) GameManager.Instance.KillPlayer();
        }
    }
}
