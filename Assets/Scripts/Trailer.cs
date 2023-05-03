using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Trailer : MonoBehaviour
{
    private Movement _movement;
    private AnimatorScript _animatorScript;
    private SpriteRenderer _sprite;
    public Transform ghostsParent;
    public Color trailColor, fadeColor;
    public float ghostInterval, fadeTime;
    
    // Start is called before the first frame update
    void Start()
    {
        _animatorScript=FindObjectOfType<AnimatorScript>();
        _movement = FindObjectOfType<Movement>();
        _sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowGhost()
    {
        Sequence s = DOTween.Sequence();

        for (int i = 0; i < ghostsParent.childCount; i++)
        {
            Transform currentGhost = ghostsParent.GetChild(i);
            s.AppendCallback(()=> currentGhost.position = _movement.transform.position);
            s.AppendCallback(() => currentGhost.GetComponent<SpriteRenderer>().flipX = _animatorScript.sprite.flipX);
            s.AppendCallback(()=>currentGhost.GetComponent<SpriteRenderer>().sprite = _animatorScript.sprite.sprite);
            s.Append(currentGhost.GetComponent<SpriteRenderer>().material.DOColor(trailColor, 10));
            s.AppendCallback(() => FadeSprite(currentGhost));
            s.AppendInterval(ghostInterval);
        }
    }

    private void FadeSprite(Transform current)
    {
        current.GetComponent<SpriteRenderer>().material.DOKill();
        current.GetComponent<SpriteRenderer>().material.DOColor(fadeColor, fadeTime);
    }
}
