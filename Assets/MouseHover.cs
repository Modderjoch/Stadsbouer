using UnityEngine;

public class MouseHover : MonoBehaviour
{
    // The height to move the object when hovering.
    public float hoverHeight = 0.5f;

    // The speed at which to move the object.
    public float hoverSpeed = 1f;

    // The position of the object before it started hovering.
    private Vector3 originalPosition;

    // Whether the object is currently hovering.
    private bool isHovering = false;

    void Start()
    {
        // Save the original position of the object.
        originalPosition = transform.position;
    }

    void Update()
    {
        if (isHovering)
        {
            // Calculate the new position of the object based on the hover height.
            Vector3 hoverPosition = originalPosition + Vector3.forward * hoverHeight;

            // Move the object towards the hover position at the specified speed.
            transform.position = Vector3.MoveTowards(transform.position, hoverPosition, hoverSpeed * Time.deltaTime);
        }
        else
        {
            // Move the object back to its original position at the specified speed.
            transform.position = Vector3.MoveTowards(transform.position, originalPosition, hoverSpeed * Time.deltaTime);
        }
    }

    public void StartHover()
    {
        // Start hovering when the mouse enters the object.
        isHovering = true;
    }

    public void StopHover()
    {
        // Stop hovering when the mouse exits the object.
        isHovering = false;
    }
}
