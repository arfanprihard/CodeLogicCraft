using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    public string scene;
    public int tingkatKesulitan;

    void Start()
    {
        SceneManager.LoadScene(scene);
    }
}
