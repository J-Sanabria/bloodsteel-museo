using UnityEngine;
using System.Collections;

public class AnimationLetrero : MonoBehaviour
{
    public float scaleUpDuration = 1f;    // Tiempo para crecer
    public float holdTime = 5f;           // Tiempo que se queda grande
    public float scaleDownDuration = 0.3f; // Tiempo para encogerse
    public Vector3 targetScale = Vector3.one; // Escala final
    public bool loop = true;              // Repetir animación

    private Vector3 initialScale;

    void Start()
    {
        initialScale = Vector3.zero; // Empieza diminuto
        transform.localScale = initialScale;
        StartCoroutine(ScaleRoutine());
    }

    IEnumerator ScaleRoutine()
    {
        do
        {
            // Escalar hacia arriba
            yield return StartCoroutine(ScaleOverTime(initialScale, targetScale, scaleUpDuration));

            // Mantenerse grande
            yield return new WaitForSeconds(holdTime);

            // Encogerse
            yield return StartCoroutine(ScaleOverTime(targetScale, initialScale, scaleDownDuration));

        } while (loop);
    }

    IEnumerator ScaleOverTime(Vector3 startScale, Vector3 endScale, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.localScale = Vector3.Lerp(startScale, endScale, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localScale = endScale;
    }
}
