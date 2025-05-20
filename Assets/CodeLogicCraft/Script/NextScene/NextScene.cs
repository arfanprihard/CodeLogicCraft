using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    public string scene;
    void Start()
    {
        SceneManager.LoadScene(scene);
    }
}
