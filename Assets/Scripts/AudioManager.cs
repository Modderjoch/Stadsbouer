using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip[] audioClips;  // Array of audio clips to be played
    private AudioSource audioSource;  // Reference to the AudioSource component

    // Singleton pattern to ensure only one instance of the AudioManager exists
    private static AudioManager instance;
    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AudioManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = "AudioManager";
                    instance = obj.AddComponent<AudioManager>();
                }
            }
            return instance;
        }
    }

    void Awake()
    {
        // Ensure only one instance of the AudioManager exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Get reference to the AudioSource component
        audioSource = GetComponent<AudioSource>();
    }

    // Play an audio clip by name
    public void Play(string clipName)
    {
        AudioClip clip = GetAudioClip(clipName);
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("Audio clip '" + clipName + "' not found in AudioManager.");
        }
    }

    // Get an audio clip by name
    private AudioClip GetAudioClip(string clipName)
    {
        foreach (AudioClip clip in audioClips)
        {
            if (clip.name == clipName)
            {
                return clip;
            }
        }
        return null;
    }
}