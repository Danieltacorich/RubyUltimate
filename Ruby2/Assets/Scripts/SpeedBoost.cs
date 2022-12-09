using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoost : MonoBehaviour
{
    public AudioClip collectedPotion;
    public ParticleSystem potionEffect;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();

        if (controller != null)
        {

            controller.SpeedBoost(1);
            potionEffect = Instantiate(potionEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
           
            controller.PlaySound(collectedPotion);
            
        }

    }
}
