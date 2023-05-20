using UnityEngine;
using System.Collections;

public class addEnemy : MonoBehaviour
{
	public Transform target;
	public Vector3 direction;
	public float velocity;
	public float accelaration;

	Rigidbody2D rigid;
	public int nextMove;
	Animator anim;
	SpriteRenderer spriteRenderer;
	CapsuleCollider2D capsuleCollider;
	// Update is called once per frame
	void Update()
	{
		MoveToTarget();
	}

	public void MoveToTarget()
	{
		// Player의 현재 위치를 받아오는 Object
		target = GameObject.Find("Player").transform;
		// Player의 위치와 이 객체의 위치를 빼고 단위 벡터화 한다.
		direction = (target.position - transform.position).normalized;
		// 가속도 지정 (추후 힘과 질량, 거리 등 계산해서 수정할 것)
		accelaration = 0.1f;
		// 초가 아닌 한 프레임으로 가속도 계산하여 속도 증가
		velocity = (velocity + accelaration *0.5f * Time.deltaTime);
		// Player와 객체 간의 거리 계산
		float distance = Vector3.Distance(target.position, transform.position);
		// 일정거리 안에 있을 시, 해당 방향으로 무빙
		if (distance <= 7.5f)
		{
			this.transform.position = new Vector3(transform.position.x + (direction.x * velocity),
												   transform.position.y + (direction.y * velocity),
													 transform.position.z);
		}
		// 일정거리 밖에 있을 시, 속도 초기화 
		else
		{
			velocity = 0.0f;
		}
	}
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        Invoke("Think", 5);
    }


    void FixedUpdate()
    {
        // Move
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);
        // Platform Check
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.3f, rigid.position.y);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform"));
        if (rayHit.collider == null)
        {
            Turn();
        }
    }


    //재귀 함수 딜레이 사용해야 함
    void Think()
    {
        //Set Next Active
        nextMove = Random.Range(-1, 2);
        // Sprite Animation
        anim.SetInteger("WalkSpeed", nextMove);
        // 방향
        if (nextMove != 0)
            spriteRenderer.flipX = nextMove == 1;



        //Set Next Active
        float nextThinkTime = Random.Range(2f, 5f);
        Invoke("Think", nextThinkTime);
    }
    void Turn()
    {

        nextMove *= -1;
        spriteRenderer.flipX = nextMove == 1;
        CancelInvoke();
        Invoke("Think", 2);
    }
    public void OnDamaged()
    {
        // Sprite Alpha
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        // Sprite Flip Y
        spriteRenderer.flipY = true;
        // Collider Disable
        capsuleCollider.enabled = false;
        // Die Effect Jump
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
        // Destroy
        Invoke("DeActive", 5);
    }
    void DeActive()
    {
        gameObject.SetActive(false);
    }
}