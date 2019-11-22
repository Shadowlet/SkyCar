using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUp : MonoBehaviour
{
    private Rocket rocket;
    //private SpeedBoost speedBoost;
    private Car car;
    private Rigidbody chestRB;

    public Image itemImage;
    public Sprite rocketImage;
    public Sprite boostImage;

    private Vector3 chestRotation = new Vector3(0, 2, 0);

    // Start is called before the first frame update
    void Start()
    {
        car = GetComponent<Car>();
        chestRB = GetComponent<Rigidbody>();
        rocket = GetComponent<Rocket>();
        //speedBoost = GetComponent<SpeedBoost>();
    }

    private void Update()
    {
        chestRB.transform.Rotate(chestRotation);
    }

    private void GivePowerUp()
    {
        itemImage.sprite = rocketImage;
        //itemImage = boostImage;

        car.currentItem = 1; // 1 -- Rocket

        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "car")
        {
            Debug.Log("COLLDIEREREGHUIERIOJE");
            GivePowerUp();
            
        }
    }
}
