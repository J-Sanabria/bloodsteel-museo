using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CambioEscenaGaze : MonoBehaviour
{
    [SerializeField] private string escenaDestino = "EscenaPrueba01";

    public void OnPointerClickXR()
    {
        if (SceneManager.GetActiveScene().name == "Tutorial")
        {
             SceneManager.LoadScene(escenaDestino);
            return;
        }

        if (!InventarioManager.Instance.Inicio)
            InventarioManager.Instance.Inicio = true;

       SceneManager.LoadScene(escenaDestino);
    }

}
