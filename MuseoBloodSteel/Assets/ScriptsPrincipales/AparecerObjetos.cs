using UnityEngine;

public class AparecerObjetos : MonoBehaviour
{
    public GameObject Coleccion01;
    public GameObject Coleccion02;

    public void OnPointerClickXR()
    {
        Coleccion01.SetActive(false);
        Coleccion02.SetActive(true);
    }
}