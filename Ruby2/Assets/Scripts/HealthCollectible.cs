using UnityEngine;


public class HealthCollectible : MonoBehaviour 
{
    public AudioClip collectedClip;
    public ParticleSystem potionEffect;

    void Start()
    {
        potionEffect.Stop();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();

        if (controller != null)
        {
            controller.ChangeHealth(1);
            potionEffect = Instantiate(potionEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
            controller.PlaySound(collectedClip);
            
        }
    }
}
