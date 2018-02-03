using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraKeybordController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    float moveSpeed = 25;

	// Update is called once per frame
	void Update () {

        Vector3 translate = new Vector3
            (
                Input.GetAxis("Horizontal"),
                Input.GetAxis("Vertical"),
                0
            );

        this.transform.Translate(translate * moveSpeed* Time.deltaTime, Space.World);

	}
}
