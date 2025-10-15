using UnityEngine;
using System.Collections;

public class Cofre : MonoBehaviour
{
    public Transform puntoA;
    public Transform puntoB;
    public float velocidad = 2f;

    private bool moviendoHaciaA = true;

    public void MoverCofre()                      
    {
        StartCoroutine(MoverEntrePuntos());
    }

    IEnumerator MoverEntrePuntos()
    {
        Transform objetivo = moviendoHaciaA ? puntoA : puntoB;
        moviendoHaciaA = !moviendoHaciaA;

        while (Vector3.Distance(transform.position, objetivo.position) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, objetivo.position, velocidad * Time.deltaTime);
            yield return null;
        }

        Debug.Log("✅ Cofre llegó a su destino.");
    }
}
