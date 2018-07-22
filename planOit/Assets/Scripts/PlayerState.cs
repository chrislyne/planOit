using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using planOit;

public class PlayerState : MonoBehaviour {

    public planet currentPlanet;
    private static readonly int MAX_HEALTH = 1000;
    private int health = MAX_HEALTH;
    public int healthDamageRate;

    public int foodDepletionRate;
    public int oxygenDepletionRate;
    public int materialsDepletionRate;

    public float resourceDepletionMultiplier;

    public GameObject[] resourceBars;
    public GameObject healthBar;

    public ResourceSet resources;

    private bool IsHealthDepleting = false;

    private static readonly int MATERIAL_PER_REPAIR = 25;
    private static readonly int HEALTH_PER_REPAIR = 10;

    // Use this for initialization
    void Start() {
        resources = new ResourceSet(250, 250, 250, 250);
        InvokeRepeating("ExpendResources", 0, 1);
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
        if (resources.oxygen > 0) resources.oxygen -= Mathf.RoundToInt(oxygenDepletionRate*resourceDepletionMultiplier);
        if (resources.food > 0) resources.food -= Mathf.RoundToInt(foodDepletionRate*resourceDepletionMultiplier);
    }

    void ReduceHealth()
    {
        health -= healthDamageRate;
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

    public void TryToRepair()
    {
        if (resources.materials > MATERIAL_PER_REPAIR && health < MAX_HEALTH)
        {
            resources.materials -= MATERIAL_PER_REPAIR;
            health += HEALTH_PER_REPAIR;
        }
    }


}
