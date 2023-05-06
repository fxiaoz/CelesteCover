using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCheck : MonoBehaviour
{
    public GameObject revivePosition;

    public LayerMask ground, lava;

    public static bool onGround, onWall, onWallR, onWallL, isDead;
    public static int wallSide;

    [SerializeField]public float collisionRadius = 1f;
    public Vector2 bottomOffset, rightOffset, leftOffset;

    private bool lastOnGround;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        lastOnGround = onGround;

        var position = transform.position;
        onGround = Physics2D.OverlapCircle((Vector2)position + bottomOffset, collisionRadius, ground);
        onWall = Physics2D.OverlapCircle((Vector2)position + rightOffset, collisionRadius, ground) 
                 || Physics2D.OverlapCircle((Vector2)position + leftOffset, collisionRadius, ground);
        
        onWallR = Physics2D.OverlapCircle((Vector2)position + rightOffset, collisionRadius, ground);
        onWallL = Physics2D.OverlapCircle((Vector2)position + leftOffset, collisionRadius, ground);

        //isDead = Physics2D.OverlapCircle((Vector2)position + bottomOffset, collisionRadius, lava)
         //        ||Physics2D.OverlapCircle((Vector2)position + rightOffset, collisionRadius, lava) 
         //        || Physics2D.OverlapCircle((Vector2)position + leftOffset, collisionRadius, lava);
        
        //return side of wall(R:1, L:-1)
        wallSide = onWallR ? -1 : 1;

        if(!lastOnGround && onGround)
        {
            DustBehaviour dust = transform.GetComponentInChildren<DustBehaviour>(true);

            if(dust != null)
            {
                dust.Play(0.5f);
            }
        }
    }
    
    //visible collider
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        var positions = new Vector2[] { bottomOffset, rightOffset, leftOffset };

        Gizmos.DrawWireSphere((Vector2)transform.position  + bottomOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, collisionRadius);
    }

    public static bool IfDead()
    {
        return isDead;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        int layer = LayerMask.NameToLayer("Lava");
        GameObject target = collision.gameObject;

        if(layer == target.layer)
        {
            transform.position = revivePosition.transform.position;
        }
    }
}
