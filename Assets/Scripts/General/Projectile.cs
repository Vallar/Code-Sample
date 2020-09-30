using System.Collections;
using System.Collections.Generic;
using ThirteenPixels.Pooling;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 300;
    [SerializeField] private float timeTillDeath;
    private int damage;
    private PoolReturner returner;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        returner = GetComponent<PoolReturner>();
    }

    private void OnEnable()
    {
        returner.ReturnAfter(timeTillDeath);
        rb.isKinematic = false;
    }

    public void Fire(Transform _target, int _damage)
    {
        damage = _damage;

        Vector2 direction = (Vector2)_target.position - rb.position;
        direction = direction.normalized;

        rb.AddForce(direction * speed);

    }

    private void OnCollisionEnter2D(Collision2D _collision)
    {
        if (_collision.gameObject.layer == gameObject.layer)
            return;

        //StatsHandlerBase stats = _collision.transform.GetComponent<StatsHandlerBase>();

        //stats?.TakeDamage(damage);
        returner.Return();

        //TODO Spawn some stuff here.
    }

    private void OnDisable()
    {
        rb.isKinematic = true;
        rb.velocity = Vector2.zero;
    }
}
