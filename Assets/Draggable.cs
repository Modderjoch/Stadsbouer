using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Draggable : MonoBehaviour
{
    bool isDragging = false;
    private GameObject _drag;
    public LayerMask gamePieceLayer;
    public LayerMask gameCollisionObjectsLayers;
    public float verticalOffset = 4;
    public Transform targetArea;
    public float overlapCheckRadius = 0.3f;

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out hit, 200.0f, gamePieceLayer))
            {
                isDragging = true;
                _drag = hit.transform.gameObject;
            }
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            if (isDragging)
            {
                Collider[] colliders = Physics.OverlapSphere(_drag.transform.position, overlapCheckRadius, gameCollisionObjectsLayers);
                foreach (Collider collider in colliders)
                {
                    // Check if the collider belongs to a different object
                    if (collider.transform != _drag.transform)
                    {
                        Debug.Log("Overlap detected with: " + collider.name);
                        // Perform the desired action for overlapping objects here
                        // For example, you can snap the dragged object to a valid position
                        // or trigger some other game logic

                        collider.GetComponent<ParentCard>().ParentCardTo(gameObject);
                    }
                }

                _drag = null;
                isDragging = false;
            }
        }

        if (isDragging && Mouse.current.leftButton.isPressed)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            Debug.Log("C1");
            if (Physics.Raycast(ray, out hit, 200.0f, gameCollisionObjectsLayers))
            {
                Debug.Log("C");
                _drag.transform.position = hit.point + new Vector3(0, verticalOffset, 0);
            }
        }
    }

}