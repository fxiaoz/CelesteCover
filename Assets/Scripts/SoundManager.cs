using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SoundManager : MonoBehaviour
{
    public AudioClip jump;
    public AudioClip dash;
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
        
    }
}