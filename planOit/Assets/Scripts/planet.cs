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

    public float[] resourceValue;
    public Text[] resourceText;
    public GameObject[] resourceBar;

    public GameObject line;
    private LineRenderer destinationLine;
    PlayerState playerState;
    Planet currentPlanet;

    private Image spriteRenderer;

    private enum PlanetType
    {
        BACON,
        NACHOS,
        SPOTTY,
        STRIPE,
        WAVY
    }

    void Start()
    {
        //dotted line
        destinationLine = GameObject.Find("Line").GetComponent<LineRenderer>();
        playerState = GameObject.Find("HUD").GetComponent<PlayerState>();


        cam = Camera.main;
        //planet sprite
        spriteRenderer = spriteNode.GetComponent<Image>();
        PlanetType planetType = (PlanetType) Random.Range(0, 9);
        spriteRenderer.sprite = sprites[(int)planetType];

        //planet scale
        float planetSize = Random.Range(0.5f, 1.5f);
        transform.localScale = new Vector3(planetSize, planetSize, planetSize);

        ResourceSet resourceSet = new ResourceSet(
            Random.Range(0, 200),
            Random.Range(0, 200),
            Random.Range(0, 200),
            Random.Range(0, 200)
            );
        switch (planetType)
        {
            case PlanetType.BACON:
                break;
            case PlanetType.NACHOS:
                break;
            case PlanetType.SPOTTY:
                break;
            case PlanetType.STRIPE:
                break;
            case PlanetType.WAVY:
                break;
        }
        //planet resource UI
        int i = 0;
        foreach (float rv in resourceValue)
        {
            float randomResourceValue = Random.Range(0, 200);
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
}
