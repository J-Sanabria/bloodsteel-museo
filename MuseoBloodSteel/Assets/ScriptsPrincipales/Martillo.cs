using System;
using UnityEngine;

public class Martillo : MonoBehaviour
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
