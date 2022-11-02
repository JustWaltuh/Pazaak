using UnityEngine;

public class ForMusicScript : MonoBehaviour
{
    [SerializeField] private AudioClip[] music;
    [SerializeField] private AudioSource cantinaMusic;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        PlayMusic();
    }

    private void Update()
    {
        if (!cantinaMusic.isPlaying)
        {
            PlayMusic();
        }
    }

    private void PlayMusic()
    {
        int rndMusic = Random.Range(0, music.Length);
        cantinaMusic.PlayOneShot((music[rndMusic]));
    }
}
