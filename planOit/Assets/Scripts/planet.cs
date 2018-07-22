﻿using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    private static readonly float DISTANCE_PER_FUEL = 0.5f;
    private Camera cam;

    public Sprite[] sprites;
    public Sprite[] damagedSprites01;
    public Sprite[] damagedSprites02;
    public Sprite[] damagedSprites03;
    public GameObject spriteNode;
    public GameObject resourcesUI;

    public ResourceSet resources;
    public bool alive = true;

    public GameObject line;
    private LineRenderer destinationLine;
    PlayerState playerState;

    private Image spriteRenderer;
    PlanetType planetType;

    private RectTransform[] resourceIconSizes;

    private Button button;

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

        button = GetComponentInChildren<Button>();

        updateUI();
    }

    public void Hover()
    {
        if (!alive)
        {
            return;
        }
        resourcesUI.transform.localScale = new Vector3(3, 3, 3);
        // Check distance before drawing line
        float distanceToPlanet = Vector3.Magnitude(playerState.currentPlanet.transform.position - transform.position);

        if (playerState.resources.fuel * DISTANCE_PER_FUEL >= distanceToPlanet)
        {
            destinationLine.SetPosition(0, playerState.currentPlanet.transform.position);
            destinationLine.SetPosition(1, transform.position);
        } else
        {
            Debug.Log("Not enough fuel to go distance: " + distanceToPlanet + " only enough for distance: " + (playerState.resources.fuel * DISTANCE_PER_FUEL));
        }
        
    }
    public void HoverOut()
    {
        resourcesUI.transform.localScale = new Vector3(2, 2, 2);
        destinationLine.SetPosition(0, new Vector3(0, 0, 100));
        destinationLine.SetPosition(1, new Vector3(0, 0, 100));
    }

    public void PlanetClicked()
    {
        if (!alive)
        {
            return;
        }

        float distanceToPlanet = Vector3.Magnitude(playerState.currentPlanet.transform.position - transform.position);
        if (playerState.resources.fuel < (int)(distanceToPlanet / DISTANCE_PER_FUEL))
        {
            // Not enough fuel.. Ignore
            return;
        }
        playerState.resources.fuel -= (int)(distanceToPlanet / DISTANCE_PER_FUEL);

        Vector3 newPosition = new Vector3(transform.position.x, transform.position.y, -10);
        cam.GetComponent<moveCamera>().Targetposition = newPosition;
        
        playerState.StartGathering(this);
    }

    public void updateUI()
    {
        updateSprite(); // Decides if alive, which is needed for icons
        updateIconSizes();
    }

    public void updateIconSizes()
    {
        if (!alive)
        {
            resourcesUI.gameObject.SetActive(false);
            return;
        }
        for (int c = 0; c < resourceIconSizes.Length; c++)
        {
            int resourceValue = resources.getResourceByIndex(c);
            float iconSize = (float)resourceValue / 200.0f + 0.8f;
            resourceIconSizes[c].sizeDelta = new Vector2(iconSize, iconSize);
        }
    }
    public void updateSprite()
    {
        if (!alive || resources.ResourceTotal == 0)
        {
            alive = false;
            if (spriteRenderer.sprite != damagedSprites03[(int)planetType])
            {
                spriteRenderer.sprite = damagedSprites03[(int)planetType];
                Image imageScript = button.GetComponentInChildren<Image>();
                imageScript.raycastTarget = false;
            }
        }
        else if (resources.ResourceTotal < 200)
        {
            spriteRenderer.sprite = damagedSprites02[(int)planetType];
        }
        else if (resources.ResourceTotal < 400)
        {
            if (spriteRenderer.sprite != damagedSprites01[(int)planetType])
            {
                spriteRenderer.sprite = damagedSprites01[(int)planetType];
                // Start Event
                playerState.startEvent();
            }
        }

    }
}
