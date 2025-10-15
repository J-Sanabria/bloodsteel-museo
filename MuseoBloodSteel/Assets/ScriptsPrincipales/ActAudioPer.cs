using UnityEngine;

public class ActAudioPer : MonoBehaviour
{
    public GazeAnimationController personaje;
    public AudioClip AudioClipInicio;
    private bool AudioActivado = false;
    public void OnPointerClickXR()
    {
        if (personaje != null && AudioClipInicio != null && AudioActivado == false)
        {
            personaje.ActivarDesdeOtroObjeto(AudioClipInicio);
            AudioActivado = true;
        }
    }
}
