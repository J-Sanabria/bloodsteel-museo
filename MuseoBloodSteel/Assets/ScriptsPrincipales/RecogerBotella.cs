using UnityEngine;

public class RecogerBotella : MonoBehaviour
{
    public Transform puntoMira; // Punto donde se ancla la botella (hijo de la c�mara)
    private GameObject botellaSujeta;
    private bool estaSujeta = false; // Para saber si la botella est� en manos del jugador

    void Update()
    {
        if (botellaSujeta != null)
        {
            botellaSujeta.transform.position = puntoMira.position;
            botellaSujeta.transform.rotation = puntoMira.rotation;
        }
    }

    public void OnPointerClickXR() // Recoger la botella al mirar y hacer clic
    {
        if (!estaSujeta)
        {
            Recoger();
        }
    }

    private void Recoger()
    {
        botellaSujeta = gameObject;
        estaSujeta = true;
    }

    private void OnTriggerEnter(Collider otro)
    {
        if (otro.CompareTag("Barril") && estaSujeta) // Si se suelta en el barril
        {
            otro.GetComponent<Barril>().DepositarBotella(); // Notifica al barril
            DesaparecerBotella();
        }
    }

    private void DesaparecerBotella()
    {
        estaSujeta = false;
        botellaSujeta = null;
        Destroy(gameObject); // Destruye la botella despu�s de entregarla
    }
}