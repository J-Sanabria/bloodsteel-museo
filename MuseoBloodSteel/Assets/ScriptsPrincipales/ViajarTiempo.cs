using UnityEngine;
using System.Collections;

public class TimeSwitcher : MonoBehaviour
{
    [Header("Configuración de Tiempo")]
    public GameObject pasado;
    public GameObject futuro;
    private bool enPasado = true;

    [Header("Configuración de Rotación")]
    public Transform cameraTransform;
    public float alturaFija = 1.5f;
    public float distanciaFrontal = 1.0f;

    private Vector3 initialRotation;

    [Header("Transición")]
    public Animator fadeAnimator; // Arrástralo desde el inspector
    public float tiempoDeFade = 1f; // Duración del fade (puedes ajustarlo)

    void Start()
    {
        initialRotation = transform.eulerAngles;
        ActualizarTiempo();
    }

    void LateUpdate()
    {
        Vector3 direccionHorizontal = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up).normalized;
        Vector3 nuevaPosicion = cameraTransform.position + (direccionHorizontal * distanciaFrontal);
        nuevaPosicion.y = cameraTransform.position.y + alturaFija;
        transform.position = nuevaPosicion;

        Vector3 rotacionCamaraY = cameraTransform.eulerAngles;
        transform.eulerAngles = new Vector3(
            initialRotation.x,
            rotacionCamaraY.y,
            initialRotation.z
        );
    }

    public void OnPointerClickXR()
    {
        StartCoroutine(CambiarTiempoConFade());
    }

    private IEnumerator CambiarTiempoConFade()
    {
        if (fadeAnimator != null)
        {
            fadeAnimator.SetTrigger("FadeOut");
            yield return new WaitForSeconds(tiempoDeFade);
        }

        enPasado = !enPasado;
        ActualizarTiempo();

        if (fadeAnimator != null)
        {
            fadeAnimator.SetTrigger("FadeIn");
        }
    }

    private void ActualizarTiempo()
    {
        pasado.SetActive(enPasado);
        futuro.SetActive(!enPasado);
    }
}
