using UnityEngine;

public class AnimationArrow : MonoBehaviour
{
    [Header("Rotación")]
    public float velocidadRotacion = 50f; // grados por segundo

    [Header("Flotación")]
    public float amplitud = 0.25f; // altura máxima de movimiento
    public float velocidadFlotacion = 2f; // velocidad del movimiento arriba-abajo

    private Vector3 posicionInicial;

    void Start()
    {
        posicionInicial = transform.position;
    }

    void Update()
    {
        // Rotación en el eje Y
        transform.Rotate(Vector3.up, velocidadRotacion * Time.deltaTime, Space.World);

        // Movimiento flotante usando seno para suavidad
        float nuevoY = posicionInicial.y + Mathf.Sin(Time.time * velocidadFlotacion) * amplitud;
        transform.position = new Vector3(transform.position.x, nuevoY, transform.position.z);
    }
}
