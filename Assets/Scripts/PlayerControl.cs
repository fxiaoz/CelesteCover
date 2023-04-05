using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
    //Stats
    public float speed = 1f;

    public float castDist = 0.2f;
    public float gravityScale = 5f;
    public float gravityFall = 40f;
    public float jumpLimit = 1f;

    //Bool
    public bool dash = false;
    public bool jump = false;
    public bool climb = true;

    //Components
    Rigidbody2D myBody;
    Animator myAnim;
    SpriteRenderer myRend;

    // Start is called before the first frame update
    void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();
        myRend = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {

        }
        else if (Input.GetKeyDown(KeyCode.D))
        {

        }

        if (Input.GetKeyDown(KeyCode.W))
        {

        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            jump = true;
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            dash = true;
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            if (climb)
            {
                climb = false;
            }

            if (!climb)
            {
                climb = true;
            }
        }
    }

    void FixedUpdate()
    {
        if (jump)
        {

        }

        if (dash)
        {

        }
    }

    private void OnCollisionEnter2D(Collision2D Collision)
    {

    }
}
