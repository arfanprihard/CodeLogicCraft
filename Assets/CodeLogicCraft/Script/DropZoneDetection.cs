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
            if (obj.name == "Main" || obj.name == "Method" || obj.name == "LoopIn")
            {
                gameObject.tag = "Button";
                break;
            }

            else
            {
                gameObject.tag = "Untagged";
                break;
            }
        }
    }
}