using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class planet : MonoBehaviour {
    public Sprite[] sprites;

    private SpriteRenderer spriteRenderer; 
    private ResourceSet resources;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); 
        var randomSprite = Random.Range(0, 3);
        spriteRenderer.sprite = sprites[randomSprite];
    }
}
