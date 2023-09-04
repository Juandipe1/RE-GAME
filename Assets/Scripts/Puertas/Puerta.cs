using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puerta : MonoBehaviour
{
    public GameObject PivotPuerta;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PuertaAbriendo());
    }
    IEnumerator PuertaAbriendo()
    {
        yield return new WaitForSeconds(2.8f);
        PivotPuerta.GetComponent<Animator>().Play("PuertaAnim");
        yield return new WaitForSeconds(4);
    }
}
