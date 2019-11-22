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
    private ItemManager itemManager;

    public Image itemImage;
    public Sprite rocketImage;
    public Sprite boostImage;

    private Vector3 chestRotation = new Vector3(0, 2, 0);

    // Start is called before the first frame update
    void Start()
    {
        itemManager = GetComponent<ItemManager>();
        car = GetComponent<Car>();
        chestRB = GetComponent<Rigidbody>();
        rocket = GetComponent<Rocket>();
        //speedBoost = GetComponent<SpeedBoost>();
    }

    private void Update()
    {
        chestRB.transform.Rotate(chestRotation);
    }

    private void GivePowerUp(int itemNum)
    {
        if(itemNum == 1)
        {
            itemImage.sprite = rocketImage;
        }
        else if(itemNum == 2)
        {
            itemImage.sprite = boostImage;
        }
        itemManager.currentItem = itemNum; // 1 -- Rocket, 2 -- Boost;

        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "car")
        {
            //Debug.Log("COLLDIEREREGHUIERIOJE");
            GivePowerUp(Random.Range(1,2));
            
        }
    }
}
