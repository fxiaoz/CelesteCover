using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class PlayerControl : MonoBehaviour
{
    public LayerMask ground;
    
    //Speed
    [SerializeField] public float speed;
    [SerializeField] public float dashSpeed;
    [SerializeField]public float jumpSpeed;
    
    private float _horizontalMove;
    private float _verticalMove;
    private readonly float _fallMultiplier=2.5f;
    private readonly float _lowJumpMultiplier = 2f;
    
    //StateMachine
    public bool ifDash, ifClimb, ifGrounded;

    //Components
    private Rigidbody2D _myBody;
    private Animator _myAnim;
    private SpriteRenderer _myRend;
    private BoxCollider2D _myBox;

    // Start is called before the first frame update
    void Start()
    {
        _myBody = GetComponent<Rigidbody2D>();
        _myAnim = GetComponent<Animator>();
        _myRend = GetComponent<SpriteRenderer>();
        _myBox = GetComponent<BoxCollider2D>();
        ifGrounded = true;
        ifClimb = false;
        ifDash = false;
    }

    // Update is called once per frame
    void Update()
    {
        _horizontalMove = Input.GetAxis("Horizontal");
        _verticalMove = Input.GetAxis("Vertical");
        
        //dash check
        if (Input.GetKeyDown(KeyCode.K)&&!ifGrounded)
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
        ifGrounded = _myBox.IsTouchingLayers(ground);;
        Move();
        Jump();
        Climb();
        Dash();
        CheckState();
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
        if (Input.GetButton("Jump")&&(ifGrounded||ifClimb))
        {
            //jump sound play
            _myBody.AddForce(Vector2.up*jumpSpeed,ForceMode2D.Impulse);
        }
        
        if (_myBody.velocity.y<0)
        {
            _myBody.velocity += Vector2.up * Physics2D.gravity.y * (_fallMultiplier - 1) * Time.deltaTime;
        }
        else if (_myBody.velocity.y>0&&!Input.GetButton("Jump"))
        {
            _myBody.velocity += Vector2.up * Physics2D.gravity.y * (_lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    private void Climb()
    {   
        
    }

    private void Dash()
    {
        if (ifDash)
        {
            Vector2 direction = new Vector2(_horizontalMove,_verticalMove);
            _myBody.AddForce(direction * dashSpeed, ForceMode2D.Impulse);
            ifDash = false;
        }
    }
    
    
}
