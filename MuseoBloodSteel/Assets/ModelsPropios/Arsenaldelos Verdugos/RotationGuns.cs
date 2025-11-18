using UnityEngine;

public class RotationGuns : MonoBehaviour
{
    // --- Parámetros de Rotación ---
    [Header("Rotación 360 Grados")]
    [Tooltip("Velocidad de rotación en grados por segundo.")]
    public float velocidadGiro = 50f;

    // --- Parámetros de Levitación (Eje Y) ---
    [Header("Movimiento de Levitación (Eje Y)")]
    [Tooltip("Distancia máxima que el objeto se moverá hacia arriba y abajo.")]
    public float amplitudLevitacion = 0.5f; 
    [Tooltip("Velocidad con la que el objeto sube y baja (frecuencia).")]
    public float velocidadLevitacion = 2f; 

    // Almacena la posición inicial Y para calcular la oscilación
    private float posicionYInicial;

    void Start()
    {
        // Guardamos la posición Y inicial del objeto al comienzo del juego
        posicionYInicial = transform.position.y;
    }

    void Update()
    {
        // 1. Rotación 360 Grados (Giro Constante)
        
        // Multiplicamos la velocidad por Time.deltaTime para hacerla independiente de la tasa de frames
        // Utilizamos Space.Self para girar sobre su propio eje.
        transform.Rotate(Vector3.up, velocidadGiro * Time.deltaTime, Space.Self);

        // 2. Movimiento de Levitación (Oscilación en Y)
        
        // Utilizamos la función seno (Mathf.Sin) para crear un movimiento suave y cíclico.
        // El tiempo (Time.time) se usa como la entrada cambiante para la función sinusoidal.
        float nuevoY = posicionYInicial + Mathf.Sin(Time.time * velocidadLevitacion) * amplitudLevitacion;

        // Actualizamos la posición Y del objeto, manteniendo X y Z.
        transform.position = new Vector3(transform.position.x, nuevoY, transform.position.z);
    }
}