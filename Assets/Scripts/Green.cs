using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Green : MonoBehaviour
{
    [SerializeField] private float maxHealth = 3f;
    [SerializeField] private float damageThreshold = 0.2f;
    private float currentHealth;

    [SerializeField] private GameObject deathParticle;

    [SerializeField] private AudioClip greenPoof;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    private void DamageGreen(float damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth < 0)
        {
            DieGreen();
        }
    }

    private void DieGreen()
    {
        GameManager.instance.RemoveGreen(this);
        Instantiate(deathParticle, transform.position, Quaternion.identity);

        AudioSource.PlayClipAtPoint(greenPoof, transform.position);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        float impactVelocity = collision.relativeVelocity.magnitude;

        if (impactVelocity > damageThreshold)
        {
            DamageGreen(impactVelocity);
        }
    }
}
