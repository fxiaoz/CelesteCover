using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCheck : MonoBehaviour
{
    public LayerMask ground;

    public static bool onGround, onWall, onWallR, onWallL;
    public static int wallSide;

    [SerializeField]public float collisionRadius = 1f;
    public Vector2 bottomOffset, rightOffset, leftOffset;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var position = transform.position;
        onGround = Physics2D.OverlapCircle((Vector2)position + bottomOffset, collisionRadius, ground);
        onWall = Physics2D.OverlapCircle((Vector2)position + rightOffset, collisionRadius, ground) 
                 || Physics2D.OverlapCircle((Vector2)position + leftOffset, collisionRadius, ground);
        
        onWallR = Physics2D.OverlapCircle((Vector2)position + rightOffset, collisionRadius, ground);
        onWallL = Physics2D.OverlapCircle((Vector2)position + leftOffset, collisionRadius, ground);
        
        //return side of wall(R:1, L:-1)
        wallSide = onWallR ? -1 : 1;
    }
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        var positions = new Vector2[] { bottomOffset, rightOffset, leftOffset };

        Gizmos.DrawWireSphere((Vector2)transform.position  + bottomOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, collisionRadius);
    }
}
