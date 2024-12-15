using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb; // Rigidbody untuk peluru
    [SerializeField] private SpriteRenderer spriteRenderer; // Sprite peluru (opsional)

    [Header("Attributes")]
    [SerializeField] private float bulletSpeed = 5f; // Kecepatan peluru
    [SerializeField] private LayerMask enemyLayer; // Layer musuh
    [SerializeField] public float bulletDamage = 1.0f;

    private Transform target; // Target peluru
    private Vector2 direction; // Arah gerakan peluru

    public void SetTarget(Transform _target)
    {
        target = _target;

        // Tentukan arah berdasarkan posisi target
        if (target != null)
        {
            direction = (target.position - transform.position).normalized;

            // Atur rotasi peluru sesuai arah
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle); // Sesuaikan dengan desain peluru
        }
    }

    private void FixedUpdate()
    {
        if (target == null)
        {
            Destroy(gameObject); // Hancurkan peluru jika target tidak ada
            return;
        }

        // Gerakkan peluru ke arah target
        rb.velocity = direction * bulletSpeed;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // Periksa apakah objek yang ditabrak ada di layer musuh
        if (((1 << other.gameObject.layer) & enemyLayer) != 0)
        {
            // Hancurkan musuh (opsional)
            other.gameObject.GetComponent<EnemyHealth>().TakeDamage(bulletDamage);

            // Hancurkan peluru
            Destroy(gameObject);
        }
    }
}
