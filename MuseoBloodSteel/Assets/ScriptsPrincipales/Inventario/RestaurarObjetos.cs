using UnityEngine;

public class RestaurarObjetos : MonoBehaviour
{
    public GameObject artefacto01;
    public GameObject artefacto02;
    public GameObject artefacto03;
    public GameObject artefacto04;
    public GameObject PuertaOeste;
    public GameObject PuertaEspacio;
    public GameObject Premio;
    public GameObject Puerta;
    public GameObject TimeMachine;

    private void Start()
    {
        if (InventarioManager.Instance.Artefacto01)
        {
            artefacto01.SetActive(true);
            PuertaOeste.SetActive(true);
            PuertaEspacio.SetActive(false);
        }

        if (InventarioManager.Instance.Artefacto02)
        {
            artefacto02.SetActive(true);
            PuertaOeste.SetActive(false);
        }

        if (InventarioManager.Instance.Artefacto03)
            artefacto03.SetActive(true);

        if (InventarioManager.Instance.Artefacto04)
            artefacto04.SetActive(true);

        if (InventarioManager.Instance.Win)
        {
            // Cambiar el tag de la máquina del tiempo
            Premio.SetActive(true);
            TimeMachine.tag = "Interactable";
            Debug.Log("¡Has ganado! Se activan el pastel, la puerta y la máquina del tiempo es interactuable.");
        }
        else
        {
            Debug.Log("Aún no has ganado. Faltan artefactos.");
        }
    }

}
