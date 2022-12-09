using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedSlow : MonoBehaviour
{
    public AudioClip SlowSound;
     public ParticleSystem SlowEffect;

    void Start()

     {
        SlowEffect.Stop();
     }

    void OnTriggerEnter2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();

        if (controller != null)
        {
            controller.SpeedSlow(-1);
            SlowEffect = Instantiate(SlowEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
           
            controller.PlaySound(SlowSound);

        }
    }
}
