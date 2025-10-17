using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Sound Effects")]
    public AudioClip placeBlockClip;
    public AudioClip gameOverClip;
    private AudioSource audioSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    public void PlayPlaceBlock()
    {
        if (placeBlockClip != null)
        {
            audioSource.PlayOneShot(placeBlockClip);
        }
    }

    public void PlayGameOver()
    {
        if (gameOverClip != null)
        {
            audioSource.PlayOneShot(gameOverClip);
        }
    }
}
