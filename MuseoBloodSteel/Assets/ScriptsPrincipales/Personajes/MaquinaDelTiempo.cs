using UnityEngine;

public class MaquinaDelTiempo : MonoBehaviour
{
    public GazeAnimationController personaje;
    public AudioClip Incio;
    public AudioClip Artefacto01;
    public AudioClip Artefacto02;
    public AudioClip Artefacto03;
    public AudioClip Win;

    public void OnPointerClickXR()
    {
        if (personaje == null)
        {
            Debug.LogWarning("No se ha asignado el personaje.");
            return;
        }

        int cantidadArtefactos = InventarioManager.Instance.cantidadArtefactos;

        bool InicioInventario = InventarioManager.Instance.Inicio;

        if (InicioInventario == false)
        {
            personaje.ActivarDesdeOtroObjeto(Incio);
            Debug.Log("Audio: Inicio");
        }
        else
        {
            switch (cantidadArtefactos)
            {
                case 0:
                    if (Incio != null)
                    {
                        personaje.ActivarDesdeOtroObjeto(Incio);
                        Debug.Log("Audio: Inicio");
                    }
                    break;

                case 1:
                    personaje.ActivarDesdeOtroObjeto(Artefacto01);
                    InventarioManager.Instance.Artefacto01Activado = true;
                    Debug.Log("Audio: un solo artefacto");
                    break;

                case 2:
                    personaje.ActivarDesdeOtroObjeto(Artefacto02);
                    InventarioManager.Instance.Artefacto02Activado = true;
                    Debug.Log("Audio: Artefacto 02");
                    break;

                case 3:
                    personaje.ActivarDesdeOtroObjeto(Artefacto03);
                    InventarioManager.Instance.Artefacto03Activado = true;
                    Debug.Log("Audio: Artefacto 03");
                    break;

                case 4:
                    personaje.ActivarDesdeOtroObjeto(Win);
                    InventarioManager.Instance.Win = true;
                    Debug.Log("Audio: Win");
                    break;
            }
        }
           
    }
}
