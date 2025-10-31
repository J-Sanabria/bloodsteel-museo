using UnityEngine;

public class MuseumPiece : MonoBehaviour
{
    public void OnPointerClickXR()
    {
        // Aquí se incrementa el contador global
        MuseumProgressManager.Instance.AddCount();
    }
}
