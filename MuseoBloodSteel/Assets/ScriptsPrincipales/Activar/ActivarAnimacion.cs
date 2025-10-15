using UnityEngine;

public class ActivarAnimacion : MonoBehaviour
{
    public ObjetoAnimado objetoAnimado;     // Objeto que se va a activar

    public Animation animacionPropia;       // Animación del propio control remoto
    public string nombreAnimacionPropia;    // Nombre de la animación del control remoto

    public AudioSource audioSourcePropio;   // Sonido del control remoto

    [Header("Objeto a desactivar al presionar")]
    public GameObject objetoADesactivar;    // Objeto que se desactiva al hacer click

    public void OnPointerClickXR()
    {
        // Reproduce la animación propia
        if (animacionPropia != null && !string.IsNullOrEmpty(nombreAnimacionPropia))
        {
            animacionPropia.Play(nombreAnimacionPropia);
        }

        // Reproduce sonido del control remoto
        if (audioSourcePropio != null)
        {
            audioSourcePropio.Play();
        }

        // Activa el otro objeto
        if (objetoAnimado != null)
        {
            objetoAnimado.ActivarDesdeOtroObjeto();
        }

        // Desactiva el objeto asignado
        if (objetoADesactivar != null)
        {
            objetoADesactivar.SetActive(false);
        }
    }
}
