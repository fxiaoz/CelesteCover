using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SoundManager : MonoBehaviour
{
    public bool meow = false;
    public AudioClip[] sounds;
    public AudioClip jump;
    public AudioClip dash;
    public AudioClip meow1;
    public AudioClip meow2;
    public AudioClip meow3;
    public AudioClip meow4;
    public AudioClip landing;
    private AudioSource _myAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        _myAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Jump()
    {
        _myAudioSource.PlayOneShot(jump);
    }

    public void Land()
    {
        _myAudioSource.PlayOneShot(landing);
    }

    public void Dash()
    {
        _myAudioSource.PlayOneShot(dash);
    }

    public void Slide()
    {
        
    }

    public void Climb()
    {
        
    }

    public void Walk()
    {
        
    }

    public void Grab()
    {
        
    }

    public void Meow()
    {
        if ((Input.GetMouseButtonDown(0)) && (meow == false)){
            meow = true;

        }
        else if ((Input.GetMouseButtonUp(0)) && (meow == true))
        {
            meow = false;
        }
    }
}