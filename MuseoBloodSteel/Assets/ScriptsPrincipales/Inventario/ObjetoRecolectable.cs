using UnityEngine;

public class ObjetoRecolectable : MonoBehaviour
{
    public int numeroArtefacto;

    public void OnPointerClickXR()
    {
        if (numeroArtefacto >= 1 && numeroArtefacto <= 4)
        {
            InventarioManager.Instance.ActivarArtefacto(numeroArtefacto);

            gameObject.SetActive(false);
        }
    }
}
