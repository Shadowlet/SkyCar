using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WheelCollider))]
public class SkidSoundEmmiter : MonoBehaviour
{
    public float skidAt = 1.5f;
    public int soundEmissionPersecond = 10;
    public AudioClip skidSound;

    private float soundDelay;
    private WheelCollider attachedWheel;

    // Start is called before the first frame update
    void Start()
    {
        attachedWheel = transform.GetComponent<WheelCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        WheelHit hit;

        if(attachedWheel.GetGroundHit(out hit))
        {
            float frictionValue = Mathf.Abs(hit.sidewaysSlip);

            if(skidAt <= frictionValue && soundDelay <= 0)
            {
                AudioSource.PlayClipAtPoint(skidSound, hit.point);
                soundDelay = 1;
            }
        }
        soundDelay -= Time.deltaTime * soundEmissionPersecond;
    }
}
