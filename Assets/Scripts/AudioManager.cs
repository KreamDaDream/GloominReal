using UnityEngine;

public class AudioManager : MonoBehaviour
{

    [Header("Other")]
    public static AudioManager Instance;
    public float BackPos = 0;
    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource SFXSource;

    [Header("Audio Clips")]
    public AudioClip background;
    public AudioClip FIGHT;
    public AudioClip wind;
    public AudioClip mainMenu;

    public AudioClip slice;
    public AudioClip impact;
    public AudioClip hurt;
    public AudioClip summon;
    public AudioClip coin;
    public AudioClip notice;
    public AudioClip checkPoint;
    public AudioClip talk;
    public AudioClip pipe;
    public AudioClip openDoor;
    public AudioClip bite;





    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {

        
        musicSource.Play();

    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    public void ChangeMusic(AudioClip clip)
    {
        if (musicSource.clip == background)
        {
            BackPos = musicSource.time;
        }
        if (clip == null)
        {
            musicSource.Stop();
            musicSource.clip = null;
        }
        else
        {
            musicSource.clip = clip;
            musicSource.Play();
            if (musicSource.clip == background)
            {
                musicSource.time = BackPos;
            }
        }
    }
}
