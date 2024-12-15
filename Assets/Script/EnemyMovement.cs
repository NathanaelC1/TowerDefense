using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Attributes")]
    [SerializeField] private float moveSpeed = 2f;

    private Transform target;
    private int pathIndex = 0;

    private void Start()
    {
        // Ambil waypoint pertama dari LevelManager
        target = LevelManager.main.path[pathIndex];
    }

    private void Update()
    {
        // Cek jarak ke target
        float distance = Vector2.Distance(target.position, transform.position);
        if (distance <= 0.1f)
        {
            // Berpindah ke waypoint berikutnya
            pathIndex++;
            if (pathIndex == LevelManager.main.path.Length)
            {
                EnemySpawner.onEnemyDestroy.Invoke();
                Destroy(gameObject); // Hapus musuh jika mencapai akhir jalur
                return;
            }
            else
            {
                target = LevelManager.main.path[pathIndex];
            }
        }
    }

    private void FixedUpdate()
    {
        // Konversi posisi target ke Vector2
        Vector2 targetPosition = target.position;

        // Hitung arah menuju target
        Vector2 direction = (targetPosition - rb.position).normalized;

        // Perbarui kecepatan Rigidbody
        rb.velocity = direction * moveSpeed;

        // Jika sangat dekat dengan target, hentikan dan pastikan tepat di posisi target
        if (Vector2.Distance(rb.position, targetPosition) <= 0.1f)
        {
            rb.velocity = Vector2.zero;
            rb.position = targetPosition;
        }

        // Panggil fungsi untuk rotasi musuh berdasarkan arah
        RotateSprite(direction);
    }

    private void RotateSprite(Vector2 direction)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.x > 0)
            {
                // Gerakan ke kanan: rotasi ke -90 derajat
                transform.rotation = Quaternion.Euler(0, 0, 270);
            }
            else
            {
                // Gerakan ke kiri: rotasi ke 90 derajat
                transform.rotation = Quaternion.Euler(0, 0, 90);
            }
        }
        else
        {
            if (direction.y > 0)
            {
                // Gerakan ke atas: rotasi ke 0 derajat
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                // Gerakan ke bawah: rotasi ke 180 derajat
                transform.rotation = Quaternion.Euler(0, 0, 180);
            }
        }
    }

}