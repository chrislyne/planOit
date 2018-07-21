using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGenerator : MonoBehaviour
{

    public Planet planetPrefab;

    private Planet endPlanet;

    private static readonly int MAX_RANDOM_PLANETS = 1000;

    private static readonly float MAX_X_DISTANCE = 1000.0f;
    private static readonly float MIN_GAP = 30.0f;
    private static readonly float MAX_GAP = 80.0f;
    private static readonly float MAX_Y_GAP = MIN_GAP; // +- Variation around Y axis
    private static readonly float MIN_Z_DEPTH = 20.0f;
    private static readonly float MAX_Z_DEPTH = 60.0f;

    // Use this for initialization
    void Start ()
    {
        Planet startPlanet = (Planet) Instantiate(planetPrefab, new Vector3(0f, 0f, (MAX_Z_DEPTH + MIN_Z_DEPTH) / 2), Quaternion.identity);
        startPlanet.name = "Start Planet";

        endPlanet = (Planet) Instantiate(planetPrefab, new Vector3(MAX_X_DISTANCE, 0f, (MAX_Z_DEPTH + MIN_Z_DEPTH) / 2), Quaternion.identity);
        endPlanet.name = "End Planet";

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
                0f
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

        Vector3 currentPosition = Vector3.zero;

        for (int p = 0; p < MAX_RANDOM_PLANETS; p++)
        {
            if (Vector3.Magnitude(planetPositions[p] - currentPosition) <= MIN_GAP)
            {
                // Too close
                p++;
                continue;
            }
            if (Vector3.Magnitude(planetPositions[p] - currentPosition) > MAX_GAP)
            {
                // Too far
                p++;
                continue;
            }
            // Just right
            // But compare with all currently added:
            bool tooClose = false;
            foreach(Vector3 goodPos in prunedPositions)
            {
                if (Vector3.Magnitude(planetPositions[p] - goodPos) <= MIN_GAP)
                {
                    tooClose = true;
                    break;
                }
            }
            if (tooClose)
            {
                p++;
                continue;
            }
            currentPosition = planetPositions[p];
            prunedPositions.Add(currentPosition);

        }
        Debug.Log("Planets to be created:" + prunedPositions.Count);
        // Check distance to end planet
        if (Vector3.Magnitude(currentPosition - endPlanet.transform.position) > MAX_GAP)
        {
            // After checking "the last one, check through all"
            bool tooFar = true;
            foreach (Vector3 goodPos in prunedPositions)
            {
                if (Vector3.Magnitude(goodPos - endPlanet.transform.position) <= MAX_GAP)
                {
                    tooFar = false;
                    break;
                }
            }
            if (tooFar)
            {
                Debug.LogError("Last node is too far, retry random gen");
                //GenerateTree();
                //return;
            }
        }

        // Actually create
        foreach (Vector3 position in prunedPositions) 
        {
            Vector3 posWithZ = new Vector3(position.x, position.y, Random.Range(MIN_Z_DEPTH, MAX_Z_DEPTH));
            Planet planet = Instantiate(planetPrefab, posWithZ, Quaternion.identity);
            // TODO: Random Type
        }
    }

	// Update is called once per frame
	void Update () {
		
	}
}
