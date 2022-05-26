using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    // Start is called before the first frame update
    [HideInInspector]
    public bool bFaceRight = true; //控制人物朝向
    [HideInInspector]
    public bool jump = false; //判断人物跳起
    public float jumpforce = 1000f;
    
    private bool grounded = false;
    private Transform groundcheck;
    
    public float MoveForce = 100.0f; //此变量设置刚体力大小
    public float MaxSpeed = 5f; //角色最大移动速度
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
        //转身功能
        if (h > 0 && !bFaceRight)
        {
            flip();
        }
        else if (h < 0 && bFaceRight)
        {
            flip();
        }
        //控制移动
        if (h * rigidBody.velocity.x < MaxSpeed)
        {
            rigidBody.AddForce(Vector2.right * h * MoveForce);
        }
        //限制最大速度
        if (Mathf.Abs(rigidBody.velocity.x) > MaxSpeed)
        {
            rigidBody.velocity = new Vector2(Mathf.Sign(rigidBody.velocity.x) * MaxSpeed, rigidBody.velocity.y);
        }
        anim.SetFloat("speed", Mathf.Abs(h));
        //设置起跳
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
