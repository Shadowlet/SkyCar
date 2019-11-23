using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorMenu : MonoBehaviour
{
    public Slider bodyColorSlider;
    public Slider wheelColorSlider;

    //private Renderer colorRend;

    public Button startButton;

    private float bodyValue = 0.03f;
    private float wheelValue = 0.08f;
    

    void Start()
    {
        //colorRend = GetComponent<Renderer>();
        bodyColorSlider.minValue = 0.1f;
        bodyColorSlider.maxValue = 1.0f;
        bodyColorSlider.value = bodyValue;

        wheelColorSlider.minValue = 0.1f;
        wheelColorSlider.maxValue = 1.0f;
        wheelColorSlider.value = wheelValue;
    }

    public void ChangeColor()
    {
        bodyValue = bodyColorSlider.value;
        wheelValue = wheelColorSlider.value;
        //colorRend.material.SetColor("CarColor", new Color(bodyValue, wheelValue, 1));
    }

}
