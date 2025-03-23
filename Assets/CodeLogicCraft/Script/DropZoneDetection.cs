using UnityEngine;

public class DropZoneDetection : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Transform obj = transform;
        while (obj != null) // Loop selama masih ada parent
        {
            obj = obj.parent;
            if (obj.name == "Main" || obj.name == "Method")
            {
                gameObject.tag = "Button";
                break;
            }
            else if (obj.name == "Canvas")
            {
                gameObject.tag = "Untagged";
                break;
            }
        }
    }
}
