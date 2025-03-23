using Unity.VisualScripting;
using UnityEngine;

public class DropZoneDetection : MonoBehaviour
{
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