using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using DG.Tweening;

public class Movement : MonoBehaviour
{
    private BoxCollider2D _collision2D;
    private Rigidbody2D _rigidbody2D;

    [SerializeField] public float moveSpeed = 10f;
    [SerializeField] public float jumpForce = 50f;
    [SerializeField] public float dashSpeed = 20f;
    [SerializeField] public float jumpLerp = 10f;
    [SerializeField] public float slideSpeed = 5f;

    private float x, y, xRaw, yRaw;
    
    private bool _canMove, _isGrab, _isWallJumped, _isSlide, _isDash, _onGround, _hasDashed;
    
    public ParticleSystem dashParticle,jumpParticle,wallJumpParticle,slideParticle;

    //character facing (R:1, L:-1)
    private int _side = 1;

    // Start is called before the first frame update
    void Start()
    {
        _collision2D = GetComponent<BoxCollider2D>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _canMove = true;
        _isGrab = false;
        _isDash = false;
        _isSlide = false;
        _isWallJumped = false;
        _onGround = true;
        _hasDashed = false;
    }

    // Update is called once per frame
    void Update()
    {
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");
        xRaw = Input.GetAxisRaw("Horizontal");
        yRaw = Input.GetAxisRaw("Vertical");
        Vector2 direction = new Vector2(x,y);
        
        Walk(direction);
        //TODO walk animation

        StateMachine();
    }

    private void Walk(Vector2 direction)
    {
        if (!_canMove||_isGrab)
        {
            return;
        }
        
        if (!_isWallJumped)
        {
            _rigidbody2D.velocity = new Vector2(direction.x * moveSpeed, _rigidbody2D.velocity.y);
        }
        else
        {
            var velocity = _rigidbody2D.velocity;
            _rigidbody2D.velocity = Vector2.Lerp(velocity, (new Vector2(direction.x * moveSpeed, velocity.y)), jumpLerp * Time.deltaTime);
        }
    }

    private void StateMachine()
    {
        if (CollisionCheck.onWall&&Input.GetKey(KeyCode.L)&&_canMove)
        {
            if (_side!=CollisionCheck.wallSide)
            {
                //TODO Flip(side*-1)
            }

            _isGrab = true;
            _isSlide = false;
        }

        if (Input.GetKey(KeyCode.L) || !CollisionCheck.onWall || !_canMove)
        {
            _isGrab = false;
            _isSlide = false;
        }

        if (CollisionCheck.onGround && !_isDash)
        {
            _isWallJumped = false;
            GetComponent<Falloptimization>().enabled = true;
        }

        if (_isGrab&&!_isDash)
        {
            _rigidbody2D.gravityScale = 0;
            if (x> 0.2f||x<-0.2f)
            {
                _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, 0);
            }
            float speedModifier = y > 0 ? 0.5f : 1;
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, y * (moveSpeed * speedModifier));
        }
        else
        {
            _rigidbody2D.gravityScale = 3;
        }

        if (CollisionCheck.onWall&&!CollisionCheck.onGround)
        {
            if (x!=0&&!_isGrab)
            {
                _isSlide = true;
                Sliding();
            }
        }

        if (!CollisionCheck.onWall||CollisionCheck.onGround)
        {
            _isSlide = false;
        }

        if (Input.GetButton("Jump"))
        {
            //todo jump animation
            if (CollisionCheck.onGround)
            {
                Jump(Vector2.up,false);
            }
            if (CollisionCheck.onWall&&!CollisionCheck.onGround)
            {
                WallJump();
            }
        }

        if (Input.GetKey(KeyCode.K)&&!_hasDashed)
        {
            if (xRaw!=0||yRaw!=0)
            {
                Dash(xRaw,yRaw);
            }
        }

        if (CollisionCheck.onGround&&!_onGround)
        {
            Landing();
            _onGround = true;
        }

        if (!CollisionCheck.onGround&&_onGround)
        {
            _onGround = false;
        }
        
        //todo wall particle effect

        //如果爬墙、滑墙、定格状态则跳过翻转
        if (_isGrab||_isSlide||!_canMove)
        {
            return;
        }

        if (x>0)
        {
            _side = 1;
            //todo anime flip
        }

        if (x<0)
        {
            _side = -1;
            //todo anime flip
        }
    }

    private void Jump(Vector2 direction,bool wall)
    {
        //slideParticle.transform.parent.localScale = new Vector3(ParticleSide(), 1, 1);
        //ParticleSystem particle = wall ? wallJumpParticle : jumpParticle;

        _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, 0);
        _rigidbody2D.velocity += direction * jumpForce;

        //particle.Play();
    }

    private void WallJump()
    {
        
    }

    private void Dash(float hor, float ver)
    {
        //todo camera shaking
        
        _hasDashed = true;
        
        //todo anime set dash

        _rigidbody2D.velocity = Vector2.zero;
        Vector2 direction = new Vector2(hor, ver);
        _rigidbody2D.velocity += direction.normalized * dashSpeed;
    }

    private IEnumerator DashCD;



    private void Landing()
    {
        _hasDashed = false;
        _isDash = false;
        //todo side=animator.spriteRender.flip ? -1:1
        //jumpParticle.Play();
    }

    private void Sliding()
    {
        
    }
    
    int ParticleSide()
    {
        int particleSide = CollisionCheck.onWallR ? 1 : -1;
        return particleSide;
    }

}