using UnityEngine;

public class PerAudio : MonoBehaviour
{
    public AudioSource audioSource;
    private bool audioActivo = false;
    private AudioClip clipActual; // Nuevo: Guarda el clip en curso


    public void OnPointerClickXR()
    {
        if (audioActivo == true)
        {
            audioSource.Stop();
        }
    }
    public void ActivarDesdeOtroObjeto(AudioClip nuevoClip)
    {
        if (audioActivo == true)
        {
            audioSource.Stop();
        }
        else
        {
            // Si ya está activo, reinicia con el nuevo audio
            ActivarDialogo(nuevoClip);
        }
    }

    private void ActivarDialogo(AudioClip clip)
    {
        clipActual = clip;


        if (audioSource != null && clipActual != null)
        {
            audioSource.clip = clipActual;
            audioSource.Play();
        }

        audioActivo = true;
    }
}
