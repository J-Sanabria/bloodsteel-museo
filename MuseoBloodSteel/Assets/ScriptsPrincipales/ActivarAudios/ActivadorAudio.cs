using UnityEngine;

public class ActivadorAudio : MonoBehaviour
{
    public AudioManager audioManager;
    public AudioClip audioClip;

    [Tooltip("Si está activado, el audio solo se reproducirá una vez.")]
    public bool reproducirUnaVez = false;

    [Header("Objeto a activar")]
    public GameObject objetoParaActivar; // El objeto que inicia apagado

    private bool yaReproducido = false;

    public void OnPointerClickXR()
    {
        // Activar el objeto si está asignado
        if (objetoParaActivar != null)
        {
            objetoParaActivar.SetActive(true);
        }

        // Reproducir el audio si está asignado
        if (audioManager != null && audioClip != null)
        {
            if (reproducirUnaVez)
            {
                if (!yaReproducido)
                {
                    audioManager.ReproducirSonido(audioClip);
                    yaReproducido = true;
                }
            }
            else
            {
                audioManager.ReproducirSonido(audioClip);
            }
        }
        else
        {
            Debug.LogWarning("Falta asignar el AudioManager o el AudioClip en " + gameObject.name);
        }
    }
}
