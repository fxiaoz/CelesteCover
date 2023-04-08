using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
    //Stats
    public float speed = 1f;
    float horizontalMove;

    public float castDist = 0.2f;
    public float gravityScale = 5f;
    public float gravityFall = 40f;
    public float jumpLimit = 1f;

    //Bool
    public bool dash = false;
    public bool jump = false;
    public bool climb = true;

    public bool grounded = true;

    //Components
    Rigidbody2D myBody;
    Animator myAnim;
    SpriteRenderer myRend;

    // Start is called before the first frame update
    void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();
        myRend = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.A))
        {

        }
        else if (Input.GetKeyDown(KeyCode.D))
        {

        }

        if (Input.GetKeyDown(KeyCode.W))
        {

        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            //if (grounded)
            //{
                jump = true;
                //jump sound play
            //}
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            dash = true;
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            if (climb)
            {
                climb = false;
            }

            if (!climb)
            {
                climb = true;
            }
        }

        if(horizontalMove > 0.2f)
        {

        }
        else if(horizontalMove < -0.2f)
        {

        }
        else
        {

        }
    }

    void FixedUpdate()
    {
        float moveSpeed = horizontalMove * speed;
        myBody.velocity = new Vector3(moveSpeed, myBody.velocity.y, 0);

        if (jump)
        {
            myBody.AddForce(Vector2.up * jumpLimit, ForceMode2D.Impulse);
            jump = false;

            Debug.Log("jump");
        }

        if (dash)
        {

        }

        if (myBody.velocity.y > 0)
        {
            myBody.gravityScale = gravityScale;
        }
        else if (myBody.velocity.y < 0)
        {
            myBody.gravityScale = gravityFall;
        }

        //Raycasting for grounding.

        //RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.down, castDist);

        //Debug.DrawRay(transform.position, Vector2.down * castDist, Color.red);

        //for (int i = 0; i < hits.Length; i++)
        //{
        //    RaycastHit2D hit = hits[i];

        //    if (hit.collider != null && hit.transform.name == "obj_ground")
        //    {
        //        grounded = true;
        //    }
        //    else
        //    {
        //        grounded = false;
        //    }
        //}

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, castDist);

        Debug.DrawRay(transform.position, Vector2.down * castDist, Color.red);

        if (hit.transform != null)
        {
            Debug.Log(hit.transform.name);
        }

        if (hit.collider != null && hit.transform.name == "obj_ground")
        {
            grounded = true;
            Debug.Log("Grounded");
        }
        else
        {
            grounded = false;
        }

        myBody.velocity = new Vector3(moveSpeed, myBody.velocity.y, 0);
    }

    private void OnCollisionEnter2D(Collision2D Collision)
    {

    }
}
