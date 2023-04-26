using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleCollision : MonoBehaviour
{
    public bool haveCoin = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "obj_player")
        {
            haveCoin = true;
            Destroy(gameObject);
            //mySource.PlayOneShot(coinsfx, volume); (for sound effect)
        }
    }

}
