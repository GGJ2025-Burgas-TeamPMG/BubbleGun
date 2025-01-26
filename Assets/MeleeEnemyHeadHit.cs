using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyHeadHit : MonoBehaviour
{
    public int headHitDmgMultiplier = 2;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, transform.GetComponent<CircleCollider2D>().radius);

        foreach (var hit in hits)
        {
            if (hit.GetComponent<Projectile>() != null)
            {
                OnHit(hit);
            }
        }
    }

    public void OnHit(Collider2D collider)
    {
        transform.GetComponentInParent<EnemyBehaviour>().health -= collider.gameObject.GetComponent<Projectile>().damage * headHitDmgMultiplier;
        collider.transform.GetComponent<Projectile>().DeleteProjectile();
    }
}
