using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public int itemNum = 0; // Empty
    private Car car;

    public void GetItem(GameObject targetedCar)
    {
        //Item Manager gives the car it's item.
        car = targetedCar.GetComponent<Car>();
        car.currentItem = itemNum;

    }

}
