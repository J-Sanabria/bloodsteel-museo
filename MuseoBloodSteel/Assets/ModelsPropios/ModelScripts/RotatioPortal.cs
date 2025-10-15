using UnityEngine;

public class RotateOnX : MonoBehaviour
{
    public float rotationSpeed = 30f; // Velocidad de rotación en grados por segundo

    void Update()
    {
        transform.Rotate(rotationSpeed * 0f, 0f,0.2f);
    }
}
