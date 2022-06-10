using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    // Start is called before the first frame update
    [HideInInspector]
    public bool bFaceRight = true; //�������ﳯ��
    [HideInInspector]
    public bool jump = false; //�ж���������
    public float jumpforce = 1000f;
    
    private bool grounded = false;
    private Transform groundcheck;
    
    public float MoveForce = 100.0f; //�˱������ø�������С
    public float MaxSpeed = 5f; //��ɫ����ƶ��ٶ�
    private Animator anim;
    void Start()
    {
        groundcheck = transform.Find("groundcheck");
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics2D.Linecast(transform.position, groundcheck.position, 1 << LayerMask.NameToLayer("Ground"));
        Debug.DrawLine(transform.position, groundcheck.position,Color.red, 1f);
        if (Input.GetButtonDown("Jump") && grounded)
            jump = true;
    }
    void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");
        Rigidbody2D rigidBody = GetComponent<Rigidbody2D>();
        //ת����
        if (h > 0 && !bFaceRight)
        {
            flip();
        }
        else if (h < 0 && bFaceRight)
        {
            flip();
        }
        //�����ƶ�
        if (h * rigidBody.velocity.x < MaxSpeed)
        {
            rigidBody.AddForce(Vector2.right * h * MoveForce);
        }
        //��������ٶ�
        if (Mathf.Abs(rigidBody.velocity.x) > MaxSpeed)
        {
            rigidBody.velocity = new Vector2(Mathf.Sign(rigidBody.velocity.x) * MaxSpeed, rigidBody.velocity.y);
        }
        anim.SetFloat("speed", Mathf.Abs(h));
        //��������
        if (jump)
        {
            rigidBody.AddForce(new Vector2(0f, jumpforce));
                jump = false;
            anim.SetTrigger("jump");
        }
    }
    void flip()
    {
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
        bFaceRight = !bFaceRight;
    }
}
