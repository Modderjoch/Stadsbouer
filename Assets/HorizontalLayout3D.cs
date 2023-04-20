using UnityEngine;

public class HorizontalLayout3D : MonoBehaviour
{
    public float spacing = 1.0f; // Spacing between objects
    public float offset = 0.5f; // Offset from the center of the layout

    private void Start()
    {
        ArrangeChildren();
    }

    private void OnTransformChildrenChanged()
    {
        ArrangeChildren();
    }

    private void ArrangeChildren()
    {
        int numChildren = transform.childCount;
        if (numChildren <= 0) return;

        float totalWidth = 0.0f;

        // Calculate total width of all children objects
        for (int i = 0; i < numChildren; i++)
        {
            Transform child = transform.GetChild(i);
            Renderer renderer = child.GetComponentInChildren<Renderer>();
            if (renderer != null)
            {
                totalWidth += renderer.bounds.size.x + spacing;
            }
        }

        // Calculate starting position for the first child object
        Vector3 startPos = transform.position - transform.right * (totalWidth * 0.5f - offset);

        // Arrange children objects in a horizontal line
        for (int i = 0; i < numChildren; i++)
        {
            Transform child = transform.GetChild(i);
            Renderer renderer = child.GetComponentInChildren<Renderer>();
            if (renderer != null)
            {
                // Set position of the child object
                Vector3 pos = startPos + transform.right * (renderer.bounds.size.x * 0.5f);
                child.position = pos;

                // Update starting position for the next child object
                startPos += transform.right * (renderer.bounds.size.x + spacing);
            }
        }
    }
}
