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

    private float _x, _y, _xRaw, _yRaw;
    
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
        _x = Input.GetAxis("Horizontal");
        _y = Input.GetAxis("Vertical");
        _xRaw = Input.GetAxisRaw("Horizontal");
        _yRaw = Input.GetAxisRaw("Vertical");
        Vector2 direction = new Vector2(_x,_y);
        
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
        if (CollisionCheck.onWall&&Input.GetButton("Grab")&&_canMove)
        {
            if (_side!=CollisionCheck.wallSide)
            {
                //TODO Flip(side*-1)
            }

            _isGrab = true;
            _isSlide = false;
        }

        if (Input.GetButtonUp("Grab") || !CollisionCheck.onWall || !_canMove)
        {
            _isGrab = false;
            _isSlide = false;
        }

        if (CollisionCheck.onGround && !_isDash)
        {
            _isWallJumped = false;
            GetComponent<FixedFalling>().enabled = true;
        }

        if (_isGrab&&!_isDash)
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
            if (_x!=0&&!_isGrab)
            {
                _isSlide = true;
                Sliding();
            }
        }

        if (!CollisionCheck.onWall||CollisionCheck.onGround)
        {
            _isSlide = false;
        }

        if (Input.GetButtonDown("Jump"))
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

        if (Input.GetButtonDown("Dash")&&!_hasDashed)
        {
            if (_xRaw!=0||_yRaw!=0)
            {
                Dash(_xRaw,_yRaw);
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

        if (_x>0)
        {
            _side = 1;
            //todo anime flip
        }

        if (_x<0)
        {
            _side = -1;
            //todo anime flip
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
                //todo anim flip
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
        
        //todo anime dash

        _rigidbody2D.velocity = Vector2.zero;
        Vector2 direction = new Vector2(hor, ver);
        _rigidbody2D.velocity += direction.normalized * dashSpeed;
        StartCoroutine(DashRecover());
    }

    private IEnumerator DashRecover()
    {
        StartCoroutine(GroundDash());
        DOVirtual.Float(14, 0, 0.8f, RigidbodyDrag);

        //dashParticle.Play();
        _rigidbody2D.gravityScale = 0;
        GetComponent<FixedFalling>().enabled = false;
        _isWallJumped = true;
        _isDash = true;

        yield return new WaitForSeconds(0.5f);

        //dashParticle.Stop();
        _rigidbody2D.gravityScale = 3;
        GetComponent<FixedFalling>().enabled = true;
        _isWallJumped = false;
        _isDash = false;
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
        _isDash = false;
        //todo character sprite flip
        //todo jumpParticle.Play();
    }

    private void Sliding()
    {
        if (CollisionCheck.wallSide!=_side)
        {
            //todo anim flip
        }

        if (!_canMove)
        {
            return;
        }
        
        //检测是否朝向墙体方向运动
        bool isPushing = (_rigidbody2D.velocity.x > 0 && CollisionCheck.onWallR) || (_rigidbody2D.velocity.x < 0 && CollisionCheck.onWallL);
        float force = isPushing ? 0 : _rigidbody2D.velocity.x;
        _rigidbody2D.velocity = new Vector2(force, -slideSpeed);
    }
    
    IEnumerator DisableMovement(float time)
    {
        _canMove = false;
        yield return new WaitForSeconds(time);
        _canMove = true;
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

    public bool ifOnWall()
    {
        return (_isGrab || _isSlide);
    }
}