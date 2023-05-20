using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemyshooting : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject bullet;
    public float distance;
    public LayerMask isLayer;
    bool check = true;
    int layerMask = 1 << 10;

    void Start()
    {
        //StartCoroutine(Bullet());
    }
    void Update()
    {
        RaycastHit2D raycast = Physics2D.CircleCast(transform.position, 1f, transform.right, distance, layerMask);
        RaycastHit2D raycast2 = Physics2D.CircleCast(transform.position, 1f, transform.right * -1, distance, layerMask);

        if (raycast.collider != null && check)
        {
            StartCoroutine(Bullet());
            check = false;
        }
        if (raycast2.collider != null && check)
        {
            StartCoroutine(Bullet());
            check = false;
        }

    }

    // Update is called once per frame
    IEnumerator Bullet()
    {
        Instantiate(bullet, transform.position, transform.rotation);
        yield return new WaitForSeconds(3.0f);
        check = true;
        //StartCoroutine(Bullet());

    }
}
