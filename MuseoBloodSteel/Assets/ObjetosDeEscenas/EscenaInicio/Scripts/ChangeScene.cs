using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene: MonoBehaviour
{
    [Header("Configuración de Escena")]
    public string nombreEscena; // Nombre exacto de la escena a cargar

    public void OnPointerClickXR()
    {
        if (!string.IsNullOrEmpty(nombreEscena))
        {
            SceneManager.LoadScene(nombreEscena);
        }
        else
        {
            Debug.LogWarning("No se ha asignado un nombre de escena en " + gameObject.name);
        }
    }
}
