using UnityEngine;

public class AudioLoopInicio : MonoBehaviour
{
    public AudioSource audioSource; // Asigna el AudioSource en el inspector
    private bool isPlayingLoop = true; // Controla si el loop sigue activo
    private Coroutine loopCoroutine;

    void Start()
    {
        if (audioSource != null)
        {
            loopCoroutine = StartCoroutine(PlayAudioWithPause());
        }
    }

    // Este método se llama cuando haces OnPointerClick en el objeto
    public void OnPointerClickXR()
    {
        if (isPlayingLoop)
        {
            isPlayingLoop = false; // Detenemos el loop
            if (loopCoroutine != null)
            {
                StopCoroutine(loopCoroutine);
            }
            audioSource.Stop();
        }
        else
        {
            isPlayingLoop = true; // Reactivamos el loop
            loopCoroutine = StartCoroutine(PlayAudioWithPause());
        }
    }

    private System.Collections.IEnumerator PlayAudioWithPause()
    {
        while (isPlayingLoop)
        {
            audioSource.Play(); // Reproduce el audio
            yield return new WaitForSeconds(audioSource.clip.length); // Espera a que termine
            yield return new WaitForSeconds(1f); // Pausa de 1 segundo antes del próximo loop
        }
    }
}
