using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class TelerporUI : MonoBehaviour
{
    [Header("Glow settings")]
    public Color glowColor = new Color(0.2f, 0.6f, 1f, 1f);
    [Tooltip("Multiplicador general de la emisión")]
    public float glowIntensity = 2f;
    [Tooltip("Velocidad del pulso")]
    public float pulseSpeed = 3f;
    [Tooltip("Cuánto afecta el pulso (0 = no pulso, 1 = pulso máximo)")]
    [Range(0f, 1f)]
    public float pulseAmount = 0.45f;
    [Tooltip("Velocidad de transición entrada/salida")]
    public float transitionSpeed = 5f;

    [Header("Shader property names (ajusta si tu shader usa otros nombres)")]
    public string crackColorProp = "_CrackColor";
    public string fresnelColorProp = "_FresnelColor";
    public string emissionProp = "_EmissionColor";
    public string baseColorProp = "_BaseColor";

    // Internals
    Renderer rend;
    Material mat; // instancia del material
    bool isGazed = false;
    float fade = 0f; // 0..1

    // Colores originales cacheados
    Color origCrackColor = Color.white;
    Color origFresnelColor = Color.black;
    Color origEmissionColor = Color.black;
    Color origBaseColor = Color.white;

    void Awake()
    {
        rend = GetComponent<Renderer>();
        if (rend == null)
        {
            Debug.LogError("[GazeGlow] No Renderer found on " + gameObject.name);
            enabled = false;
            return;
        }

        // Usa renderer.material para obtener una instancia única (no modifica SharedMaterial)
        mat = rend.material;

        // Cachear valores originales si existen
        if (mat.HasProperty(crackColorProp)) origCrackColor = mat.GetColor(crackColorProp);
        if (mat.HasProperty(fresnelColorProp)) origFresnelColor = mat.GetColor(fresnelColorProp);
        if (mat.HasProperty(emissionProp)) origEmissionColor = mat.GetColor(emissionProp);
        if (mat.HasProperty(baseColorProp)) origBaseColor = mat.GetColor(baseColorProp);
        else origBaseColor = mat.color; // fallback
    }

    void Update()
    {
        // Fade hacia 1 cuando es mirado, hacia 0 cuando no
        float target = isGazed ? 1f : 0f;
        fade = Mathf.MoveTowards(fade, target, Time.deltaTime * transitionSpeed);

        // Pulso solo si estamos mirando
        float pulse = isGazed ? (0.5f + 0.5f * Mathf.Sin(Time.time * pulseSpeed)) : 0f;

        // Colores que vamos a escribir en el material
        Color crackColorToSet = Color.Lerp(origCrackColor, glowColor, fade);
        Color fresnelToSet = Color.Lerp(origFresnelColor, glowColor * 0.75f, fade);
        Color baseToSet = Color.Lerp(origBaseColor, Color.Lerp(origBaseColor, glowColor, 0.25f * fade), fade);

        // Emission calculada (se multiplica por intensidad, fade, y pulso)
        float emissionFactor = fade * (1f + pulse * pulseAmount) * glowIntensity;
        Color emissionToSet = glowColor * emissionFactor;

        // Escribir propiedades si existen en el shader
        if (mat.HasProperty(crackColorProp)) mat.SetColor(crackColorProp, crackColorToSet);
        if (mat.HasProperty(fresnelColorProp)) mat.SetColor(fresnelColorProp, fresnelToSet);
        if (mat.HasProperty(baseColorProp)) mat.SetColor(baseColorProp, baseToSet);

        if (mat.HasProperty(emissionProp))
        {
            mat.SetColor(emissionProp, Color.Lerp(origEmissionColor, emissionToSet, fade));
            // En URP a veces ayuda habilitar la keyword de emission para que el editor muestre el brillo
            mat.EnableKeyword("_EMISSION");
        }
    }

    // Métodos a llamar desde tu sistema de gaze
    public void OnPointerEnterXR()
    {
        isGazed = true;
    }

    public void OnPointerExitXR()
    {
        isGazed = false;
    }

    // Alternativa directa
    public void SetGazed(bool gazing)
    {
        isGazed = gazing;
    }
}
