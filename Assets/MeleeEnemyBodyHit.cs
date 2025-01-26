using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyBodyHit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D[] hits = Physics2D.OverlapCapsuleAll(transform.position, transform.GetComponent<CapsuleCollider2D>().size, transform.GetComponent<CapsuleCollider2D>().direction, 0);

        foreach (var hit in hits)
        {
            if(hit.GetComponent<Projectile>() != null)
            {
                OnHit(hit);
            }
        }
    }

    public void OnHit(Collider2D collider)
    {
        transform.GetComponentInParent<EnemyBehaviour>().health -= collider.gameObject.GetComponent<Projectile>().damage;
        collider.transform.GetComponent<Projectile>().DeleteProjectile();
    }
}
