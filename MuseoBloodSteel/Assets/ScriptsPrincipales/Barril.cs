using UnityEngine;

public class Barril : MonoBehaviour
{
    public GameObject cofreCerrado;  // Cofre que aparece inicialmente
    public GameObject cofreAbierto;  // Cofre que aparecerá cuando se entreguen las 3 botellas
    public GameObject objetoRecompensa; // Objeto que aparece encima del cofre
    public GameObject SiCofre;
    private int botellasRecogidas = 0; // Contador de botellas entregadas

    public void DepositarBotella()
    {
        botellasRecogidas++;

        Debug.Log("✅ Botella entregada. Total: " + botellasRecogidas);

        // Si ya se entregaron las 3 botellas, cambiar cofres
        if (botellasRecogidas >= 3)
        {
            CambiarCofre();
        }
    }

    private void CambiarCofre()
    {
        Debug.Log("🔓 ¡Todas las botellas entregadas! Se abre el cofre.");

        if (cofreCerrado != null)
        {
            cofreCerrado.SetActive(false);  // Desactiva el cofre cerrado
        }

        if (cofreAbierto != null)
        {
            cofreAbierto.SetActive(true);   // Activa el cofre abierto
        }
        if (objetoRecompensa != null)
        {
            objetoRecompensa.SetActive(true); // Activa el objeto encima del cofre
        }

   
    }
}