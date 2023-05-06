using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DustBehaviour : MonoBehaviour
{
    public ParticleSystem dustParticle;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play(float time)
    {
        gameObject.SetActive(true);
        
        dustParticle.Play();
        
        Sequence s = DOTween.Sequence();
        s.AppendInterval(time);
        s.AppendCallback(() =>
        {
            gameObject.SetActive(false);
        });
    }
}
