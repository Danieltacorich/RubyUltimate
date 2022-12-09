using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison : MonoBehaviour
{
    public AudioClip PoisonPotion;
    public ParticleSystem potionEffect;
   void OnTriggerEnter2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();

        if (controller != null)
        {
            controller.ChangeHealth(-2);
            Destroy(gameObject);
             controller.PlaySound(PoisonPotion);
        }
    }
}
