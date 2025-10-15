using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip audioInicio;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void Start()
    {
        if (audioInicio != null)
        {
            ReproducirSonido(audioInicio);
        }
    }

    public void ReproducirSonido(AudioClip clip)
    {
        if (clip != null)
        {
            // Si hay un audio sonando, detenerlo antes de reproducir uno nuevo
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }

            audioSource.clip = clip;
            audioSource.Play();
        }
    }

    public void Pausar()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
