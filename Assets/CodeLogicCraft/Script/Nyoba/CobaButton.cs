using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;

public class CobaButton : MonoBehaviour
{
    public Button cobaBt;
    public Transform main;
    public Transform cobaLayout;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cobaBt.onClick.AddListener(OnPlayClicked);
    }

    void OnPlayClicked()
    {
        PindahkanButtonKeLayout(main, cobaLayout);
    }

    private void PindahkanButtonKeLayout(Transform parent, Transform targetLayout)
    {
        if (parent != null)
        {
            foreach (Transform child in parent)
            {
                // Cek apakah child memenuhi kriteria
                if (child.CompareTag("Button") && child.parent.name != "isi" && child.gameObject.activeInHierarchy && child.name != "loopImg")
                {
                    // Pindahkan button ke layout lain
                    GameObject objekBaru = new GameObject(child.name);
                    objekBaru.transform.SetParent(targetLayout, true);
                }

                // Rekursif untuk memeriksa child dari child yang aktif
                PindahkanButtonKeLayout(child, targetLayout);
            }
        }
    }
}
