using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentAudio : MonoBehaviour
{
    private static PersistentAudio instance = null;
    private AudioSource audioSource;

    void Awake()
    {
        // Check if an instance already exists
        if (instance == null)
        {
            // This is the first instance, so set it as the singleton
            instance = this;
            // Prevent this GameObject from being destroyed when a new scene loads
            DontDestroyOnLoad(gameObject);

            // Get the AudioSource component attached to this GameObject
            audioSource = GetComponent<AudioSource>();

            // If there's no AudioSource, add one (though you should ideally add it in the Inspector)
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }

            // You can optionally start playing the music here if "Play On Awake" wasn't checked
            // if (!audioSource.isPlaying && audioSource.clip != null)
            // {
            //     audioSource.Play();
            // }
        }
        else if (instance != this)
        {
            // Another PersistentAudio instance already exists, so destroy this one
            Destroy(gameObject);
        }
    }

    // Public method to change the background music (optional)
    public void PlayNewMusic(AudioClip newClip)
    {
        if (audioSource != null && newClip != null)
        {
            audioSource.clip = newClip;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("AudioSource not found or new AudioClip is null on PersistentAudio.");
        }
    }

    // Public method to adjust the volume (optional)
    public void SetVolume(float volumeLevel)
    {
        if (audioSource != null)
        {
            audioSource.volume = Mathf.Clamp01(volumeLevel); // Ensure volume is between 0 and 1
        }
        else
        {
            Debug.LogWarning("AudioSource not found on PersistentAudio.");
        }
    }

    // Public static property to easily access the instance from other scripts (optional)
    public static PersistentAudio Instance
    {
        get { return instance; }
    }
}