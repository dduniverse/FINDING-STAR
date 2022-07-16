using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D myrigidbody;

    [SerializeField]
    private float power;
    private float speed = 1;
    // Start is called before the first frame update
    private void Start()
    {
        myrigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))  //점프 시작
        {
            myrigidbody.velocity = Vector2.up * power;
        }
        
            //캐릭터 움직임 시작
        Vector2 position = transform.position;

        if(Input.GetKey(KeyCode.A))
        {
            position.x += -0.1f * speed * Time.deltaTime;
        }
        transform.position = position;

        if (Input.GetKey(KeyCode.D))
        {
            position.x += 0.1f * speed * Time.deltaTime;
        }
        transform.position = position;

    }

    private void FixedUpdate()
    {
        float hor = Input.GetAxis("Horizontal");
        myrigidbody.velocity = new Vector2(hor * 3, myrigidbody.velocity.y);
    }
}
