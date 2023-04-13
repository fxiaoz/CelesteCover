using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedFalling : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(_rigidbody2D.velocity.y < 0)
        {
            _rigidbody2D.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if(_rigidbody2D.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            _rigidbody2D.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }
}
