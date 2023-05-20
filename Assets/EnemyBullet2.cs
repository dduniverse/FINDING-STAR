using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet2 : MonoBehaviour
{
    // Start is called before the first frame update
    public int bulletspeed;
    public LayerMask layer;
    Rigidbody2D rigid;

    void Start()
    {
        bulletspeed = -bulletspeed;
        rigid = GetComponent<Rigidbody2D>();
        rigid.velocity = new Vector2(rigid.velocity.x, bulletspeed);
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.rotation = Quaternion.AngleAxis(-90, Vector3.forward);
        RaycastHit2D ray = Physics2D.Raycast(transform.position, transform.right, 0, layer);
        StartCoroutine(Bullet());
        if (ray.collider != null)
        { //collider �浹ü
            Destroy(gameObject);
            if (ray.collider.tag == "Player")
            { // �浹ü�� �÷��̾���
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
