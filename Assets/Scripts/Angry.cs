using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Angry : MonoBehaviour
{
    private Rigidbody2D rb;
    private CircleCollider2D col;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CircleCollider2D>();
    }

    private void Start()
    {
        rb.isKinematic = true;
        col.enabled = false;
    }
    public void LaunchBirb(Vector2 dir, float force)
    {
        rb.isKinematic = false;
        col.enabled = true;

        rb.AddForce(dir * force, ForceMode2D.Impulse);
    }
}
