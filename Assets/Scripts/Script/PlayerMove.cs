using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    //sounds
    public AudioClip audioJump;
    public AudioClip audioAttack;
    public AudioClip audioDamaged;
    public AudioClip audioItem;
    public AudioClip audioDie;
    public AudioClip audioFinish;


    public GameManager gameManager;
    public float maxSpeed;
    public float jumpPower;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void Awake()
    {

        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }


    void PlaySound(string action)
    {
        switch (action)
        {
            case "JUMP":
                audioSource.clip = audioJump;
                break;
            case "ATTACK":
                audioSource.clip = audioAttack;
                break;
            case "DAMAGED":
                audioSource.clip = audioDamaged;
                break;
            case "ITEM":
                audioSource.clip = audioItem;
                break;
            case "DIE":
                audioSource.clip = audioDie;
                break;
            case "FINISH":
                audioSource.clip = audioFinish;
                break;
        }
    }
    void Update()
    {
        //Stop Speed
        if (Input.GetButtonUp("Horizontal"))
        {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
        }

        //Direction Sprite
        if (Input.GetButton("Horizontal"))
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;

        if (Mathf.Abs(rigid.velocity.x) > 0.2)
        {
            anim.SetBool("isWalking", true);

        }

        else
        {
            anim.SetBool("isWalking", false);

        }

        if (Input.GetKeyDown(KeyCode.Space) && anim.GetBool("isWalking"))
        {
            anim.SetBool("isWattacking", true);
            anim.SetBool("isWalking", true);
            PlaySound("ATTACK");
            audioSource.Play();

        }
        else if (Input.GetKeyUp(KeyCode.Space) && anim.GetBool("isWalking"))
        {
            anim.SetBool("isWattacking", false);
            anim.SetBool("isWalking", true);
        }


        //Jump
        if (Input.GetKeyDown(KeyCode.UpArrow) && !anim.GetBool("isJump"))
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            anim.SetBool("isJump", true);
            PlaySound("JUMP");
            audioSource.Play();
        }

        //Attack
        if (Input.GetKeyDown(KeyCode.Space) && !anim.GetBool("isAttacking"))
        {
            anim.SetBool("isAttacking", true);
            PlaySound("ATTACK");
            audioSource.Play();

        }
        else
        {
            anim.SetBool("isAttacking", false);
        }


    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Move Speed
        float h = Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right * h * 10, ForceMode2D.Impulse); // h * 숫자 경사로 올라갈때 가해줄수있는 힘

        //Max Speed
        if (rigid.velocity.x > maxSpeed) // 오른쪽 최대 스피드
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        else if (rigid.velocity.x < maxSpeed * (-1)) // 왼쪽 최대 스피드
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);
        //Landing Platform
        if (rigid.velocity.y < 0)
        {
            Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));
            if (rayHit.collider != null)
            {
                if (rayHit.distance < 0.5f)
                    anim.SetBool("isJump", false);
            }
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            //Attack
            if (rigid.velocity.y < 0 && transform.position.y > collision.transform.position.y)
            {
                OnAttack(collision.transform);
                PlaySound("ATTACK");
                audioSource.Play();
                gameManager.HealthDown();   /// 버섯 부딪혔을 떄 라이프 -1
            }
            else // Damaged
            {
                OnDamaged(collision.transform.position);
                PlaySound("DAMAGED");
                audioSource.Play();
            };
        }

    }
    void OnAttack(Transform enemy)
    {
        // Point

        //Reaction
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
        //Enemy Die
        EnemyMove enemyMove = enemy.GetComponent<EnemyMove>();
        enemyMove.OnDamaged();
    }
    void OnDamaged(Vector2 targetPos)
    {
        //Health Down
        gameManager.HealthDown();
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
        gameObject.layer = 10;
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }

    //coin
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Item")
        {
            // point
            bool isCoin = collision.gameObject.name.Contains("Coin");

            if (isCoin)
                gameManager.StagePoint += 100;

            //deactive item
            collision.gameObject.SetActive(false);
            PlaySound("ITEM");
            audioSource.Play();
        }
        else if (collision.gameObject.tag == "Finish")
        {
            //next stage
            gameManager.NextStage();
            PlaySound("FINISH");
            audioSource.Play();
        }
    }
    public void OnDie()
    {
        // Sprite Alpha
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        // Sprite Flip Y
        spriteRenderer.flipY = true;
        // Collider Disable

        // Die Effect Jump
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
        PlaySound("DIE");
        audioSource.Play();
    }

    public void VelocityZero()
    {
        rigid.velocity = Vector2.zero;
    }

}
