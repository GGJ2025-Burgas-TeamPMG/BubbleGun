using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public double projLifetime = 5;
    public int damage = 1;
    private double spawnedTime;
    // Start is called before the first frame update
    void Start()
    {
        spawnedTime = Time.timeAsDouble;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.timeAsDouble >= spawnedTime + projLifetime)
        {
            DeleteProjectile();
        }
    }

    public void DeleteProjectile()
    {
        Destroy(transform.parent.gameObject);
    }
}
