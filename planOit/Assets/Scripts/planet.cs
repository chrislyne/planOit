using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using planOit;

public class planet : MonoBehaviour
{
    public int minResource;
    public int maxResource;
    public int minSpecialResource;
    public int maxSpecialResource;

    private static readonly float DISTANCE_PER_FUEL = 0.5f;
    private static readonly int FUEL_PER_JUMP = 120;

    private Camera cam;

    public Sprite[] sprites;
    public Sprite[] damagedSprites01;
    public Sprite[] damagedSprites02;
    public Sprite[] damagedSprites03;
    public GameObject spriteNode;
    public GameObject resourcesUI;

    public ResourceSet resources;
    public bool alive = true;
    private PlanetEventType planetEvent = PlanetEventType.UNSET;

    GameObject ship;
    GameObject straw;
    Animator strawAnimator;

    public GameObject line;
    private LineRenderer destinationLine;
    PlayerState playerState;

    private Image spriteRenderer;
    PlanetType planetType;

    private RectTransform[] resourceIconSizes;

    private Button button;

    public bool isSpecialPlanet = false;
    public bool isEndPlanet = false; // special & true == start planet

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

        //ship
        ship = GameObject.Find("Rocket");
        straw = GameObject.Find("RocketSprite");
        strawAnimator = straw.GetComponent<Animator>();

        cam = Camera.main;
        
        

        spriteRenderer = spriteNode.GetComponent<Image>();

        if (isSpecialPlanet)
        {

            if (isEndPlanet)
            {
                //planet scale
                float planetSize = Random.Range(4.0f, 4.0f);
                transform.localScale = new Vector3(planetSize, planetSize, planetSize);

                spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/end_planet");
            }
            else
            {
                //planet scale
                float planetSize = Random.Range(3.0f, 3.0f);
                transform.localScale = new Vector3(planetSize, planetSize, planetSize);

                spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/start_planet");
            }
        }
        else
        {
            //planet scale
            float planetSize = Random.Range(0.5f, 1.5f);
            transform.localScale = new Vector3(planetSize, planetSize, planetSize);

            //planet sprite
            planetType = (PlanetType)Random.Range(0, (int)PlanetType.COUNT);
            spriteRenderer.sprite = sprites[(int)planetType];

            resources = new ResourceSet(
                Random.Range(minResource, maxResource),
                Random.Range(minResource, maxResource),
                Random.Range(minResource, maxResource),
                Random.Range(minResource, maxResource)
                );
            switch (planetType)
            {
                case PlanetType.BACON:
                    resources.food = Random.Range(minSpecialResource, maxSpecialResource);
                    break;
                case PlanetType.NACHOS:
                    resources.food = Random.Range(minSpecialResource, maxSpecialResource);
                    break;
                case PlanetType.SPOTTY:
                    resources.fuel = Random.Range(minSpecialResource, maxSpecialResource);
                    break;
                case PlanetType.STRIPE:
                    resources.materials = Random.Range(minSpecialResource, maxSpecialResource);
                    break;
                case PlanetType.WAVY:
                    resources.oxygen = Random.Range(minSpecialResource, maxSpecialResource);
                    break;
            }

            // # of Children expected to match # of resource types
            GameObject[] resourceObjects = new GameObject[resourcesUI.transform.childCount];
            resourceIconSizes = new RectTransform[resourcesUI.transform.childCount];
            for (int c = 0; c < resourcesUI.transform.childCount; c++)
            {
                resourceObjects[c] = resourcesUI.transform.GetChild(c).gameObject;

                int resourceValue = resources.getResourceByIndex(c);

                Text resourceTextUI = resourceObjects[c].GetComponentInChildren<Text>(true);
                resourceTextUI.text = resourceValue.ToString();

                Image resourceImageUI = resourceObjects[c].GetComponentInChildren<Image>(true);
                resourceIconSizes[c] = resourceImageUI.gameObject.GetComponent<RectTransform>();
                float iconSize = (float)resourceValue / 200.0f + 0.8f;
                resourceIconSizes[c].sizeDelta = new Vector2(iconSize, iconSize);
            }
            updateUI();
        }
        

        button = GetComponentInChildren<Button>();

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
        playerState.resources.fuel -= FUEL_PER_JUMP;
        
        Vector3 newPosition = new Vector3(transform.position.x, transform.position.y, -10);
        cam.GetComponent<moveCamera>().Targetposition = newPosition;
        ship.GetComponent<MoveShip>().Targetposition = new Vector3(transform.position.x, transform.position.y, transform.position.z-1);

        strawAnimator.Play("strawPlay", -1, 0f);
        playerState.StartGathering(this);

        if (planetEvent == PlanetEventType.ALIEN_ATTACK)
        {
            // Continue the attack
            Debug.Log("TODO: Aliens still present and attacking you");
        }
    }

    public void updateUI()
    {
        if (isSpecialPlanet)
        {
            return; // No UI Changes
        }
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
        else if (resources.IsNoLongerFull)
        {
            if (spriteRenderer.sprite != damagedSprites01[(int)planetType])
            {
                spriteRenderer.sprite = damagedSprites01[(int)planetType];
                // Start Event
                planetEvent = EventManager.getRandomEvent();
                playerState.startEvent(planetEvent);
                // Event needs to be permanent in case the user returns
                switch(planetEvent)
                {
                    case PlanetEventType.ALIEN_ATTACK:
                        // Do nothing, need to keep this state
                        break;
                    default:
                        // Reset event to... NOTHING so on re-entry nothing happens
                        planetEvent = PlanetEventType.NOTHING;
                        break;
                }
            }
        }

    }

    public void setEndPlanet()
    {
        isSpecialPlanet = true;
        isEndPlanet = false;
        // Hide Resources
        resourcesUI.gameObject.SetActive(false);
    }

    public void setStartPlanet()
    {
        isSpecialPlanet = true;
        alive = false;
        
        // Hide Resources
        resourcesUI.gameObject.SetActive(false);
    }
}
