using System;
using UnityEngine;

public class Llave : MonoBehaviour
{
    public Transform puntoMira; // Punto donde se ancla el martillo (hijo de la cámara)
    private GameObject MartilloSujeto;
    private Transform player; // Referencia al Player

    void Start()
    {
        player = GameObject.FindWithTag("Player")?.transform; // Busca al Player por su tag
    }

    void Update()
    {
        if (MartilloSujeto != null)
        {
            MartilloSujeto.transform.position = puntoMira.position;
            MartilloSujeto.transform.rotation = puntoMira.rotation;
        }
    }

    public void OnPointerClickXR()
    {
        if (MartilloSujeto == null)
        {
            Recoger();
        }
    }

    private void OnTriggerEnter(Collider otro)
    {
        if (otro.CompareTag("Cañon"))
        {
            otro.GetComponent<Cañon>().CargarCañon();
            DesaparecerBala();
        }
    }

    private void DesaparecerBala()
    {
        if (MartilloSujeto != null)
        {

            MartilloSujeto = null;

            // Reactivar el gaze pointer al soltar la bala
            
            Destroy(gameObject);
        }
    }

    private void Recoger()
    {
        if (player != null)
        {
            MartilloSujeto = gameObject;
            MartilloSujeto.transform.SetParent(player); // Emparenta al Player
            MartilloSujeto.transform.localPosition = puntoMira.localPosition;
            MartilloSujeto.transform.localRotation = puntoMira.localRotation;
        }
        else
        {
            Debug.LogWarning("No se encontró el Player en la escena.");
        }
    }
}