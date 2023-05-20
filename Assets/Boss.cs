using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    Animator anim;
    public GameObject Fire;
    public GameObject Missile;
    public int speed;
    public float distance;
    public LayerMask isLayer;
    bool check = true;
    //bool key = true;
    int layerMask = 1 << 10;
    private IEnumerator coroutine;
    int a;
    int angle;
    Transform playerPos;
    private SpriteRenderer rend;
    int b, c;
    bool look = true;
    bool MissileCheck = true;
    Rigidbody2D rigid;
    bool move = true;
    int speedsave;
    Vector3 direction;
    float velocity;
    float accelaration;
    bool x = false;
    public int hp;
    SpriteRenderer spriteRenderer;
    bool isjump;
    //int nextmove;

    //++HP 바
    public string enemyName;
    public int maxHp;
    public int nowHp;
    public int atkDmg;
    public float atkSpeed;
    public float moveSpeed;
    public float atkRange;
    public float fieldOfVision;

    public GameObject UIGameClear;

    private void SetEnemyStatus(string _enemyName, int _maxHp, int _atkDmg, float _atkSpeed, float _moveSpeed, float _atkRange, float _fieldOfVision)
    {
        enemyName = _enemyName;
        maxHp = _maxHp;
        nowHp = _maxHp;
        atkDmg = _atkDmg;
        atkSpeed = _atkSpeed;
        moveSpeed = _moveSpeed;
        atkRange = _atkRange;
        fieldOfVision = _fieldOfVision;
    }

    public GameObject prfHpBar;
    public GameObject canvas;
    RectTransform hpBar;
    Image nowHpbar;
    public float height = 1.7f;


    void Start()
    {
        //StartCoroutine(Bullet());
        speedsave = -speed;
        rend = this.GetComponent<SpriteRenderer>();
        StartCoroutine(Think());
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        //++ 추가 작업
        hpBar = Instantiate(prfHpBar, canvas.transform).GetComponent<RectTransform>();
        if (name.Equals("Boss"))
        {
            SetEnemyStatus("Boss", 100, 10, 1.5f, 2, 1.5f, 7f);
        }
        nowHpbar = hpBar.transform.GetChild(0).GetComponent<Image>();

    }


    void FixedUpdate()
    {
        RaycastHit2D raycast = Physics2D.CircleCast(transform.position, 1f, transform.right, distance, layerMask);
        RaycastHit2D raycast2 = Physics2D.CircleCast(transform.position, 1f, transform.right * -1, distance, layerMask);

        if (move == false)
        {
            Vector3 getVel = new Vector3(1, 0, 0) * speed;
            rigid.velocity = getVel;
        }

        if (look)
        {
            playerPos = GameObject.Find("Player").GetComponent<Transform>();
            if (this.transform.position.x < playerPos.position.x)
            {
                rend.flipX = true;
                if (move)
                {
                    speed = speedsave;
                    if (speedsave < 0)
                        speedsave = -speedsave;
                    move = false;
                }
            }

            if (this.transform.position.x > playerPos.position.x)
            {
                rend.flipX = false;
                if (move)
                {
                    speed = speedsave;
                    if (speedsave > 0)
                        speedsave = -speedsave;
                    move = false;
                }
            }
        }
    }

    void Update()
    {

        if (Mathf.Abs(rigid.velocity.x) < 0.15)
            anim.SetBool("isWalk", false);
        else
            anim.SetBool("isWalk", true);

        if (x)
        {
            BossRush();
        }
        if (isjump)
        {
            rigid.AddForce(new Vector3(0, 1.0f, 0) * 200);
        }
        else
            rigid.AddForce(new Vector3(0, -1.0f, 0) * 200);

        //추가
        Vector3 _hpBarPos = Camera.main.WorldToScreenPoint
            (new Vector3(transform.position.x, transform.position.y + height, 0));
        hpBar.position = _hpBarPos;
        nowHpbar.fillAmount = (float)nowHp / (float)maxHp;
    }

    // 보스 패턴
    IEnumerator Think()
    {
        //speed = speedsave;
        move = true;
        yield return new WaitForSeconds(4.0f);
        speed = 0;
        int Action = Random.Range(0, 4);
        
        switch (Action)
        {
            case 0:
                StartCoroutine(BossFire());
                anim.SetBool("isFire", true);
                break;
            case 1:
                StartCoroutine(BossMissile());

                break; 
            case 2:
                playerPos = GameObject.Find("Player").transform;
                StartCoroutine(BossRushs());

                break;
            case 3:
                StartCoroutine(BossJump());
                break;
        }

    }

    IEnumerator BossFire()
    {
        if (this.transform.position.x > playerPos.position.x)
        {
            b = 160;
            c = 190;
            check = true;
            look = false;
            StartCoroutine(BossFireBullet());
            Invoke("Timer", 3f);
        }

        if (this.transform.position.x < playerPos.position.x)
        {
            b = -10;
            c = 20;
            check = true;
            look = false;
            StartCoroutine(BossFireBullet());
            Invoke("Timer", 3f);
        }
        yield return new WaitForSeconds(4.0f);
        StartCoroutine(Think());
    }



    public void BossRush() //이거
    {

        // Player의 현재 위치를 받아오는 Object

        // Player의 위치와 이 객체의 위치를 빼고 단위 벡터화 한다.
        direction = (playerPos.position - transform.position).normalized;
        // 가속도 지정 (추후 힘과 질량, 거리 등 계산해서 수정할 것)
        accelaration = 0.1f;
        // 초가 아닌 한 프레임으로 가속도 계산하여 속도 증가
        velocity = (accelaration * 1f);
        // Player와 객체 간의 거리 계산
        float distance = Vector3.Distance(playerPos.position, transform.position);
        // 일정거리 안에 있을 시, 해당 방향으로 무빙
        if (distance <= 50.0f)
        {
            /*this.transform.position = new Vector3(transform.position.x + (direction.x * velocity),
                                                   transform.position.y + (direction.y * velocity),
                                                     transform.position.z);*/
            this.transform.position = new Vector3(transform.position.x + (direction.x * velocity),
                                                   transform.position.y,
                                                     transform.position.z);
        }
        // 일정거리 밖에 있을 시, 속도 초기화 
        else
        {
            velocity = 0.0f;
        }
    }

    IEnumerator BossRushs()
    {
        anim.SetBool("isTurtle", true);
        yield return new WaitForSeconds(2.0f);
        anim.SetBool("isTurtling", true);
        look = false;
        x = true;
        yield return new WaitForSeconds(3.0f);
        anim.SetBool("isTurtling", false);
        x = false;
        look = true;
        yield return new WaitForSeconds(0.2f);
        anim.SetBool("isTurtle", false);
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(Think());

    }

    IEnumerator BossMissile()
    {
        if (MissileCheck)
        {
            anim.SetBool("isPunch", true);
            Instantiate(Missile, transform.position, transform.rotation);
            MissileCheck = false;
            StartCoroutine(MissileTimer());
            yield return new WaitForSeconds(1.0f);
            anim.SetBool("isPunch", false);
            yield return new WaitForSeconds(2.0f);
            StartCoroutine(Think());
        }
        else
            StartCoroutine(Think());
    }



    IEnumerator BossJump()
    {
        isjump = true;
        StartCoroutine(Think());
        yield return new WaitForSeconds(1.3f);
        isjump = false;
        //rigid.AddForce(new Vector3(0,- 1.0f, 0) * 150);

    }



    // 기타 등

    IEnumerator BossFireBullet()
    {
        if (check)
        {
            a = UnityEngine.Random.Range(b, c);
            Quaternion qRotation = Quaternion.Euler(0f, 0f, a);
            yield return new WaitForSeconds(0.2f);
            Instantiate(Fire, transform.position, qRotation);
            StartCoroutine(BossFireBullet());
        }
    }

    void Timer()
    {
        check = false;
        look = true;
        anim.SetBool("isFire", false);
    }

    void Timer2()
    {
        StartCoroutine(Think());
        x = false;
        anim.SetBool("isTurtling", false);
    }


    //미사일 사라지는 시간과 같게해야함
    IEnumerator MissileTimer()
    {
        yield return new WaitForSeconds(20.0f);
        MissileCheck = true;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "bullet")
        {
            nowHp = nowHp - 5;
            OnDamaged(collision.transform.position);
            if (nowHp == 0)
                Die();
        }
    }
    void OnDamaged(Vector2 targetPos)
    {
        //change layer
        gameObject.layer = 11;
        //view Alpha
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        //reaction force 튕겨나가는 힘
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(dirc, 1) * 10, ForceMode2D.Impulse);

        Invoke("OffDamaged", 2);
        //Animation
        anim.SetTrigger("doDamaged");
        Invoke("OffDamaged", 3);
    }
    void OffDamaged()
    {
        gameObject.layer = 9;
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }

    //추가
    void Die()
    {
        GetComponent<Collider2D>().enabled = false;
        Destroy(GetComponent<Rigidbody2D>());
        Destroy(gameObject, 3);
        Destroy(hpBar.gameObject, 3);
        GameObject.Find("Canvas").transform.Find("GameClear").gameObject.SetActive(true);
    }
}

