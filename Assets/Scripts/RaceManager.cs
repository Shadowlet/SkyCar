using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceManager : MonoBehaviour
{
    public Rigidbody[] cars;
    public float respawnDelay = 5;
    public float distanceToCover = 1;
    private CarController[] scripts;
    private float[] respawnTimes;
    private float[] distanceLeftToTravel;
    private Transform[] waypoint;

    // Start is called before the first frame update
    void Start()
    {
        respawnTimes = new float[cars.Length];
        distanceLeftToTravel = new float[cars.Length];
        scripts = new CarController[cars.Length];
        waypoint = new Transform[cars.Length];
        
        for(int i = 0; i < respawnTimes.Length; i++)
        {
            scripts[i] = cars[i].gameObject.GetComponent<CarController>();
            respawnTimes[i] = respawnDelay;
            distanceLeftToTravel[i] = float.MaxValue;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < cars.Length; i++)
        {
            Transform nextWaypoint = scripts[i].GetCurrentWaypoint();
            float distanceCovered = (nextWaypoint.position - cars[i].position).magnitude;

            if(distanceLeftToTravel[i] - distanceToCover > distanceCovered || waypoint[i] != nextWaypoint)
            {
                waypoint[i] = nextWaypoint;
                respawnTimes[i] = respawnDelay;
                distanceLeftToTravel[i] = distanceCovered;
            }
            else
            {
                respawnTimes[i] -= Time.deltaTime;
            }

            if(respawnTimes[i] <= 0)
            {
                respawnTimes[i] = respawnDelay;
                distanceLeftToTravel[i] = float.MaxValue;
                cars[i].velocity = Vector3.zero;

                Transform lastWaypoint = scripts[i].GetLastWaypoint();
                cars[i].position = lastWaypoint.position;
                cars[i].rotation = Quaternion.LookRotation(nextWaypoint.position - lastWaypoint.position);
            }
        }
    }
}
