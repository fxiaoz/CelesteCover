using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using DG.Tweening;
using UnityEngine.Serialization;

public class Movement : MonoBehaviour
{
    private BoxCollider2D _collision2D;
    private Rigidbody2D _rigidbody2D;
    private AnimatorScript _animator;

    [SerializeField] public float moveSpeed = 10f;
    [SerializeField] public float jumpForce = 50f;
    [SerializeField] public float dashSpeed = 20f;
    [SerializeField] public float jumpLerp = 10f;
    [SerializeField] public float slideSpeed = 5f;

    private float _x, _y, _xRaw, _yRaw;
    
    public bool canMove, isGrab, isSlide, isDash, onGround;
    private bool _hasDashed, _isWallJumped;

    public ParticleSystem dashParticle,jumpParticle,wallJumpParticle,slideParticle;

    //character facing (R:1, L:-1)
    private int _side = 1;

    // Start is called before the first frame update
    void Start()
    {
        _collision2D = GetComponent<BoxCollider2D>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<AnimatorScript>();
        canMove = true;
        isGrab = false;
        isDash = false;
        isSlide = false;
        _isWallJumped = false;
        onGround = true;
        _hasDashed = false;
    }

    // Update is called once per frame
    void Update()
    {
        _x = Input.GetAxis("Horizontal");
        _y = Input.GetAxis("Vertical");
        _xRaw = Input.GetAxisRaw("Horizontal");
        _yRaw = Input.GetAxisRaw("Vertical");   
        
        //无停止惯性
        Vector2 direction = _xRaw!=0 ? new Vector2(_x, _y) : new Vector2(_xRaw, _y);
        //有停止惯性
        //Vector2 direction=new Vector2(_x,_y);
        
        Walk(direction);
        _animator.SetHorizontalMovement(_x,_y,_rigidbody2D.velocity.y);
        StateMachine();
    }

    private void Walk(Vector2 direction)
    {
        if (!canMove||isGrab)
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
        if (CollisionCheck.onWall&&Input.GetButton("Grab")&&canMove)
        {
            if (_side!=CollisionCheck.wallSide)
            {
                _animator.Flip(_side*-1);
            }

            isGrab = true;
            isSlide = false;
        }

        if (Input.GetButtonUp("Grab") || !CollisionCheck.onWall || !canMove)
        {
            isGrab = false;
            isSlide = false;
        }

        if (CollisionCheck.onGround && !isDash)
        {
            _isWallJumped = false;
            GetComponent<FixedFalling>().enabled = true;
        }

        if (isGrab&&!isDash)
        {
            _rigidbody2D.gravityScale = 0;
            if (_x> 0.2f||_x<-0.2f)
            {
                _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, 0);
            }
            float speedModifier = _y > 0 ? 0.5f : 1;
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _y * (moveSpeed * speedModifier));
        }
        else
        {
            _rigidbody2D.gravityScale = 3;
        }

        if (CollisionCheck.onWall&&!CollisionCheck.onGround)
        {
            if (_x!=0&&!isGrab)
            {
                isSlide = true;
                Sliding();
            }
        }

        if (!CollisionCheck.onWall||CollisionCheck.onGround)
        {
            isSlide = false;
        }

        if (Input.GetButtonDown("Jump"))
        {
            _animator.SetTrigger("Jump");
            if (CollisionCheck.onGround)
            {
                Jump(Vector2.up,false);
            }
            if (CollisionCheck.onWall&&!CollisionCheck.onGround)
            {
                WallJump();
            }
        }

        if (Input.GetButtonDown("Dash")&&!_hasDashed)
        {
            if (_xRaw!=0||_yRaw!=0)
            {
                Dash(_xRaw,_yRaw);
            }
        }

        if (CollisionCheck.onGround&&!onGround)
        {
            Landing();
            onGround = true;
        }

        if (!CollisionCheck.onGround&&onGround)
        {
            onGround = false;
        }
        
        //todo wall particle effect
        
        //if climbing or sliding, skip the flipping
        if (isGrab||isSlide||!canMove)
        {
            return;
        }

        if (_x>0)
        {
            _side = 1;
            _animator.Flip(_side);
        }

        if (_x<0)
        {
            _side = -1;
            _animator.Flip(_side);
        }
    }

    private void Jump(Vector2 direction,bool ifWallJump)
    {
        //slideParticle.transform.parent.localScale = new Vector3(ParticleSide(), 1, 1);
        //ParticleSystem particle = ifWallJump ? wallJumpParticle : jumpParticle;
        _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, 0);
        _rigidbody2D.velocity += direction * jumpForce;

        //particle.Play();
    }

    private void WallJump()
    {
        if (_x==0)
        {
            StopCoroutine(DisableMovement(0));
            StartCoroutine(DisableMovement(0.1f));
            Jump(Vector2.up,true);
        }
        else
        {
            if ((_side==1&&CollisionCheck.onWallR)||(_side==-1&&CollisionCheck.onWallL))
            {
                _side *= -1;
                _animator.Flip(_side);
            }
            
            StopCoroutine(DisableMovement(0));
            StartCoroutine(DisableMovement(0.1f));
            
            Vector2 direction = CollisionCheck.onWallL ? Vector2.left : Vector2.right;
            Jump((Vector2.up / 1.5f + direction / 1.5f), true);
            _isWallJumped = true;
        }
    }

    private void Dash(float hor, float ver)
    {
        //todo camera shaking
        
        _hasDashed = true;

        _animator.SetTrigger("Dash");

        _rigidbody2D.velocity = Vector2.zero;
        Vector2 direction = new Vector2(hor, ver);
        _rigidbody2D.velocity += direction.normalized * dashSpeed;
        StartCoroutine(DashRecover());
    }

    private IEnumerator DashRecover()
    {
        StartCoroutine(GroundDash());
        DOVirtual.Float(14, 0, 0.8f, RigidbodyDrag);

        //todo dashParticle.Play();
        _rigidbody2D.gravityScale = 0;
        GetComponent<FixedFalling>().enabled = false;
        _isWallJumped = true;
        isDash = true;

        yield return new WaitForSeconds(0.5f);

        //todo dashParticle.Stop();
        _rigidbody2D.gravityScale = 3;
        GetComponent<FixedFalling>().enabled = true;
        _isWallJumped = false;
        isDash = false;
        if (CollisionCheck.onGround)
        {
            _hasDashed = false;
        }
    }

    private IEnumerator GroundDash()
    {
        yield return new WaitForSeconds(0.15f);
        if (CollisionCheck.onGround)
        {
            _hasDashed = true;
        }
    }

    private void Landing()
    {
        _hasDashed = false;
        isDash = false;
        _side = _animator.sprite.flipX ? -1 : 1;
        _animator.SetTrigger("Land");
        //todo jumpParticle.Play();
    }

    private void Sliding()
    {
        if (CollisionCheck.wallSide!=_side)
        {
            _animator.Flip(_side*-1);
        }

        if (!canMove)
        {
            return;
        }
        
        //checking that if it is moving toward the wall
        bool isPushing = (_rigidbody2D.velocity.x > 0 && CollisionCheck.onWallR) || (_rigidbody2D.velocity.x < 0 && CollisionCheck.onWallL);
        float force = isPushing ? 0 : _rigidbody2D.velocity.x;
        _rigidbody2D.velocity = new Vector2(force, -slideSpeed);
    }
    
    IEnumerator DisableMovement(float time)
    {
        canMove = false;
        yield return new WaitForSeconds(time);
        canMove = true;
    }
    
    void RigidbodyDrag(float x)
    {
        _rigidbody2D.drag = x;
    }
    
    //todo wall particle method
    
    private int ParticleSide()
    {
        int particleSide = CollisionCheck.onWallR ? 1 : -1;
        return particleSide;
    }
}