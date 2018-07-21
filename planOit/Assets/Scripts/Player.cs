using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public int foodDepletionRate;
    public int oxygenDepletionRate;
    public int materialsDepletionRate;

    public int resourceDepletionMultiplier;

    public ResourceSet resources;

    // Use this for initialization
    void Start() {
        resources = new ResourceSet(100, 100, 100, 100);
        InvokeRepeating("ExpendResources", 0, 1);
    }

    // Update is called once per frame
    void Update() {
        print(resources);
    }

    void ExpendResources()
    {
        resources.oxygen -= oxygenDepletionRate*resourceDepletionMultiplier;
        resources.food -= foodDepletionRate*resourceDepletionMultiplier;
        resources.materials -= materialsDepletionRate*resourceDepletionMultiplier;
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
}
