using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    public GameObject bullet;
    public Transform pos;
    private SpriteRenderer rend;
    public Quaternion qRotation = Quaternion.Euler(0f, 0f, 0f);
    bool term = true;

    private Vector3 lastMoveDirection = Vector3.zero;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        //float y = Input.GetAxisRaw("Vertical");

        if (x != 0)
        {
            lastMoveDirection = new Vector3(x, 0, 0);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (term)
            {
                term = false;
                GameObject b = Instantiate(bullet, transform.position, qRotation);

                b.GetComponent<Bullet>().setDirection(lastMoveDirection);
                rend = b.GetComponent<SpriteRenderer>();
                Invoke("time", 0.5f);
            }
        }

        if (Input.GetAxis("Horizontal") > 0)
        {
            qRotation = Quaternion.Euler(0f, 0f, 0f);
            lastMoveDirection = Vector3.right;
            //rend.flipX = false;
            //renderer.flipX = x == 1;
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            qRotation = Quaternion.Euler(0f, 0f, 180f);
            lastMoveDirection = Vector3.left;
            //rend.flipX = true;
            //renderer.flipX = x == -1;
        }


    }
    void time()
    {
        term = true;
    }
}