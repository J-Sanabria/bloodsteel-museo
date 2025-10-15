using UnityEngine;

public class ObjetoAnimado : MonoBehaviour
{
    public Animation animador;              // Componente Animation
    public string nombreAnimacion;         // Nombre de la animación a reproducir
    public bool yaSeAnimo = false;

    public AudioSource audioSource;        // Componente de audio con el sonido del objeto

    public void ActivarDesdeOtroObjeto()
    {
        if (animador != null && !yaSeAnimo)
        {
            animador.Play(nombreAnimacion);    // Reproduce la animación
            if (audioSource != null)
            {
                audioSource.Play();            // Reproduce el sonido asociado
            }
            yaSeAnimo = true;
        }
    }
}
