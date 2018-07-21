using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGenerator : MonoBehaviour
{

    public Planet planetSource;            
    private GameObject endPlanet;

    private static readonly int MAX_RANDOM_PLANETS = 1000;

    private static readonly float MAX_X_DISTANCE = 100.0f;
    private static readonly float MIN_GAP = 40.0f;
    private static readonly float MAX_GAP = 80.0f;
    private static readonly float MAX_Y_GAP = 5.0f; // Vertical Size of the Camera

    // Use this for initialization
    void Start ()
    {
        planetSource.gameObject.transform.position = Vector3.zero;

        endPlanet = GameObject.Instantiate(planetSource.gameObject);
        endPlanet.transform.position = new Vector3(MAX_X_DISTANCE, 0f, 0f);

        GenerateTree();
    }

    private int GenerationLimit = 5;
    void GenerateTree()
    {
        GenerationLimit--;
        if (GenerationLimit <= 0)
        {
            Debug.LogError("Took a few attempts and failed to generate a suitable tree");
            return;
        }
        List<Vector3> planetPositions = new List<Vector3>();
        for (int p = 0; p < MAX_RANDOM_PLANETS; p++)
        {
            planetPositions.Add(new Vector3(
                Random.Range(0.0f, MAX_X_DISTANCE-MIN_GAP),
                Random.Range(-MAX_Y_GAP, MAX_Y_GAP),
                Random.Range(-80.0f, 0.0f)
                )
            );
        }

        planetPositions.Sort((a, b) => Vector3.Magnitude(a).CompareTo(Vector3.Magnitude(b)));

        if (planetPositions[0].magnitude > MAX_GAP) {
            // First node is too far, retry random gen
            Debug.LogError("First node is too far, retry random gen");
            GenerateTree();
            return;
        }

        List<Vector3> prunedPositions = new List<Vector3>();
        
        for (int p = 0; p < MAX_RANDOM_PLANETS - 1; p++)
        {
            prunedPositions.Add(planetPositions[p]);
            while(p+1 < MAX_RANDOM_PLANETS)
            {
                if (Vector3.Magnitude(planetPositions[p + 1] - planetPositions[p]) <= MIN_GAP)
                {
                    // Too close
                    p++;
                    continue;
                }
                if (Vector3.Magnitude(planetPositions[p + 1] - planetPositions[p]) > MAX_GAP)
                {
                    // Too far
                    p++;
                    continue;
                }
                // Just right
                break;
            }
        }
        
        // Actually create
        foreach(Vector3 position in prunedPositions) 
        {
            GameObject planet = GameObject.Instantiate(planetSource.gameObject);
            planet.transform.position = position;
            // TODO: Random Type
        }
    }

	// Update is called once per frame
	void Update () {
		
	}
}
