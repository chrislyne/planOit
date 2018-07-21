using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour {
    private Camera cam;

    public Sprite[] sprites;
    public GameObject spriteNode;
    public GameObject resourcesUI;

    public ResourceSet resources;

    public float[] resourceValue;
    public Text[] resourceText;
    public GameObject[] resourceBar;

    private Image spriteRenderer;


    void Start()
    {
        cam = Camera.main;
        //planet sprite
        spriteRenderer = spriteNode.GetComponent<Image>();
        var randomSprite = Random.Range(0,5);
        spriteRenderer.sprite = sprites[randomSprite];

        //planet scale
        float planetSize = Random.Range(0.5f,1.5f);
        transform.localScale = new Vector3(planetSize,planetSize,planetSize);

        //planet resource UI
        int i = 0;
        foreach (float rv in resourceValue)
        {
            float randomResourceValue = Random.Range(0,200);
            resourceValue[i] = randomResourceValue;
            RectTransform rt = resourceBar[i].GetComponent<RectTransform>();
            float iconSize = randomResourceValue / 200 + 0.8f;
            rt.sizeDelta = new Vector2(iconSize, iconSize);
            resourceText[i].text = randomResourceValue.ToString();
            i++;
        }
    }

    public void Hover()
    {
        print("hover");
        resourcesUI.transform.localScale = new Vector3(3,3,3);
    }
    public void HoverOut()
    {
        print("hoverOut");
        resourcesUI.transform.localScale = new Vector3(2, 2, 2);
    }

    public void MoveCamera()
    {
        //print(transform.position.x);
        Vector3 newPosition = new Vector3(transform.position.x,transform.position.y,-10);
        //cam.transform.position = newPosition; 
        cam.GetComponent<moveCamera>().Targetposition = newPosition;
    }
}
