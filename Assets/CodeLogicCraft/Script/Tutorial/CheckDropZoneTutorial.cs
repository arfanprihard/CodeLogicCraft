using UnityEngine;

public class CheckDropZoneTutorial : MonoBehaviour
{
    public int maxTotalMain;
    public GameObject main;
    public GameObject nextObject;
    public GameObject currentObject;
    private Draggable draggable;
    void Start()
    {
        draggable = FindFirstObjectByType<Draggable>();
    }
    void Update()
    {
        if (draggable.CountTaggedActiveChildren(main.transform) >= maxTotalMain)
        {
            currentObject.SetActive(false);
            if (nextObject != null)
            {
                nextObject.SetActive(true);
            }

        }
        else
        {
            currentObject.SetActive(true);
            if (nextObject != null)
            {
                nextObject.SetActive(false);
            }
        }
    }
}
