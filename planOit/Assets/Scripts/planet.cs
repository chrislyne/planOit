using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour {
    public Sprite[] sprites;
    public GameObject spriteNode;
    public GameObject cam;

    public float[] resourceValue;
    public Text[] resourceText;
    public GameObject[] resourceBar;

    private Image spriteRenderer;

    public Vector3 Location
    {
        get
        {
            return gameObject.transform.position;
        }
        set
        {
            gameObject.transform.position = value;
        }
    }

    void Start()
    {
        //planet sprite
        spriteRenderer = spriteNode.GetComponent<Image>();
        var randomSprite = Random.Range(0,2);
        spriteRenderer.sprite = sprites[randomSprite];

        //planet resource UI
        int i = 0;
        foreach (float rv in resourceValue)
        {
            RectTransform rt = resourceBar[i].GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(1, rv/100);
            resourceText[i].text = rv.ToString();
            i++;
        }

    }
    public void MoveCamera()
    {
        print(transform.position.x);
        Vector3 newPosition = new Vector3(transform.position.x,transform.position.y,-10);
        cam.transform.position = newPosition; 
    }
}
