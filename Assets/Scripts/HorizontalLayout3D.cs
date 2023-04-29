using UnityEngine;

public class HorizontalLayout3D : MonoBehaviour
{
    public float spacing = 1.0f; // Spacing between objects
    public float offset = 0.5f; // Offset from the center of the layout

    public bool isCardParent;
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

//using UnityEngine;

//public class HorizontalLayout3D : MonoBehaviour
//{
//    public float spacing = 1.0f; // Spacing between objects
//    public float offset = 0.5f; // Offset from the center of the layout
//    public bool isCardParent;

//    public int maxItemsPerRow = 0; // Maximum number of items per row (0 = no limit)

//    private void Start()
//    {
//        ArrangeChildren(maxItemsPerRow);
//    }

//    private void OnTransformChildrenChanged()
//    {
//        ArrangeChildren(maxItemsPerRow);
//    }

//    public void ArrangeChildren(int maxItemsPerRow)
//    {
//        int numChildren = transform.childCount;
//        if (numChildren <= 0) return;

//        float totalWidth = 0.0f;
//        float rowWidth = 0.0f;
//        int itemsInRow = 0;
//        int row = 0;

//        // Calculate total width of all children objects
//        for (int i = 0; i < numChildren; i++)
//        {
//            Transform child = transform.GetChild(i);
//            Renderer renderer = child.GetComponentInChildren<Renderer>();
//            if (renderer != null)
//            {
//                rowWidth += renderer.bounds.size.x + spacing;
//                itemsInRow++;

//                if (itemsInRow == maxItemsPerRow || i == numChildren - 1)
//                {
//                    // Add the row width to the total width and reset the row width
//                    totalWidth += Mathf.Max(rowWidth - spacing, 0f);
//                    rowWidth = 0.0f;
//                    itemsInRow = 0;
//                    row++;
//                }
//            }
//        }

//        // Calculate starting position for the first child object
//        Vector3 startPos = transform.position - transform.right * (totalWidth * 0.5f - offset);

//        // Arrange children objects in horizontal rows
//        itemsInRow = 0;
//        row = 0;
//        Vector3 rowOffset = Vector3.zero;
//        for (int i = 0; i < numChildren; i++)
//        {
//            Transform child = transform.GetChild(i);
//            Renderer renderer = child.GetComponentInChildren<Renderer>();
//            if (renderer != null)
//            {
//                // Set position of the child object
//                Vector3 pos = startPos + transform.right * (renderer.bounds.size.x * 0.5f);
//                pos += rowOffset;
//                child.position = pos;

//                // Update starting position for the next child object
//                startPos += transform.right * (renderer.bounds.size.x + spacing);
//                rowWidth += renderer.bounds.size.x + spacing;
//                itemsInRow++;

//                if (itemsInRow == maxItemsPerRow || i == numChildren - 1)
//                {
//                    // Move to next row
//                    itemsInRow = 0;
//                    row++;
//                    rowOffset -= transform.up * (renderer.bounds.size.y + spacing);
//                    startPos = transform.position - transform.right * (rowWidth * 0.5f - offset);
//                    rowWidth = 0.0f;
//                }
//            }
//        }

//        if (isCardParent)
//        {
//            CardPrefab[] cardPrefabs = GetComponentsInChildren<CardPrefab>();
//            for (int i = 0; i < cardPrefabs.Length; i++)
//            {
//                int pos = cardPrefabs[i].itemPos;
//                cardPrefabs[i].itemPos = i;
//            }
//        }
//    }
//}
