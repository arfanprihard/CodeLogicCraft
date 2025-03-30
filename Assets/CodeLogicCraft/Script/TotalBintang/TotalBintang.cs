using UnityEngine;
using TMPro;

public class TotalBintang : MonoBehaviour
{
    public TMP_Text totalBintangTxt;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        totalBintangTxt.text = SaveLoadSystem.Instance.GetTotalBintang() + "/60";
    }

}
