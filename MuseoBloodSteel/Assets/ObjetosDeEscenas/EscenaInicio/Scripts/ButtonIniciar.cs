using UnityEngine;

public class ButtonIniciar : MonoBehaviour
{
    [Header("Configuración de Audio")]
    public AudioSource audioSource; // Fuente de audio

    [Header("Objeto a activar al finalizar el audio")]
    public GameObject objetoAActivar;

    public void OnPointerClickXR()
    {
        if (audioSource != null)
        {
            audioSource.Play();

            // Se oculta el botón principal inmediatamente
            gameObject.SetActive(false);

            // Como este objeto se desactiva, la corrutina no podría ejecutarse
            // Así que la pasamos a un "controlador" temporal
            GameObject controlador = new GameObject("AudioControllerTemp");
            controlador.AddComponent<AudioControllerTemp>().Iniciar(audioSource, objetoAActivar);
        }
        else
        {
            Debug.LogWarning("Falta asignar el AudioSource en " + gameObject.name);
        }
    }
}

public class AudioControllerTemp : MonoBehaviour
{
    private AudioSource audioSource;
    private GameObject objetoAActivar;

    public void Iniciar(AudioSource source, GameObject objeto)
    {
        audioSource = source;
        objetoAActivar = objeto;
        StartCoroutine(EsperarYActivar());
    }

    private System.Collections.IEnumerator EsperarYActivar()
    {
        yield return new WaitForSeconds(audioSource.clip.length);

        if (objetoAActivar != null)
            objetoAActivar.SetActive(true);

        Destroy(gameObject); // Limpia el controlador temporal
    }
}
