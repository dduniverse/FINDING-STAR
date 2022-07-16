using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FlyingPattern2 : MonoBehaviour
{
    Rigidbody2D rigid;
    public float left;
    public float right;
    float startpos;
    float endpos;
    public float speed;
    //public float dropspeed;
    public LayerMask isLayer;
    bool check = true;
    //bool fall = true;
    bool changedir = true;
    bool check2 = true;
    public LayerMask floor;
    public GameObject bullet;
    public float createtime;



    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        float firstpos_x = this.transform.position.x;
        float firstpos_y = this.transform.position.y;
        startpos = firstpos_x - left;
        endpos = firstpos_x + right;
        speed = -speed;

    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D ray = Physics2D.Raycast(transform.position, transform.up * -1, 1000, isLayer);
        RaycastHit2D ray2 = Physics2D.Raycast(transform.position, transform.right, 40, floor);
        RaycastHit2D ray3 = Physics2D.Raycast(transform.position, transform.right * -1, 40, floor);

        if (check)
        {
            rigid.velocity = new Vector2(speed, rigid.velocity.y);
            if (this.transform.position.x < startpos && changedir)
            {
                StartCoroutine("ChangeDirection");
                changedir = false;
            }
            if (this.transform.position.x > endpos && changedir)
            {
                StartCoroutine("ChangeDirection");
                changedir = false;
            }
            if (ray2.collider != null && changedir)
            {
                StartCoroutine("ChangeDirection");
                changedir = false;
            }
            if (ray3.collider != null && changedir)
            {
                StartCoroutine("ChangeDirection");
                changedir = false;
            }
            if (check2)
            {
                StartCoroutine(Bullet());
                check2 = false;
            }
        }


    }
    IEnumerator Bullet()
    {
        Instantiate(bullet, transform.position, transform.rotation);
        yield return new WaitForSeconds(createtime);
        check2 = true;
        //StartCoroutine(Bullet());

    }

    IEnumerator ChangeDirection()
    {
        speed = -speed;
        yield return new WaitForSeconds(1.0f);
        changedir = true;
        StopCoroutine("ChangeDirection");
    }

}
