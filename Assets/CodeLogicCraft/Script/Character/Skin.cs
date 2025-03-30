using UnityEngine;

public class Skin : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int skinIndex = PlayerPrefs.GetInt("SkinCharacter");
        Transform skins = gameObject.transform.GetChild(0);
        for (int i = 0; i < skins.transform.childCount; i++)
        {
            if (skinIndex == i)
            {
                skins.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                skins.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

}
