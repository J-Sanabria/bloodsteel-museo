using UnityEngine;

public class GazeAnimationController : MonoBehaviour
{
    public Animator animator;
    public AudioSource audioSource;

    private bool animacionActiva = false;
    private AudioClip clipActual;
    private float tiempoRestante = 0f;

    public bool DialogoActivo => animacionActiva;

    public void OnPointerClickXR()
    {
        // Si está sonando, se detiene el diálogo
        if (animacionActiva)
        {
            VolverAReposo();
        }
        // Si ya no está hablando, se vuelve a reproducir el último clip si existe
        else if (clipActual != null)
        {
            ActivarDialogo(clipActual);
        }
    }

    public void ActivarDesdeOtroObjeto(AudioClip nuevoClip)
    {
        // Si es un nuevo clip, cámbialo
        if (nuevoClip != clipActual)
        {
            VolverAReposo();
            ActivarDialogo(nuevoClip);
        }
        else
        {
            // Si es el mismo clip que estaba pausado, simplemente lo reactiva
            if (!animacionActiva)
            {
                ActivarDialogo(nuevoClip);
            }
        }
    }

    private void ActivarDialogo(AudioClip clip)
    {
        clipActual = clip;

        if (animator != null)
        {
            animator.SetTrigger("Hablar");
            animator.SetBool("AudioActivo", true);
        }

        if (audioSource != null && clipActual != null)
        {
            audioSource.clip = clipActual;
            audioSource.Play();
            CancelInvoke(nameof(VolverAReposo));
            Invoke(nameof(VolverAReposo), clipActual.length);
        }

        animacionActiva = true;
    }

    private void VolverAReposo()
    {
        if (animator != null)
        {
            animator.SetBool("AudioActivo", false);
        }

        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        animacionActiva = false;
    }
}
