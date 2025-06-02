using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnDragListener : MonoBehaviour
{
    public GameObject[] currentParents;
    private Draggable draggable;
    void Start()
    {
        draggable = FindFirstObjectByType<Draggable>();
    }
    void Update()
    {
        if (draggable.GetIsDragging())
        {
            foreach (GameObject currentParent in currentParents)
            {
                currentParent.SetActive(false);
            }
        }
        // else
        // {
        //     foreach (GameObject currentParent in currentParents)
        //     {
        //         currentParent.SetActive(true);
        //     }
        // }
    }
}
