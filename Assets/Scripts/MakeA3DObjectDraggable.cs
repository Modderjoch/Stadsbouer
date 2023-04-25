using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/*
    MAKE A 3D OBJECT DRAGGABLE
    Riccardo Stecca
    http://www.riccardostecca.net
    https://github.com/rstecca
    And check out UNotes here: https://github.com/rstecca/UNotes
    
    !!! Remember that in order to get this working you need a PhysicsRaycaster on the current camera. !!!
*/

[RequireComponent(typeof(Collider))]
public class MakeA3DObjectDraggable : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{

    Camera m_cam;

    void Start()
    {
        if (Camera.main.GetComponent<PhysicsRaycaster>() == null)
            Debug.Log("Camera doesn't ahve a physics raycaster.");

        m_cam = Camera.main;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Ray R = m_cam.ScreenPointToRay(Input.mousePosition); // Get the ray from mouse position
        Vector3 PO = transform.position; // Take current position of this draggable object as Plane's Origin
        Vector3 PN = -m_cam.transform.forward; // Take current negative camera's forward as Plane's Normal
        float t = Vector3.Dot(PO - R.origin, PN) / Vector3.Dot(R.direction, PN); // plane vs. line intersection in algebric form. It find t as distance from the camera of the new point in the ray's direction.
        Vector3 P = R.origin + R.direction * t; // Find the new point.

        transform.position = P;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Begin drag");
        // Do stuff when dragging begins. For example suspend camera interaction.
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("End drag");
        // Do stuff when draggin ends. For example restore camera interaction.
    }
}