using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveShip : MonoBehaviour {

    public Vector3 Targetposition;

    public float speed = 2f;

    public GameObject fireSprite;

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, Targetposition, speed * Time.deltaTime);
        float distance = Vector3.Distance(transform.position, Targetposition);
        if (distance < 1)
        {
            fireSprite.GetComponent<SpriteRenderer>().enabled = false;  
        }
        else
        {
            fireSprite.GetComponent<SpriteRenderer>().enabled = true; 
        }
    }
}
