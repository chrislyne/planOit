﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour {

    public Planet currentPlanet;
    public int health;
    public int healthDamageRate;

    public int foodDepletionRate;
    public int oxygenDepletionRate;
    public int materialsDepletionRate;

    public float resourceDepletionMultiplier;

    public GameObject[] resourceBars;
    public GameObject healthBar;

    public ResourceSet resources;

    private bool IsHealthDepleting = false;

    // Use this for initialization
    void Start() {
        resources = new ResourceSet(100, 100, 100, 100);
        InvokeRepeating("ExpendResources", 0, 1);
    }

    // Update is called once per frame
    void Update() {

        if (resources.ResourceDepleted && !IsHealthDepleting)
        {
            InvokeRepeating("ReduceHealth", 0, 1);
            IsHealthDepleting = true;
        }
        else
        {
            if (!resources.ResourceDepleted) {
                IsHealthDepleting = false;
                CancelInvoke("ReduceHealth");
            } else
            {

            }
        }

        if (health <= 0)
        {
            CancelInvoke();
            //TODO: End game
        }

        resourceBars[0].GetComponent<RectTransform>().sizeDelta = new Vector2(resources.oxygen/10f, 1);
        resourceBars[1].GetComponent<RectTransform>().sizeDelta = new Vector2(resources.food/10f, 1);
        resourceBars[2].GetComponent<RectTransform>().sizeDelta = new Vector2(resources.fuel/10f, 1);
        resourceBars[3].GetComponent<RectTransform>().sizeDelta = new Vector2(resources.materials/10f, 1);

        healthBar.GetComponent<RectTransform>().sizeDelta = new Vector2(health/10f, 2);
    }

    void ExpendResources()
    {
        resources.oxygen -= Mathf.RoundToInt(oxygenDepletionRate*resourceDepletionMultiplier);
        resources.food -= Mathf.RoundToInt(foodDepletionRate*resourceDepletionMultiplier);
    }

    void ReduceHealth()
    {
        health -= healthDamageRate;
    }

    void SpendFuel(int amountToUse)
    {
        if (resources.fuel > amountToUse)
        {
            resources.fuel -= amountToUse;
        }
        else
        {
            //TODO: failure state?
            print("No more fuel.");
        }
    }

    public void StartGathering(Planet planet)
    {
        currentPlanet = planet;
        // Start consuming resources over time
        InvokeRepeating("consumeFromCurrentPlanet", 0.5f, 0.5f);
    }

    public void consumeFromCurrentPlanet()
    {
        resources.addFrom(currentPlanet.resources);
        currentPlanet.updateIconSizes();

        currentPlanet.updateSprite(currentPlanet.resources.ResourceTotal);
        if (currentPlanet.resources.ResourceTotal == 0)
        {
            CancelInvoke("consumeFromCurrentPlanet");
        }

    }


}
