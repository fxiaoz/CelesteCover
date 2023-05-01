using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))//later change to when the player reaches the goal point
        {
            SceneManager.LoadScene("Level 1");
            //debug test
        }
    }

<<<<<<< HEAD
    public void buttonClick()
    {
        SceneManager.LoadScene("Title");
=======
    // loads the tutorial scene (connects to the Desend button on the start screen)
    public void LoadGame()
    {
        SceneManager.LoadScene("Tutorial");
    }

    // exits the game (connects to the Exit button on the start sceen)
    public void ExitGame()
    {
        Application.Quit();
>>>>>>> Jeff2
    }
}
