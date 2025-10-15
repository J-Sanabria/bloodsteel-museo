using UnityEngine;

public class ActivadorExterno : MonoBehaviour
{
    public GazeAnimationController personaje;
    public AudioClip AudioClipInicio;
    public AudioClip Siempre;
    public AudioClip Artefacto01;// Asigna en el Inspector el clip que debe activar

    public void OnPointerClickXR()
    {
        if (personaje != null && Siempre != null )
            {
                personaje.ActivarDesdeOtroObjeto(Siempre);
            }

        if (personaje != null && AudioClipInicio != null && InventarioManager.Instance.Artefacto01 == false)
        {
            personaje.ActivarDesdeOtroObjeto(AudioClipInicio);
        }

        if (personaje != null && Artefacto01 != null && InventarioManager.Instance.Artefacto01 == true)
        {
            personaje.ActivarDesdeOtroObjeto(Artefacto01);
        }
    }
}