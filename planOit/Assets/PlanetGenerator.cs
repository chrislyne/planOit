using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGenerator : MonoBehaviour
{

    public GarethsPlanet planetSource;

    private static float MAX_X_DISTANCE = 100.0f;
    private static float MIN_X_GAP = 10.0f;
    private static float MAX_Y_GAP = 12.0f;

    // TODO: Shape Equation:
    private float MaxYForX(float x)
    {
        return MAX_Y_GAP * Mathf.Sin((x * Mathf.PI / MAX_X_DISTANCE));
    }

    // Use this for initialization
    void Start ()
    {
        planetSource.gameObject.transform.position = Vector3.zero;
        GenerateTree();
    }

    void GenerateTree()
    {
        int loopLimit = 5;
        List<GarethsPlanet> parents = new List<GarethsPlanet>();
        parents.Add(planetSource);
        while(parents.Count > 0 && loopLimit-- > 0) 
        {
            parents = addChildNodes(parents);
        }
    }

    List<GarethsPlanet> addChildNodes(List<GarethsPlanet> parents)
    {
        List<GarethsPlanet> childnodes = new List<GarethsPlanet>();
        foreach(GarethsPlanet planet in parents)
        {
            if (planet.Location.x == MAX_X_DISTANCE)
            {
                // this is the last planet
                childnodes.Clear();
                return childnodes;
            }
            if (planet.Location.x + MIN_X_GAP >= MAX_X_DISTANCE)
            {
                childnodes.Clear();
                GameObject finalPlanetObject = GameObject.Instantiate(planetSource.gameObject);
                GarethsPlanet finalPlanet = finalPlanetObject.GetComponent<GarethsPlanet>();
                finalPlanet.Location = new Vector3(MAX_X_DISTANCE, 0f, 0f);
                planet.Children.Add(finalPlanet);
                childnodes.Add(finalPlanet);
                return childnodes;
            }
        }

        print("TODO: // Add a random number of new planets");
        
        return childnodes;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
