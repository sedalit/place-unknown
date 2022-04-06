using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instanse;

    public AudioClip DoorOpened;
    public AudioClip DoorClosed;
    public AudioClip ButtonUI;

    [SerializeField] private AudioClip questStarted;
    [SerializeField] private AudioClip carExplosion;
    [SerializeField] private AudioClip openPanelUI;

    private AudioSource audioSource;

    public AudioClip QuestStarted => questStarted;
    public AudioClip CarExplosion => carExplosion;
    public AudioClip OpenPanelUI => openPanelUI;

    private void Awake()
    {
        if (Instanse != null)
        {
            Destroy(this);
            return;
        }
        Instanse = this;
        DontDestroyOnLoad(this);
    }

    private void Start() => audioSource = GetComponent<AudioSource>();

    public void Play(AudioClip audio)
    {
        audioSource.loop = false;
        audioSource.clip = audio;
        audioSource.Play();
    }

    public void PlayOneShot(AudioClip audio)
    {
        audioSource.PlayOneShot(audio);
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }
}
