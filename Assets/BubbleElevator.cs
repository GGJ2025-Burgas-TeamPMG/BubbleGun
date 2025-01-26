using DG.Tweening;
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

    Color originalColor;
    public void OnBalloonPooped()
    {
        var ft = GameManager.player.GetComponent<FollowTarget>();
        Destroy(ft);
        transform.DOLocalMove(Vector3.up * 1, 1, false).SetRelative();
        var sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
        sr.DOFade(0, 1).SetEase(Ease.OutCirc);

        GameManager.player.rb.velocity = releaseVelocity;
        GameManager.player.ResetControls(resetAerialControl: true);

        StartCoroutine(Restart());
    }

    public IEnumerator Restart()
    {
        yield return new WaitForSeconds(1);
        transform.position = GetComponent<DoTweenPathFollow>().initialPos;
        var sr = GetComponent<SpriteRenderer>();
        sr.DOFade(1, 1).SetEase(Ease.OutCirc);
        GetComponent<Collider2D>().enabled = true;
    }
}
