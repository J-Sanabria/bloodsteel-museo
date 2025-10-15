using UnityEngine;

public class SalirDelJuego : MonoBehaviour
{
    public void OnPointerClickXR()
    {
        Debug.Log("Saliendo del juego...");

        // Cierra la aplicaci�n
        Application.Quit();

        // Si est�s en el editor de Unity, tambi�n detiene la ejecuci�n
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
