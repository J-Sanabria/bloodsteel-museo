using UnityEngine;
using UnityEngine.SceneManagement; // Solo si quieres cargar otra escena

public class AudioController : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audioSource; // Asigna el AudioSource que reproduce el audio

    [Header("Objeto a activar")]
    public GameObject objetoParaActivar; // Asigna el objeto que quieres activar

    [Header("Opcional: Cambio de escena")]
  
  

    private bool activado = false; // Evita que se active más de una vez

    void Update()
    {
        if (audioSource != null && !audioSource.isPlaying && !activado)
        {
            activado = true;

            if (objetoParaActivar != null)
                objetoParaActivar.SetActive(true);
        }
    }
}
