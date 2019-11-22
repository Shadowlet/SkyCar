using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public int currentItem = 0; // Empty
    private Car car;

    public void UseItem(GameObject targetedCar)
    {
        targetedCar.GetComponent<Car>();
        car.UseItem(currentItem);

    }

}
