using UnityEngine;

public class HorizontalLayout3D : MonoBehaviour
{
    public float spacing = 1.0f; // Spacing between objects
    public float offset = 0.5f; // Offset from the center of the layout

    public bool isCardParent;
    public bool isPlaceParent;
    public int maxItemsPerRow = 0;

    private void Start()
    {
        ArrangeChildren();
    }

    private void OnTransformChildrenChanged()
    {
        ArrangeChildren();
    }

    public void ArrangeChildren()
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
                if (isPlaceParent) { Debug.Log("This operation is not applicable"); } else { child.GetComponent<MouseHover>().originalPosition = pos; }

                // Update starting position for the next child object
                startPos += transform.right * (renderer.bounds.size.x + spacing);
            }
        }

        if (isCardParent)
        {
            CardPrefab[] cardPrefabs = GetComponentsInChildren<CardPrefab>();
            for (int i = 0; i < cardPrefabs.Length; i++)
            {
                int pos = cardPrefabs[i].itemPos;
                cardPrefabs[i].itemPos = i;
            }

        }
    }
}
