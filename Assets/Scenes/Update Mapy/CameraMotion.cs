using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotion : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    Vector3 oldPosition;

	// Update is called once per frame
	void Update () {

        CheckIfCameraMoved();
	}

    public void PanToHex(Hex hex)
    {

    }

    HexComponent[] hexes;

    void CheckIfCameraMoved() {
        if (oldPosition != this.transform.position)
        {
            oldPosition = this.transform.position;

            if(hexes == null)
                hexes = GameObject.FindObjectsOfType<HexComponent>();

            foreach(HexComponent hex in hexes)
            {
                hex.UpdatePosition();
            }
        }
    }
}
