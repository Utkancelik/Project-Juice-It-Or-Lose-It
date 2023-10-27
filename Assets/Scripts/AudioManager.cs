using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioClip brickBreakSound;
    public AudioClip paddleHitSound;
    public AudioClip wallHitSound;
    public AudioClip powerUpSound;
    public AudioClip ballCreationSound;
    public AudioClip backgroundMusic;

    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PlayBackgroundMusic();
    }

    public void PlayBrickBreakSound()
    {
        audioSource.PlayOneShot(brickBreakSound);
    }

    public void PlayPaddleHitSound()
    {
        audioSource.PlayOneShot(paddleHitSound);
    }

    public void PlayWallHitSound()
    {
        audioSource.PlayOneShot(wallHitSound);
    }

    public void PlayPowerUpSound()
    {
        audioSource.PlayOneShot(powerUpSound);
    }

    public void PlayBallCreationSound()
    {
        audioSource.PlayOneShot(ballCreationSound);
    }

    public void PlayBackgroundMusic()
    {
        audioSource.clip = backgroundMusic;
        audioSource.loop = true;
        audioSource.Play();
    }
}
