using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CambioEscenaGaze : MonoBehaviour
{
    [SerializeField] private string escenaDestino = "EscenaPrueba01";

    public void OnPointerClickXR()
    {
 
       SceneManager.LoadScene(escenaDestino);
    }

}
