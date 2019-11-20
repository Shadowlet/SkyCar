using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUp : MonoBehaviour
{
    private Rocket rocket;
    private SpeedBoost speedBoost;

    public Image itemImage;
    public Image rocketImage;
    public Image boostImage;

    // Start is called before the first frame update
    void Start()
    {
        rocket = GetComponent<Rocket>();
        speedBoost = GetComponent<SpeedBoost>();
    }

    private void GivePowerUp()
    {
        itemImage = rocketImage;
    }

    private void OnTriggerEnter(Collision collision)
    {
        if (collision.gameObject.tag == "car")
        {
            GivePowerUp();
            Destroy(this);
        }
    }
}
