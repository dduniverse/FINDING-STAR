using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float speed;
    private Vector3 moveDirection = Vector3.zero;

    public float distance;
    public LayerMask isLayer;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyBullet", 2);
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D ray = Physics2D.Raycast(transform.position, transform.right, distance, isLayer);
        if (ray.collider != null)
        {
            if (ray.collider.tag == "Enemy")
            {
                Debug.Log("ИэСп");
            }
            DestroyBullet();


        }

        transform.position += moveDirection * speed * Time.deltaTime;
        //transform.Translate(transform.forward * speed * Time.deltaTime);

    }

    public void setDirection(Vector3 dir)
    {
        moveDirection = dir;
    }

    void DestroyBullet()
    {
        Destroy(gameObject);
    }
}