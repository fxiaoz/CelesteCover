using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animator : MonoBehaviour
{
    private Animator _animator;
    private SpriteRenderer _sprite;
    
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _sprite = GetComponent<SpriteRenderer>();
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
    
    
}
