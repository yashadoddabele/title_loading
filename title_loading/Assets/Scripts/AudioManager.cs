using UnityEngine;

// Singleton class to handle audio management
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioSource loopingSfxSource;  // For looping sounds (running, etc.)
    public AudioSource oneShotSfxSource;  // For one-shot effects (jump, pickup, etc.)
    public AudioSource musicSource;       // Already set for background music
    public AudioClip jumpClip;
    public AudioClip dyingClip;
    public AudioClip enemyClip;
    public AudioClip clearClip;
    public AudioClip backgroundMusicClip;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            AudioSource[] sources = GetComponents<AudioSource>();
            // Assign each audio source from inspector to a designated purpose
            if (sources.Length >= 2)
            {
                musicSource = sources[0];
                oneShotSfxSource = sources[1];
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Play background music on its dedicated audiosource
        if (backgroundMusicClip != null)
        {
            musicSource.clip = backgroundMusicClip;
            musicSource.loop = true;
            musicSource.Play();
        }
        else
        {
            Debug.LogWarning("Bg music clip not assigned");
        }
    }

    // One-shot sound effects
    public void JumpSound()
    {
        oneShotSfxSource.PlayOneShot(jumpClip);
    }

    public void ClearSound()
    {
        oneShotSfxSource.PlayOneShot(clearClip);
    }

    public void DyingSound() {
        oneShotSfxSource.PlayOneShot(dyingClip);
    }

    public void EnemySound() {
        oneShotSfxSource.PlayOneShot(enemyClip);
    }
}
