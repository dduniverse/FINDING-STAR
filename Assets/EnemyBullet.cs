using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyBullet : MonoBehaviour
{
    Transform playerPos;
    Vector2 dir;//, bullet;
    float angle;
    // 총알이 닿으면 없어질 레이어
    public LayerMask layer; //층
    public float bulletspeed;
    void Start()
    {
        playerPos = GameObject.Find("Player").GetComponent<Transform>(); //transform 위치, getcomponent가 가져오는거
        dir = (playerPos.position - transform.position).normalized;
        GetComponent<Rigidbody2D>().velocity = dir * bulletspeed;
        //bullet = transform.position;
        angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;


        //dir * Time.deltaTime * 2000

    }

    void Update()
    {
        this.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        RaycastHit2D ray = Physics2D.Raycast(transform.position, transform.right, 0, layer);
        StartCoroutine(Bullet());
        if (ray.collider != null)
        { //collider 충돌체
            Destroy(gameObject);
            if (ray.collider.tag == "Player")
            { // 충돌체가 플레이어라면
              //PlayerController player = ray.collider.GetComponent<PlayerController>(); // 
              // player.playerDamaged(transform.position);
              //Schedule<PlayerDeath>();

            }
        }
        IEnumerator Bullet()
        {
            yield return new WaitForSeconds(3.0f);
            Destroy(gameObject);
            //StartCoroutine(Bullet());

        }
    }
}

