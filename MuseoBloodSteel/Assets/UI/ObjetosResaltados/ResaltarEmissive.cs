using UnityEngine;

public class ResaltarEmmisive : MonoBehaviour
{
    [Header("Material a modificar")]
    public Renderer objetoRenderer; // Renderer que contiene el material
    public Color colorEmissive = Color.white; // Color del emissive
    public float velocidad = 2f; // Velocidad del parpadeo
    public float intensidadMax = 2f; // Intensidad máxima

    private Material materialInstancia;

    void Start()
    {
        // Creamos una instancia para no modificar el material global
        materialInstancia = objetoRenderer.material;

        // Activar emission en el material
        materialInstancia.EnableKeyword("_EMISSION");
    }

    void Update()
    {
        // Calcula la intensidad usando PingPong
        float intensidad = Mathf.PingPong(Time.time * velocidad, intensidadMax);

        // Aplica el color con intensidad
        materialInstancia.SetColor("_EmissionColor", colorEmissive * intensidad);
    }
}
