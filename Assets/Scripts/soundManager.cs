using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundManager : MonoBehaviour
{
    public Movement myScriptReference;
    public AudioClip Jump1;
    public AudioClip dash1;
    private AudioSource myAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        // Get the AudioSource component
        myAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        myAudioSource.PlayOneShot(dash1);
        // Play Jump1 sound when onGround is true
        if ((myScriptReference.onGround == true) && (Input.GetKeyDown(KeyCode.Space)))
        {
            myAudioSource.PlayOneShot(Jump1);
            Debug.Log("its happenenin babey!!");
        }
        if ((myScriptReference.isDash == true))
        {
            myAudioSource.PlayOneShot(dash1);
        }
    }
}