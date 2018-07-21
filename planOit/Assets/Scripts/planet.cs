using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    private Camera cam;

    public Sprite[] sprites;
    public GameObject spriteNode;
    public GameObject resourcesUI;

    public ResourceSet resources;

    private Image spriteRenderer;

    private enum PlanetType
    {
        BACON,
        NACHOS,
        SPOTTY,
        STRIPE,
        WAVY,
        COUNT // Why must I Hack?
    }

    void Start()
    {
        cam = Camera.main;
        //planet sprite
        spriteRenderer = spriteNode.GetComponent<Image>();
        PlanetType planetType = (PlanetType) Random.Range(0, (int)PlanetType.COUNT);
        spriteRenderer.sprite = sprites[(int)planetType];

        //planet scale
        float planetSize = Random.Range(0.5f, 1.5f);
        transform.localScale = new Vector3(planetSize, planetSize, planetSize);

        resources = new ResourceSet(
            Random.Range(0, 200),
            Random.Range(0, 200),
            Random.Range(0, 200),
            Random.Range(0, 200)
            );
        switch (planetType)
        {
            case PlanetType.BACON:
                resources.food = Random.Range(200, 300);
                break;
            case PlanetType.NACHOS:
                resources.food = Random.Range(200, 300);
                break;
            case PlanetType.SPOTTY:
                resources.fuel = Random.Range(200, 300);
                break;
            case PlanetType.STRIPE:
                resources.materials = Random.Range(200, 300);
                break;
            case PlanetType.WAVY:
                resources.oxygen = Random.Range(200, 300);
                break;
        }

        // # of Children expected to match # of resource types
        GameObject[] resourceObjects = new GameObject[resourcesUI.transform.childCount];
        for(int c = 0; c < resourcesUI.transform.childCount; c++) {
            resourceObjects[c] = resourcesUI.transform.GetChild(c).gameObject;

            int resourceValue = resources.getResourceByIndex(c);
           
            Text resourceTextUI = resourceObjects[c].GetComponentInChildren<Text>(true);
            resourceTextUI.text = resourceValue.ToString();

            Image resourceImageUI = resourceObjects[c].GetComponentInChildren<Image>(true);
            RectTransform rt = resourceImageUI.gameObject.GetComponent<RectTransform>();
            float iconSize = (float)resourceValue / 200.0f + 0.8f;
            rt.sizeDelta = new Vector2(iconSize, iconSize);
        }
    }

    public void Hover()
    {
        print("hover");
        resourcesUI.transform.localScale = new Vector3(3, 3, 3);
    }
    public void HoverOut()
    {
        print("hoverOut");
        resourcesUI.transform.localScale = new Vector3(2, 2, 2);
    }

    public void MoveCamera()
    {
        //print(transform.position.x);
        Vector3 newPosition = new Vector3(transform.position.x, transform.position.y, -10);
        //cam.transform.position = newPosition; 
        cam.GetComponent<moveCamera>().Targetposition = newPosition;
    }
}
