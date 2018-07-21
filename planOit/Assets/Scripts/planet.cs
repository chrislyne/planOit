using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    private Camera cam;

    public Sprite[] sprites;
    public Sprite[] damagedSprites01;
    public Sprite[] damagedSprites02;
    public Sprite[] damagedSprites03;
    public GameObject spriteNode;
    public GameObject resourcesUI;

    public ResourceSet resources;

    public GameObject line;
    private LineRenderer destinationLine;
    PlayerState playerState;
    Planet currentPlanet;

    private Image spriteRenderer;
    PlanetType planetType;

    private RectTransform[] resourceIconSizes;

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
        //dotted line
        destinationLine = GameObject.Find("Line").GetComponent<LineRenderer>();
        playerState = GameObject.Find("HUD").GetComponent<PlayerState>();


        cam = Camera.main;
        //planet sprite
        spriteRenderer = spriteNode.GetComponent<Image>();
        planetType = (PlanetType) Random.Range(0, (int)PlanetType.COUNT);
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
        resourceIconSizes = new RectTransform[resourcesUI.transform.childCount];
        for (int c = 0; c < resourcesUI.transform.childCount; c++) {
            resourceObjects[c] = resourcesUI.transform.GetChild(c).gameObject;

            int resourceValue = resources.getResourceByIndex(c);
           
            Text resourceTextUI = resourceObjects[c].GetComponentInChildren<Text>(true);
            resourceTextUI.text = resourceValue.ToString();

            Image resourceImageUI = resourceObjects[c].GetComponentInChildren<Image>(true);
            resourceIconSizes[c] = resourceImageUI.gameObject.GetComponent<RectTransform>();
            float iconSize = (float)resourceValue / 200.0f + 0.8f;
            resourceIconSizes[c].sizeDelta = new Vector2(iconSize, iconSize);
        }

    }

    public void Hover()
    {
        currentPlanet = playerState.currentPlanet;
        resourcesUI.transform.localScale = new Vector3(3, 3, 3);
        destinationLine.SetPosition(0, currentPlanet.transform.position);
        destinationLine.SetPosition(1, transform.position);
    }
    public void HoverOut()
    {
        resourcesUI.transform.localScale = new Vector3(2, 2, 2);
        destinationLine.SetPosition(0, new Vector3(0, 0, 100));
        destinationLine.SetPosition(1, new Vector3(0, 0, 100));
    }

    public void MoveCamera()
    {

        Vector3 newPosition = new Vector3(transform.position.x, transform.position.y, -10);
        cam.GetComponent<moveCamera>().Targetposition = newPosition;
        playerState.StartGathering(this);
    }

    public void updateIconSizes()
    {
        for (int c = 0; c < resourceIconSizes.Length; c++)
        {
            int resourceValue = resources.getResourceByIndex(c);
            float iconSize = (float)resourceValue / 200.0f + 0.8f;
            resourceIconSizes[c].sizeDelta = new Vector2(iconSize, iconSize);
        }
    }
    public void updateSprite(int total)
    {
        print (total);
        if(total == 0){
            spriteRenderer.sprite = damagedSprites03[(int)planetType];
        }
        else if (total < 200)
        {
            spriteRenderer.sprite = damagedSprites02[(int)planetType];
        }
        else if (total < 400)
        {
            spriteRenderer.sprite = damagedSprites01[(int)planetType];
        }

    }
}
