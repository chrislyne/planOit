﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using planOit;
using UnityEngine.UI;

public class PlayerState : MonoBehaviour {

    public planet currentPlanet;
    private static readonly int MAX_HEALTH = 1000;
    private int health = MAX_HEALTH;
    public int healthDamageRate;

    public int foodDepletionRate;
    public int oxygenDepletionRate;
    public int materialsDepletionRate;
    public float fuelSpendMultiplier;

    public float resourceDepletionMultiplier;

    public GameObject[] resourceBars;
    public GameObject healthBar;

    public ResourceSet resources;

    private bool IsHealthDepleting = false;

    private static readonly int MATERIAL_PER_REPAIR = 25;
    private static readonly int HEALTH_PER_REPAIR = 10;

    private GameObject popupCanvas;
    private Text popupText;

    // Use this for initialization
    void Start() {
        resources = new ResourceSet(100, 100, 100, 100);
        InvokeRepeating("ExpendResources", 0, 0.5f);

        popupCanvas = GameObject.Find("PopUpCanvas");
        popupText = GameObject.Find("PopUpText").GetComponent<Text>();
        popupCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update() {

        if (resources.ResourceDepleted && !IsHealthDepleting)
        {
            InvokeRepeating("ReduceHealth", 0, 0.5f);
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
            resources.fuel -= Mathf.CeilToInt(amountToUse*fuelSpendMultiplier);
        }
        else
        {
            //TODO: failure state?
            print("No more fuel.");
        }
    }

    public void StartGathering(planet planet)
    {
        currentPlanet = planet;
        // Start consuming resources over time
        InvokeRepeating("consumeFromCurrentPlanet", 0.5f, 0.5f);
    }

    public void consumeFromCurrentPlanet()
    {
        resources.addFrom(currentPlanet.resources);

        currentPlanet.updateUI();
        if (currentPlanet.resources.ResourceTotal == 0)
        {
            CancelInvoke("consumeFromCurrentPlanet");
        }
    }

    public void startEvent(PlanetEventType eventType)
    {
        switch(eventType)
        {
            case PlanetEventType.BONUS_RESOURCES:
            case PlanetEventType.BONUS_RESOURCES2:
                showPopup("Good news everyone! Something good happened and now you have more of something!");
                int resourceToAdd = Random.Range(0, 4);
                int RESOURCES_TO_ADD = 50;
                switch(resourceToAdd)
                {
                    case 0:
                        resources.oxygen += RESOURCES_TO_ADD;
                        break;
                    case 1:
                        resources.food += RESOURCES_TO_ADD;
                        break;
                    case 2:
                        resources.fuel += RESOURCES_TO_ADD;
                        break;
                    case 3:
                        resources.materials += RESOURCES_TO_ADD;
                        break;
                }
                break;
            case PlanetEventType.RESOURCE_PENALTY:
                showPopup("Funny story! Something bad happened and now you have less of something!");
                int resourceToTake = Random.Range(0, 4);
                int RESOURCES_TO_TAKE = -50;
                switch (resourceToTake)
                {
                    case 0:
                        resources.oxygen += RESOURCES_TO_TAKE;
                        break;
                    case 1:
                        resources.food += RESOURCES_TO_TAKE;
                        break;
                    case 2:
                        resources.fuel += RESOURCES_TO_TAKE;
                        break;
                    case 3:
                        resources.materials += RESOURCES_TO_TAKE;
                        break;
                }
                break;
            case PlanetEventType.ALIEN_ATTACK:
                Debug.Log("TODO: Show ALIEN_ATTACK UI");
                Debug.Log("TODO: Start attacking the user");
                break;
            default:
                break;
        }
    }

    private void showPopup(string message)
    {
        StartCoroutine(showPopupInternal(message));
    }

    private IEnumerator showPopupInternal(string message)
    {
        popupCanvas.SetActive(true);
        popupText.text = message;
        yield return new WaitForSeconds(5);
        popupCanvas.SetActive(false);
    }

    public void TryToRepair()
    {
        if (resources.materials > MATERIAL_PER_REPAIR && health < MAX_HEALTH)
        {
            resources.materials -= MATERIAL_PER_REPAIR;
            health += HEALTH_PER_REPAIR;
        }
    }


}
