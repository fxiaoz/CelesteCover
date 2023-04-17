using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class AnimatorScript : MonoBehaviour
{
    private Animator _animator;
    private Movement _movement;
    public SpriteRenderer sprite;
    
    private static readonly int OnGround = Animator.StringToHash("OnGround");
    private static readonly int OnWall = Animator.StringToHash("OnWall");
    private static readonly int OnWallR = Animator.StringToHash("OnWallR");
    private static readonly int IsGrab = Animator.StringToHash("IsGrab");
    private static readonly int IsSlide = Animator.StringToHash("IsSlide");
    private static readonly int IsDash = Animator.StringToHash("IsDash");
    private static readonly int CanMove = Animator.StringToHash("CanMove");
    private static readonly int VerticalVelocity = Animator.StringToHash("VerticalVelocity");
    private static readonly int VerticalAxis = Animator.StringToHash("VerticalAxis");
    private static readonly int HorizontalAxis = Animator.StringToHash("HorizontalAxis");

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        _movement = GetComponent<Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        _animator.SetBool(OnGround,CollisionCheck.onGround);
        _animator.SetBool(OnWall,CollisionCheck.onWall);
        _animator.SetBool(OnWallR,CollisionCheck.onWallR);
        _animator.SetBool(IsGrab,_movement.isGrab);
        _animator.SetBool(IsSlide,_movement.isSlide);
        _animator.SetBool(IsDash,_movement.isDash);
        _animator.SetBool(CanMove,_movement.canMove);
    }

    public void SetHorizontalMovement(float x,float y, float yVel)
    {
        _animator.SetFloat(HorizontalAxis,x);
        _animator.SetFloat(VerticalAxis,y);
        _animator.SetFloat(VerticalVelocity,yVel);
    }
    
    public void Flip(int side)
    {
        //上墙状态,如果对齐则不执行
        if (_movement.isGrab||_movement.isSlide)
        {
            if ((side == -1 && sprite.flipX)||(side == 1 && !sprite.flipX))
                return;
        }

        bool state = (side != 1);
        sprite.flipX = state;
    }
    
    public void SetTrigger(string trigger)
    {
        _animator.SetTrigger(trigger);
    }
}
