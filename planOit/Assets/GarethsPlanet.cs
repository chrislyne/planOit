using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarethsPlanet : MonoBehaviour {

    public List<GarethsPlanet> Children;

    public Vector3 Location
    {
        get
        {
            return gameObject.transform.position;
        }
        set
        {
            gameObject.transform.position = value;
        }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
