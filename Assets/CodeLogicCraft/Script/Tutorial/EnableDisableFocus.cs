using UnityEngine;

public class EnableDisableFocus : MonoBehaviour
{
    public GameObject focus;
    public GameObject disableObject;
    public bool isNextEnable;
    void OnEnable()
    {
        focus.SetActive(true);
        disableObject.SetActive(false);
    }

    void OnDisable()
    {
        focus.SetActive(false);
        if (isNextEnable)
        {
            disableObject.SetActive(true);
        }

    }
}
