using UnityEngine;

public class InventarioManager : MonoBehaviour
{
    public static InventarioManager Instance;

    public bool Artefacto01 { get; private set; } = false;
    public bool Artefacto02 { get; private set; } = false;
    public bool Artefacto03 { get; private set; } = false;
    public bool Artefacto04 { get; private set; } = false;

    public bool Inicio = false;
    public bool Artefacto01Activado = false;
    public bool Artefacto02Activado = false;
    public bool Artefacto03Activado = false;
    public bool Artefacto04Activado = false;

    public bool UnArtefacto = false;
    public bool DosArtefactos = false;
    public bool TercerArtefacto = false;
    public bool Win = false;

    public int cantidadArtefactos = 0;

    public bool Vacio = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ActivarArtefacto(int numeroArtefacto)
    {
        Vacio = true;
        switch (numeroArtefacto)
        {

            case 1:
                if (!Artefacto01)
                {
                    Artefacto01 = true;
                    cantidadArtefactos++;
                    Debug.Log("Se recolectó el primer artefacto01");
                }
                break;
            case 2:
                if (!Artefacto02)
                {
                    Artefacto02 = true;
                    cantidadArtefactos++;
                }
                break;
            case 3:
                if (!Artefacto03)
                {
                    Artefacto03 = true;
                    cantidadArtefactos++;
                }
                break;
            case 4:
                if (!Artefacto04)
                {
                    Artefacto04 = true;
                    cantidadArtefactos++;
                }
                break;
            default:
                Debug.LogWarning("Número de artefacto inválido.");
                return;
        }

        ActualizarProgreso();
    }

    private void ActualizarProgreso()
    {
        UnArtefacto = (cantidadArtefactos == 1);
        DosArtefactos = (cantidadArtefactos == 2);
        TercerArtefacto = (cantidadArtefactos == 3);
        Win = (cantidadArtefactos == 4);

        Debug.Log($"Progreso actualizado: {cantidadArtefactos} artefactos recolectados.");
    }

    public int CantidadArtefactos
    {
        get
        {
            int count = 0;
            if (Artefacto01) count++;
            if (Artefacto02) count++;
            if (Artefacto03) count++;
            if (Artefacto04) count++;
            return count;
        }
    }
}