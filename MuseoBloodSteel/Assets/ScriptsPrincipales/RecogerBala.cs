using System;
using UnityEngine;

public class RecogerBala : MonoBehaviour
{
    public Transform puntoMira; // Punto donde se ancla la bala (hijo de la cámara)
    private GameObject balaSujeta;

    //  Objeto que desaparecerá al recoger la bala
    public GameObject objetoADesaparecer;

    // Nuevo: Objeto que aparecerá al recoger la bala
    public GameObject objetoAparecer;

    void Update()
    {
        if (balaSujeta != null)
        {
            balaSujeta.transform.position = puntoMira.position;
            balaSujeta.transform.rotation = puntoMira.rotation;
        }
    }

    public void OnPointerClickXR()
    {
        if (balaSujeta == null)
        {
            Recoger();
        }
    }

    private void Recoger()
    {
        balaSujeta = gameObject;

        // 🔹 Si hay un objeto asignado, lo desactivamos
        if (objetoADesaparecer != null)
        {
            objetoADesaparecer.SetActive(false);
        }

        // 🔹 Si hay un objeto para aparecer, lo activamos
        if (objetoAparecer != null)
        {
            objetoAparecer.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider otro)
    {
        Debug.Log("Si colisiona");
        if (otro.CompareTag("Cañon"))
        {
            otro.GetComponent<BaseObjtosInreactuables>().CargarCañon();
            DesaparecerBala();
        }
    }

    private void DesaparecerBala()
    {
        if (balaSujeta != null)
        {
            balaSujeta = null;
            gameObject.SetActive(false);

            // 🔹 Al desaparecer la bala, ocultamos el objeto que había aparecido
            if (objetoAparecer != null)
            {
                objetoAparecer.SetActive(false);
            }
        }
    }
}
