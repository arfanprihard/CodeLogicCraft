using UnityEngine;

public class WaterManagement : MonoBehaviour
{
    public GameObject water1;
    public GameObject water2;
    void Start()
    {

        if (Application.platform == RuntimePlatform.Android)
        {
            water1.SetActive(false);
            water2.SetActive(true);
        }
        else
        {
            water1.SetActive(true);
            water2.SetActive(false);
        }

    }
}
