using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using planOit;

public class PlayerState : MonoBehaviour {

    public Planet currentPlanet;
    public int health;
    public int healthDamageRate;

    public int foodDepletionRate;
    public int oxygenDepletionRate;
    public int materialsDepletionRate;

    public float resourceDepletionMultiplier;

    public GameObject[] resourceBars;

    public ResourceSet resources;

    // Use this for initialization
    void Start() {
        resources = new ResourceSet(100, 100, 100, 100);
        InvokeRepeating("ExpendResources", 0, 1);
    }

    // Update is called once per frame
    void Update() {

        if (resources.ResourceDepleted)
        {
            InvokeRepeating("ReduceHealth", 0, 1);
        } else
        {
            CancelInvoke("ReduceHealth");
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
                Debug.Log("TODO: Show BONUS_RESOURCES UI");
                Debug.Log("TODO: Instantly give the user some of a resource type");
                break;
            case PlanetEventType.RESOURCE_PENALTY:
                Debug.Log("TODO: Show RESOURCE_PENALTY UI");
                Debug.Log("TODO: Instantly take some of a resource type from the user");
                break;
            case PlanetEventType.ALIEN_ATTACK:
                Debug.Log("TODO: Show ALIEN_ATTACK UI");
                Debug.Log("TODO: Start attacking the user");
                break;
            default:
                break;
        }
    }
}
