using UnityEngine;

public class Cañon : MonoBehaviour
{
    public GameObject objetoEspecial;
    public void CargarCañon()
    {
        gameObject.tag = "Untagged"; // Cambia el tag a "Untagget" al cargar
        ActivarObjetoEspecial();
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