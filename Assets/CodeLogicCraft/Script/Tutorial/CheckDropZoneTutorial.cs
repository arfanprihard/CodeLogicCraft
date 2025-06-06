using UnityEngine;

public class CheckDropZoneTutorial : MonoBehaviour
{
    public int maxTotalMain;
    public GameObject main;
    public string nameObject = "";
    public GameObject nextObject;
    public GameObject currentObject;
    private Draggable draggable;
    void Start()
    {
        draggable = FindFirstObjectByType<Draggable>();
    }
    void Update()
    {
        if (nameObject == "")
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
        else
        {
            if (IsThereButton(main.transform, nameObject))
            {
                currentObject.SetActive(false);
                if (nextObject != null)
                {
                    nextObject.SetActive(true);
                }
            }
        }
    }
    private bool IsThereButton(Transform parent, string name)
    {
        foreach (Transform child in parent.GetComponentsInChildren<Transform>(true))
        {
            if (child.name == name)
            {
                return true;
            }
        }
        return false;
    }
}
