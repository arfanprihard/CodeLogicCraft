using System.Collections;
using UnityEngine;

public class NextObject : MonoBehaviour
{
    public GameObject targetObject;
    public float delayInSeconds = 0f;

    void Start()
    {
        StartCoroutine(ActivateAfterDelay());
    }

    IEnumerator ActivateAfterDelay()
    {
        yield return new WaitForSeconds(delayInSeconds);
        targetObject.SetActive(true);
    }
}
