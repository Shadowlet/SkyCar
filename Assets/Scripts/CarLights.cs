using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarLights : MonoBehaviour
{
    public GameObject lights;
    private Light carLights;
    // Start is called before the first frame update
    void Start()
    {
        carLights = lights.GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TurnRed()
    {
        carLights.color = Color.red;
    }
    public void TurnWhite()
    {
        carLights.color = Color.white;
    }
}
