using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class Platform : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var sr = GetComponent<SpriteRenderer>();
        var bc = GetComponent<BoxCollider2D>();
        bc.size = sr.size;
    }
}
