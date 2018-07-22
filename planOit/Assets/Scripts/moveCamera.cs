using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveCamera : MonoBehaviour {

    public Vector3 Targetposition;

    public float speed = 2f;

    void Update()
    {
            transform.position = Vector3.Lerp(transform.position, new Vector3(Targetposition.x, Targetposition.y, -10), speed * Time.deltaTime);
    }
}
