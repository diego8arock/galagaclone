using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverAvion : MonoBehaviour {
    
    public float velocidad = 2f;
    public float limiteXPos = 4.3f;
    public float limiteXNeg = -10f;
    

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 posicion = transform.position + new Vector3(Input.GetAxis("Horizontal"), 0, 0) * velocidad * Time.deltaTime;
        if (posicion.x >= limiteXNeg && posicion.x <= limiteXPos)
        {
            transform.position = posicion;
        }
    }


    void LateUpdate()
    {

    }

    void OnDestroy()
    {

    }

}
