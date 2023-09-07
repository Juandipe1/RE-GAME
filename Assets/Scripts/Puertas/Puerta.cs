using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puerta : MonoBehaviour
{
    public GameObject PivotPuerta;
    public AudioSource Abriendo;
    public AudioSource Cerrando;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PuertaAbriendo());
    }
    IEnumerator PuertaAbriendo()
    {
        yield return new WaitForSeconds(2.8f);
        Abriendo.Play();
        PivotPuerta.GetComponent<Animator>().Play("PuertaAnim");
        yield return new WaitForSeconds(4.8f);
        Cerrando.Play();
    }
}
