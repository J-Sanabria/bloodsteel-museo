using UnityEngine;

public class MuseumPiece : MonoBehaviour
{
    public void OnPointerClickXR()
    {
        // Aqu� se incrementa el contador global
        MuseumProgressManager.Instance.AddCount();
    }
}
