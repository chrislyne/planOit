using System.Collections;
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

    public float resourceDepletionMultiplier;

    public GameObject[] resourceBars;
    public GameObject healthBar;

    public ResourceSet resources;

    private bool IsHealthDepleting = false;

    private static readonly int MATERIAL_PER_REPAIR = 25;
    private static readonly int HEALTH_PER_REPAIR = 10;

    private GameObject popupCanvas;
    private Text popupText;

    private bool gameStopped = false;

    private GameObject winScreen;
    private GameObject loseScreen;

    // Use this for initialization
    void Start() {
        resources = new ResourceSet(250, 250, 250, 250);
        InvokeRepeating("ExpendResources", 0, 1);

        popupCanvas = GameObject.Find("PopUpCanvas");
        popupText = GameObject.Find("PopUpText").GetComponent<Text>();
        popupCanvas.SetActive(false);

        Transform[] screenTransforms = GameObject.Find("/Screens/Canvas").GetComponentsInChildren<Transform>(true);
        foreach(Transform transform in screenTransforms)
        {
            if (transform.name == "WinScreen")
            {
                winScreen = transform.gameObject;
            } else if (transform.name == "LoseScreen")
            {
                loseScreen = transform.gameObject;
            }
        }
    }

    // Update is called once per frame
    void Update() {
        if (gameStopped)
        {
            return;
        }
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
            stopGameLoop(false);
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
        if (planet.isEndPlanet)
        {
            stopGameLoop(true);
        }
        else
        {
            // Start consuming resources over time
            InvokeRepeating("consumeFromCurrentPlanet", 0.5f, 0.5f);
        }
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

    private void stopGameLoop(bool success)
    {
        gameStopped = true;
        CancelInvoke();
        Debug.Log("Game stopped with status = " + success);
        if (success)
        {
            winScreen.SetActive(true);
        } else
        {
            loseScreen.SetActive(true);
        }
    }
}
