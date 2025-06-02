using System.Collections;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject enableObject;
    public float delayInSeconds = 0f;

    void Start()
    {

        int tingkatKesulitan = PlayerPrefs.GetInt("TingkatKesulitan");
        Debug.Log("Tingkat Kesulitan = " + tingkatKesulitan);
        SaveLoadSystem.Instance.SetSudahBukaStory(tingkatKesulitan, true);

        StartCoroutine(ActivateAfterDelay());
    }

    IEnumerator ActivateAfterDelay()
    {
        yield return new WaitForSeconds(delayInSeconds);
        enableObject.SetActive(true);
    }
}
