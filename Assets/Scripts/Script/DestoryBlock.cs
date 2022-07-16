using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryBlock : MonoBehaviour
{

    float destroySec = 0.3f;


    void Start()
    {

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Equals("Player"))
        {

            Destroy(gameObject, destroySec);
        }
    }

    void Update()
    {

    }
}