using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class planet : MonoBehaviour {
    public Sprite[] sprites;
    public GameObject spriteNode;
    public GameObject cam;

    public float[] resourceValue;
    public Text[] resourceText;
    public GameObject[] resourceBar;

    private Image spriteRenderer;


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
            rt.sizeDelta = new Vector2(1, rv / 100);
            resourceText[i].text = rv.ToString();
        }
    }



    public void MoveCamera()
    {
        //print(transform.position.x);
        Vector3 newPosition = new Vector3(transform.position.x,transform.position.y,-10);
        //cam.transform.position = newPosition; 
        cam.GetComponent<moveCamera>().Targetposition = newPosition;
    }
}
