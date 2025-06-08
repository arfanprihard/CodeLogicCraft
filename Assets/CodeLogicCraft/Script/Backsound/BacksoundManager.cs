using UnityEngine;
using UnityEngine.SceneManagement;

public class BacksoundManager : MonoBehaviour
{
    public AudioClip backsoundMenu;
    public AudioClip backsoundGameplay;

    private AudioSource audioSource;
    private static BacksoundManager instance;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject); // Hapus duplikat
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.volume = 0.5f;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Ganti backsound sesuai nama scene
        switch (scene.name)
        {
            case "MainMenu":
            case "LevelPage":
                PlayClip(backsoundMenu);
                break;

            case "InGame":
            case "TutorialDasar":
            case "TutorialPerulangan":
            case "TutorialPercabangan":
            case "TutorialMethod":
                PlayClip(backsoundGameplay);
                break;

            case "Prolog":
            case "StoryDasar":
            case "StoryPerulangan":
            case "StoryPercabangan":
            case "StoryMethod":
                StopClip();
                break;

            default:
                PlayClip(backsoundGameplay);
                break;
        }
    }

    void PlayClip(AudioClip clip)
    {
        if (audioSource.clip == clip) return;

        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.Play();
    }

    void StopClip()
    {
        audioSource.Stop();
        audioSource.clip = null;
    }
}
