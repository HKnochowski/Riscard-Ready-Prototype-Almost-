using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Update_CurrentFunc = Update_DetectModeStart;

    }

    Vector3 lastMousePosition;
    
    /*[System.NonSerialized]*/ bool allowChangeCameraAngle = true;
    int mouseDragTreshold = 1;
    Vector3 lastMouseGroundPlanePosition;
    Vector3 cameraTargetOffset;

    Unit selectedUnit = null;

    delegate void UpdateFunc();
    UpdateFunc Update_CurrentFunc;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CancelUpdateFunc();
        }

        Update_CurrentFunc();
        Update_ScrollZoom();

        lastMousePosition = Input.mousePosition;
    }

    void CancelUpdateFunc()
    {
        Update_CurrentFunc = Update_DetectModeStart;
    }

    void Update_DetectModeStart()
    {
        if (Input.GetMouseButtonDown(0))
        {

        }
        else if (Input.GetMouseButtonUp(0))
        {

        }
        else if (Input.GetMouseButton(0) &&
            Vector3.Distance(Input.mousePosition, lastMousePosition) > mouseDragTreshold)
        {
            Update_CurrentFunc = Update_CameraDrag;
            lastMouseGroundPlanePosition = MouseToGroundPlane(Input.mousePosition);
            Update_CurrentFunc();
        }
        else if (selectedUnit != null && Input.GetMouseButton(1))
        {
             
        }

    }

    Vector3 MouseToGroundPlane(Vector3 mousePos)
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(mousePos);
        if (mouseRay.direction.z <= 0)
        {
            //Debug.LogError("Why is mouse pointing up?");
            return Vector3.zero;
        }
        float rayLength = (mouseRay.origin.z / mouseRay.direction.z);
        return mouseRay.origin - (mouseRay.direction * rayLength);
    }

    void Update_UnitMovement()
    {
        if (Input.GetMouseButtonUp(1))
        {
            CancelUpdateFunc();
            return;
        }
    }


    // Update is called once per frame
    void Update_CameraDrag () {

        if (Input.GetMouseButtonUp(0))
        {
            CancelUpdateFunc();
            return;
        }

        Vector3 hitPos = MouseToGroundPlane(Input.mousePosition);

        Vector3 diff = lastMouseGroundPlanePosition - hitPos;
        Camera.main.transform.Translate(diff, Space.World);
        
        lastMouseGroundPlanePosition = hitPos = MouseToGroundPlane(Input.mousePosition);

        

    }

    void Update_ScrollZoom()
    {
        float scrollAmount = Input.GetAxis("Mouse ScrollWheel");
        float minHeight = -4;
        float maxHeight = -40;
        if (Mathf.Abs(scrollAmount) > 0.01f)
        {
            Vector3 hitPos = MouseToGroundPlane(Input.mousePosition);


            Vector3 dir = hitPos - Camera.main.transform.position;

            Vector3 p = Camera.main.transform.position;

            if (scrollAmount > 0 || p.z > (maxHeight + 0.1f))
            {
                Camera.main.transform.Translate(dir * scrollAmount, Space.World);
            }

            p = Camera.main.transform.position;

            if (p.z > minHeight)
            {
                p.z = minHeight;
            }
            if (p.z < maxHeight)
            {
                p.z = maxHeight;
            }
            Camera.main.transform.position = p;





            Camera.main.transform.rotation = Quaternion.Euler(
                           Mathf.Lerp(-70, 0, Camera.main.transform.position.z / (maxHeight / 1f)),
                           Camera.main.transform.rotation.eulerAngles.y,
                           Camera.main.transform.rotation.eulerAngles.z
                           );
        }
    }

}
