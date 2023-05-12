using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "tutorial end")
        {
            SceneManager.LoadScene("Level 1");
        }

        if(collision.gameObject.tag == "level 1 end")
        {
            SceneManager.LoadScene("Level 3");
        }

        if(collision.gameObject.tag == "level 2 end")
        {
            SceneManager.LoadScene("Win");
        }

        if (collision.gameObject.tag == "Finish")
        {
            SceneManager.LoadScene("Win");
        }

        if (collision.gameObject.tag == "level 3 end")
        {
            SceneManager.LoadScene("level 2");
        }
    }
}
