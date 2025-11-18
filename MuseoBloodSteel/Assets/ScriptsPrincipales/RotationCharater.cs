using UnityEngine;

public class RotationCharater : MonoBehaviour
{
  
    public float velocidad = 30f;

    void Update()
    {
        transform.Rotate(0, velocidad * Time.deltaTime, 0);
    }
}
