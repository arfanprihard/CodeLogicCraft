using UnityEngine;

public class PercabanganButton : MonoBehaviour
{
    public GameObject isi;
    void Update()
    {
        if (gameObject.transform.childCount > 1)
        {
            isi.SetActive(false);
        }
        else
        {
            isi.SetActive(true);
        }
    }
}
