using UnityEngine;

public class ActivadorDePersonaje : MonoBehaviour
{
    public GazeAnimationController personaje;

    public AudioClip Saludo;
    public AudioClip PorqueVuelves;
    public AudioClip Artefacto01;
    public AudioClip Artefacto02;
    public AudioClip Artefacto03;
    public AudioClip Artefacto04;

    private bool saludoActivado = false;
    private bool porqueVuelvesActivado = false;

    private void Start()
    {
        // Llama a la funci�n como si se hubiera hecho clic
        OnPointerClickXR();
    }

    private void Update()
    {
        // Reiniciar saludo y di�logo de "porque vuelves" si ya termin� de hablar
        if (personaje != null && !personaje.DialogoActivo)
        {
            saludoActivado = false;
            porqueVuelvesActivado = false;
        }
    }

    public void OnPointerClickXR()
    {
        if (personaje == null || InventarioManager.Instance == null) return;

        // Saludo inicial
        if (!InventarioManager.Instance.Inicio && !saludoActivado && Saludo != null)
        {
            personaje.ActivarDesdeOtroObjeto(Saludo);
            saludoActivado = true;
            return;
        }

        // Si ya inici� pero no est� vac�o y a�n no se ha activado "porque vuelves"
        if (InventarioManager.Instance.Inicio && !InventarioManager.Instance.Vacio && !porqueVuelvesActivado && PorqueVuelves != null)
        {
            personaje.ActivarDesdeOtroObjeto(PorqueVuelves);
            porqueVuelvesActivado = true;
            return;
        }

        // Activar artefactos recolectados
        ActivarArtefacto(1, InventarioManager.Instance.Artefacto01, ref InventarioManager.Instance.Artefacto01Activado, Artefacto01);
        ActivarArtefacto(2, InventarioManager.Instance.Artefacto02, ref InventarioManager.Instance.Artefacto02Activado, Artefacto02);
        ActivarArtefacto(3, InventarioManager.Instance.Artefacto03, ref InventarioManager.Instance.Artefacto03Activado, Artefacto03);
        ActivarArtefacto(4, InventarioManager.Instance.Artefacto04, ref InventarioManager.Instance.Artefacto04Activado, Artefacto04);
    }

    private void ActivarArtefacto(int numero, bool recolectado, ref bool activado, AudioClip clip)
    {
        if (recolectado && !activado && clip != null)
        {
            personaje.ActivarDesdeOtroObjeto(clip);
            activado = true;
            InventarioManager.Instance.Vacio = true;
        }
    }
}
