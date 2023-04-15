using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animator : MonoBehaviour
{
    private Animator _animator;
    private SpriteRenderer _sprite;
    private Movement _movement;
    
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _sprite = GetComponent<SpriteRenderer>();
        _movement = GetComponent<Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        //todo set bool onGround
        //todo set bool onWall
        //todo set bool onWallR
        //todo set bool Grab
        //todo set bool Slide
        //todo set bool Dash
        //todo set bool canMove
    }

    public void SetHorizontalMovement(float x,float y, float yVel)
    {
        //todo SetFloat x
        //todo SetFloat y
        //todo SetFloat yVel
    }
    
    public void Flip(int side)
    {
        //上墙状态,如果对齐则不执行
        if (_movement.ifOnWall())
        {
            if ((side == -1 && _sprite.flipX)||(side == 1 && !_sprite.flipX))
                return;
        }
        
        bool state = (side != 1);
        _sprite.flipX = state;
    }
    
}
