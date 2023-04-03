using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanzarPiedra : MonoBehaviour
{

    [SerializeField] float CD_lanzarpiedra;

    [SerializeField] GameObject piedra;

    private GameObject target;

    [SerializeField] Transform lugarLanzamientoPiedras;

    private bool puedeLanzarPiedra = true;

    [SerializeField] private float AnguloDispersion;
    

    private void Start()
    {
        target = GameObject.Find("Amalgama");
    }
    // Update is called once per frame
    void Update()
    {
        if (puedeLanzarPiedra) { 
            StartCoroutine(LanzarPiedraAction()); 
        }
    }

    private IEnumerator LanzarPiedraAction()
    {
        puedeLanzarPiedra = false;

        float anguloRadianes = Mathf.Atan2(target.transform.position.y - transform.position.y, target.transform.position.x - transform.position.x);
        float anguloGrados = (180 / Mathf.PI) * anguloRadianes - 90;
        int alfasigno = Random.Range(0, 2);
        float desfase;
        if (alfasigno == 0) { desfase = Random.Range(0f, AnguloDispersion) * 1; }
        else { desfase = Random.Range(0f, AnguloDispersion) * -1; }
        transform.rotation = Quaternion.Euler(0, 0, anguloGrados + desfase);

        Instantiate(piedra, lugarLanzamientoPiedras.position, transform.rotation);

        yield return new WaitForSeconds(CD_lanzarpiedra);
        puedeLanzarPiedra = true;
    }
}
