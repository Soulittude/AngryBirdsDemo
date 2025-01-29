using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Angry : MonoBehaviour
{
    private Rigidbody2D rb;
    private CircleCollider2D col;

    private bool hasBeenLaunched;
    private bool shouldFaceVelDir;

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

    private void FixedUpdate()
    {
        if (hasBeenLaunched && shouldFaceVelDir)
        {
            transform.right = rb.velocity;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        shouldFaceVelDir = false;
    }

    public void LaunchBirb(Vector2 dir, float force)
    {
        rb.isKinematic = false;
        col.enabled = true;

        rb.AddForce(dir * force, ForceMode2D.Impulse);

        hasBeenLaunched = true;
        shouldFaceVelDir = true;
    }
}
