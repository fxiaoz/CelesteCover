using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
    //Speed
    public float speed = 1f;
    float horizontalMove;
    float verticalMove;

    public float jumpReady = 1;

    //Gravity
    public float castDist = 0.2f;
    public float gravityScale = 5f;
    public float gravityFall = 40f;
    public float jumpLimit = 1f;

    //Bool
    public bool dash = false;
    public bool jump = false;
    public bool climb = false;

    public bool grounded = true;
    public bool wallDetected = false;

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

        verticalMove = Input.GetAxis("Vertical");

        //if (grounded)
        //{
        //    jumpReady = 1;
        //}

        if (Input.GetKeyDown(KeyCode.J))
        {
            if (grounded)
            {
                jump = true;
                //jumpReady --;
                //jump sound play
            }
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            dash = true;
            //jump sound play
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            if (climb)
            {
                climb = false;
            }
            else
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
        //First check for position

        float moveHoriSpeed = horizontalMove * speed;
        float moveVerSpeed = verticalMove * speed;

        if(!climb)
        {
            myBody.velocity = new Vector3(moveHoriSpeed, myBody.velocity.y, 0);
        }
        if (climb)
        {
            myBody.velocity = new Vector3(myBody.velocity.x, moveVerSpeed, 0);
        }

        //Jump

        if (jump)
        {
            myBody.AddForce(Vector2.up * jumpLimit, ForceMode2D.Impulse);
            jump = false;

            Debug.Log("jump");
        }

        //Dash

        if (dash)
        {

        }

        //Gravity

        if (climb)
        {
            myBody.gravityScale = 0;
        }
        else if (myBody.velocity.y > 0)
        {
            myBody.gravityScale = gravityScale;
        }
        else if (myBody.velocity.y < 0)
        {
            myBody.gravityScale = gravityFall;
        }

        //Raycasting for grounding.

        int layermaskG = LayerMask.GetMask("Ground");

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, castDist, layermaskG);

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

        //Raycasting for climbing.

        int layermaskW = LayerMask.GetMask("Ground");

        RaycastHit2D Clim = Physics2D.Raycast(transform.position, Vector2.right, castDist, layermaskW);

        Debug.DrawRay(transform.position, Vector2.right * castDist, Color.red);

        if (Clim.transform != null)
        {
            Debug.Log(hit.transform.name);
        }

        if (Clim.collider != null && Clim.transform.name == "obj_wall")
        {
            wallDetected = true;
            Debug.Log("Detected");
        }
        else
        {
            wallDetected = false;
        }

        //Last check for the position

        if (!climb)
        {
            myBody.velocity = new Vector3(moveHoriSpeed, myBody.velocity.y, 0);
        }
        if (climb)
        {
            myBody.velocity = new Vector3(myBody.velocity.x, moveVerSpeed, 0);
        }
    }

    private void OnCollisionEnter2D(Collision2D Collision)
    {
        //collision with lava
    }
}
