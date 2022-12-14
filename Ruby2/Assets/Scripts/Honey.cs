using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Honey : MonoBehaviour

{
    public AudioClip collectedClip;
    void OnTriggerEnter2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();

        if (controller != null)
        {
          
            Destroy(gameObject);

            controller.PlaySound(collectedClip);
            
        }
        if (controller != null)
		{
			controller.HoneyCount(1);
		}
    }
}
