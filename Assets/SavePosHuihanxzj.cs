using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SavePosHuihanxzj : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("playerX"))
        {
            float x = PlayerPrefs.GetFloat("playerX");
            float y = PlayerPrefs.GetFloat("playerY");
            transform.position = new Vector3(x, y, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var position = transform.position;
            PlayerPrefs.SetFloat("playerX",position.x);
            PlayerPrefs.SetFloat("playerY",position.y);
            Scene scene = SceneManager.GetActiveScene();
            PlayerPrefs.SetString("lastScene",scene.name);
        }
    }
}
