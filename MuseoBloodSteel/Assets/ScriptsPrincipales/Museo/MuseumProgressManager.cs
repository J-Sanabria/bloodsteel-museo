using UnityEngine;

public class MuseumProgressManager : MonoBehaviour
{
    public static MuseumProgressManager Instance; // Singleton global

    [Header("Progreso general")]
    public int counter = 0;             // contador de piezas vistas
    public bool debugLog = true;        // para ver los cambios en consola

    [Header("Umbrales de activación")]
    public int triggerAnim1 = 3;
    public int triggerAnim2 = 6;
    public int triggerAudio = 9;

    [Header("Referencias de activación")]
    public Animator anim1;              // animación 1 (puerta, compuerta, etc.)
    public Animator anim2;              // animación 2 (otra zona o vitrina)
    public AudioSource specialAudio;    // audio final o evento sonoro

    private bool anim1Triggered = false;
    private bool anim2Triggered = false;
    private bool audioTriggered = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    /// <summary>
    /// Llama este método desde OnPointerClickXR() en tus objetos del museo
    /// </summary>
    public void AddCount()
    {
        counter++;

        if (debugLog)
            Debug.Log($"[Museum] Piezas vistas: {counter}");

        CheckProgress();
    }

    private void CheckProgress()
    {
        // --- Primer evento ---
        if (!anim1Triggered && counter >= triggerAnim1)
        {
            if (anim1)
            {
                anim1.SetTrigger("Activate"); // usa un trigger llamado "Activate"
                if (debugLog) Debug.Log("Evento 1: Animación 1 activada");
            }
            anim1Triggered = true;
        }

        // --- Segundo evento ---
        if (!anim2Triggered && counter >= triggerAnim2)
        {
            if (anim2)
            {
                anim2.SetTrigger("Activate");
                if (debugLog) Debug.Log("Evento 2: Animación 2 activada");
            }
            anim2Triggered = true;
        }

        // --- Tercer evento (audio) ---
        if (!audioTriggered && counter >= triggerAudio)
        {
            if (specialAudio)
            {
                specialAudio.Play();
                if (debugLog) Debug.Log("Evento 3: Audio activado");
            }
            audioTriggered = true;
        }
    }
}

