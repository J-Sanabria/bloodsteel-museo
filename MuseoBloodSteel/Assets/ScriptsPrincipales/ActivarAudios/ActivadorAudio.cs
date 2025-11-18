using UnityEngine;
using System.Collections;

public class ActivadorAudio : MonoBehaviour
{
    [Header("Audio")]
    public AudioManager audioManager;
    public AudioClip audioClip;

    [Tooltip("Si está activado, el audio solo se reproducirá una vez.")]
    public bool reproducirUnaVez = false;

    [Header("Objetos y scripts")]
    public GameObject objetoParaActivar;     // Opcional
    public MonoBehaviour scriptRotacion;     // El script de rotación del personaje

    private bool yaReproducido = false;

    public void OnPointerClickXR()
    {
        ActivarObjeto();
        IniciarSecuenciaAudio();
    }

    // -------------------------------------------------------
    // ACTIVAR OBJETO
    // -------------------------------------------------------
    private void ActivarObjeto()
    {
        if (objetoParaActivar != null && !objetoParaActivar.activeSelf)
            objetoParaActivar.SetActive(true);
    }

    // -------------------------------------------------------
    // AUDIO + ROTACIÓN
    // -------------------------------------------------------
    private void IniciarSecuenciaAudio()
    {
        if (audioManager == null || audioClip == null)
        {
            Debug.LogWarning("Falta AudioManager o AudioClip en " + gameObject.name);
            return;
        }

        if (reproducirUnaVez && yaReproducido)
            return;

        // Reproducir audio
        audioManager.ReproducirSonido(audioClip);

        // Activar rotación
        if (scriptRotacion != null)
            scriptRotacion.enabled = true;

        // Iniciar coroutine para apagar la rotación cuando termine el audio
        StartCoroutine(DetenerRotacionDespuesDeAudio(audioClip.length));

        yaReproducido = true;
    }

    // -------------------------------------------------------
    // DESACTIVAR LA ROTACIÓN AUTOMÁTICAMENTE
    // -------------------------------------------------------
    private IEnumerator DetenerRotacionDespuesDeAudio(float duracion)
    {
        yield return new WaitForSeconds(duracion);

        if (scriptRotacion != null)
            scriptRotacion.enabled = false;
    }
}
