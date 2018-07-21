using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class planet : MonoBehaviour {
    public Sprite[] sprites;
    public GameObject spriteNode;
    public GameObject cam;
    public GameObject resourcesUI;

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

        //resourceValue[1] = 10f;
        //planet resource UI
        int i = 0;
        foreach (float rv in resourceValue)
        {
            //resourceValue[i] = 10f;
            RectTransform rt = resourceBar[i].GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(1, rv / 100);
            resourceText[i].text = rv.ToString();
        }
    }

    public void Hover()
    {
        print("hover");
        resourcesUI.transform.localScale = new Vector3(2,2,2);
    }
    public void HoverOut()
    {
        print("hoverOut");
        resourcesUI.transform.localScale = new Vector3(1, 1, 1);
    }

    public void MoveCamera()
    {
        //print(transform.position.x);
        Vector3 newPosition = new Vector3(transform.position.x,transform.position.y,-10);
        //cam.transform.position = newPosition; 
        cam.GetComponent<moveCamera>().Targetposition = newPosition;
    }
}
