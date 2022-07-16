using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// movetime 이 rotatetime보다 짧으면 버벅 거리는현상이 있음 
public class BossMissile : MonoBehaviour
{
    Transform playerPos;
    Vector2 dir;//, bullet;
    float angle;
    bool check = true;
    bool check2 = false;
    public float speed;
    Rigidbody2D rb;
    public int rotateSpeed;
    Transform target;
    public float movetime;
    public float rotatetime;
    // Start is called before the first frame update

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Invoke("Destroy", 20f);
        target = GameObject.Find("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (target != null && check)
        {
            Vector2 direction = new Vector2(
                target.position.x - transform.position.x,
                target.position.y - transform.position.y
            );
            angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;//rad 를 deg로
            Quaternion angleAxis = Quaternion.AngleAxis(angle - 180f, Vector3.forward);
            Quaternion rotation = Quaternion.Slerp(transform.rotation, angleAxis, rotateSpeed * Time.deltaTime);
            transform.rotation = rotation;
            StartCoroutine(Timer());
        }
        if (check2)
        {
            check2 = false;
            StartCoroutine(Move());

        }
    }

    /*private float getAngle()
    {
        playerPos = GameObject.Find("Player").GetComponent<Transform>(); //transform 위치, getcomponent가 가져오는거
        dir = (playerPos.position - transform.position).normalized;
        return Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;// Atan2 x증가량 / y증가량 넣으면 각도 반환
    }*/

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(rotatetime);
        check = false;
        check2 = true;
        StopCoroutine(Timer());
    }

    IEnumerator Move()
    {
        rb.velocity = -transform.right * speed;
        yield return new WaitForSeconds(movetime);

        rb.velocity = -transform.right * 0;
        check = true;
        StopCoroutine(Move());
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

}
