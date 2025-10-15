using UnityEngine;

public class BaseObjtosInreactuables : MonoBehaviour
{
    private int contadorPiedras = 0; // Contador de piedras insertadas
    public GameObject objetoEspecial; // Objeto especial ya presente en la escena (debe estar desactivado)

    public void CargarCañon()
    {
        if (contadorPiedras < 3) // Evita sobrecargar el cañón
        {
            contadorPiedras++;
            Debug.Log("Piedras en el cañón: " + contadorPiedras);

            if (contadorPiedras == 3) // Si ya hay 3 piedras, se carga completamente
            {
                gameObject.tag = "Untagged"; // Cambia el tag para que pueda disparar
                ActivarObjetoEspecial();
            }
        }
    }

    private void ActivarObjetoEspecial()
    {
        if (objetoEspecial != null)
        {
            objetoEspecial.SetActive(true); // Activa el objeto en la escena
            Debug.Log("¡Objeto especial activado!");
        }
    }
}