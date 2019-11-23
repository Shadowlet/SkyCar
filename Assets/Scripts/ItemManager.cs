using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public int itemNum = 0; // Empty
    private Car car;

    public void GetItem(GameObject targetedCar)
    {
        car = targetedCar.GetComponent<Car>();
        car.currentItem = itemNum;

    }

}
