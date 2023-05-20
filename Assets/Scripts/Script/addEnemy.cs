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
		// Player�� ���� ��ġ�� �޾ƿ��� Object
		target = GameObject.Find("Player").transform;
		// Player�� ��ġ�� �� ��ü�� ��ġ�� ���� ���� ����ȭ �Ѵ�.
		direction = (target.position - transform.position).normalized;
		// ���ӵ� ���� (���� ���� ����, �Ÿ� �� ����ؼ� ������ ��)
		accelaration = 0.1f;
		// �ʰ� �ƴ� �� ���������� ���ӵ� ����Ͽ� �ӵ� ����
		velocity = (velocity + accelaration *0.5f * Time.deltaTime);
		// Player�� ��ü ���� �Ÿ� ���
		float distance = Vector3.Distance(target.position, transform.position);
		// �����Ÿ� �ȿ� ���� ��, �ش� �������� ����
		if (distance <= 7.5f)
		{
			this.transform.position = new Vector3(transform.position.x + (direction.x * velocity),
												   transform.position.y + (direction.y * velocity),
													 transform.position.z);
		}
		// �����Ÿ� �ۿ� ���� ��, �ӵ� �ʱ�ȭ 
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


    //��� �Լ� ������ ����ؾ� ��
    void Think()
    {
        //Set Next Active
        nextMove = Random.Range(-1, 2);
        // Sprite Animation
        anim.SetInteger("WalkSpeed", nextMove);
        // ����
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