using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class PlayerControl : MonoBehaviour
{
    //Speed
    [SerializeField] public float speed = 1f;
    private float _horizontalMove;
    private float _verticalMove;

    public float jumpReady = 1;

    //Gravity
    [SerializeField] public float castDist = 0.2f;
    public float gravityScale = 5f;
    public float gravityFall = 40f;
    public float jumpSpeed = 1f;

    //StateMachine
    public bool ifDash = false;
    public bool ifJump = false;
    public bool ifClimb = false;

    public bool grounded = true;
    public bool wallDetected = false;

    //Components
    private Rigidbody2D _myBody;
    private Animator _myAnim;
    private SpriteRenderer _myRend;

    // Start is called before the first frame update
    void Start()
    {
        _myBody = GetComponent<Rigidbody2D>();
        _myAnim = GetComponent<Animator>();
        _myRend = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        _horizontalMove = Input.GetAxis("Horizontal");
        _verticalMove = Input.GetAxis("Vertical");
        
        //jump check
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (grounded)
            {
                ifJump = true;
            }
        }

        //dash check
        if (Input.GetKeyDown(KeyCode.K)&&!grounded)
        {
            ifDash = true;
        }

        //climb check
        if (Input.GetKey(KeyCode.L))
        {
            ifClimb = true;
        }
        else
        {
            ifClimb = false;
        }
    }

    void FixedUpdate()
    {
        Move();
        Jump();
        Climb();
        Dash();

        //Gravity

        if (ifClimb)
        {
            _myBody.gravityScale = 0;
        }
        else if (_myBody.velocity.y > 0)
        {
            _myBody.gravityScale = gravityScale;
        }
        else if (_myBody.velocity.y < 0)
        {
            _myBody.gravityScale = gravityFall;
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

        //Raycast for climbing.

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
    }

    private void CheckState()
    {
        
    }
        
    private void Move()
    {
        float moveHorSpeed = _horizontalMove * speed;
        float moveVerSpeed = _verticalMove * speed;

        if(!ifClimb)
        {
            _myBody.velocity = new Vector3(moveHorSpeed, _myBody.velocity.y, 0);
            //walking sound
        }
        if (ifClimb)
        {
            _myBody.velocity = new Vector3(_myBody.velocity.x, moveVerSpeed, 0);
            //climbing sound
        }
    }

    private void Jump()
    {
        if (ifJump)
        {
            //jump sound play
            _myBody.velocity = new Vector2(_myBody.velocity.x,jumpSpeed*Time.fixedDeltaTime*50);
            ifJump = false;
        }
    }

    private void Climb()
    {   
        
    }

    private void Dash()
    {
        if (ifDash)
        {
            Vector2 direction = new Vector2(_horizontalMove * speed,_verticalMove * speed);
            _myBody.AddForce(direction * jumpSpeed, ForceMode2D.Impulse);
            ifDash = false;
        }
    }
}
