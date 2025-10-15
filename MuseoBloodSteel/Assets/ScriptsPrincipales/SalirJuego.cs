using UnityEngine;

public class SalirDelJuego : MonoBehaviour
{
    public void OnPointerClickXR()
    {
        Debug.Log("Saliendo del juego...");

        // Cierra la aplicación
        Application.Quit();

        // Si estás en el editor de Unity, también detiene la ejecución
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
