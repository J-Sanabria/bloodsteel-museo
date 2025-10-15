using UnityEngine;

public class FinalMaquina : MonoBehaviour
{
    [Header("Animación del objeto actual")]
    public Animation animObjeto;          
    public string nombreAnimObjeto;       

    [Header("Personaje")]
    public GazeAnimationController personaje;  
    public AudioClip AudioClipInicio;          
    private bool AudioActivado = false;

    [Header("Objetos a controlar")]
    public GameObject objetoA;
    public GameObject Reloj;         
    public GameObject objetoB;        
    public GameObject objetoFinal;    

    [Header("Nuevo Audio")]
    public AudioSource audioSourceExtra;    // Fuente para el segundo audio
    public AudioClip audioExtra;            // Clip del segundo audio
    public bool reproducirInmediatamente = false; // ✅ Si es true, suena al mismo tiempo

    public float delayExtra = 0.5f;   

    public void OnPointerClickXR()
    {
        if (!AudioActivado && personaje != null && AudioClipInicio != null)
        {

             gameObject.tag = "Untagged";
             
            // 1. Activar animación en este objeto
            if (animObjeto != null && !string.IsNullOrEmpty(nombreAnimObjeto))
            {
                animObjeto.Play(nombreAnimObjeto);
            }

            // 2. Activar personaje con su animación + audio
            personaje.ActivarDesdeOtroObjeto(AudioClipInicio);

            // 3. Marcar que ya se activó
            AudioActivado = true;

            // 4. Ocultar inmediatamente Objeto A y el Reloj
            if (objetoA != null) objetoA.SetActive(false);
            if (Reloj != null) Reloj.SetActive(false);

            // 5. Si el audio extra debe sonar al mismo tiempo
            if (reproducirInmediatamente && audioSourceExtra != null && audioExtra != null)
            {
                audioSourceExtra.clip = audioExtra;
                audioSourceExtra.Play();
            }

            // 6. Calcular tiempo máximo entre animación y audio del personaje
            float tiempoAnimObjeto = (animObjeto != null && animObjeto[nombreAnimObjeto] != null) 
                ? animObjeto[nombreAnimObjeto].length 
                : 0f;

            float tiempoAudio = AudioClipInicio.length;
            float tiempoMax = Mathf.Max(tiempoAnimObjeto, tiempoAudio);

            // 7. Si el audio extra debe sonar al final
            if (!reproducirInmediatamente && audioSourceExtra != null && audioExtra != null)
            {
                Invoke(nameof(ReproducirAudioExtra), tiempoMax);
            }

            // 8. Ocultar Objeto B y activar el objetoFinal después del tiempo
            Invoke(nameof(AccionesFinales), tiempoMax + delayExtra);
        }
    }

    private void ReproducirAudioExtra()
    {
        if (audioSourceExtra != null && audioExtra != null)
        {
            audioSourceExtra.clip = audioExtra;
            audioSourceExtra.Play();
        }
    }

    private void AccionesFinales()
    {
        if (objetoB != null) objetoB.SetActive(false);
        if (objetoFinal != null) objetoFinal.SetActive(true);
    }
}
