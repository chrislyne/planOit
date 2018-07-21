using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour {

    public int health;

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

        if (health <= 0 || resources.ResourceDepleted)
        {
            print("YOU DIED!");
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
        resources.materials -= Mathf.RoundToInt(materialsDepletionRate*resourceDepletionMultiplier);
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

    public void StartGathering()
    {

    }
}
