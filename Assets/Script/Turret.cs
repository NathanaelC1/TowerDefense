using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Turret : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private Transform turretRotationPoint; // Titik rotasi turret
    [SerializeField] private LayerMask enemyMask; // Layer musuh
    [SerializeField] private GameObject bulletPrefab; // Peluru
    [SerializeField] private Transform firingPoint; // Titik tembak

    [Header("Attribute")]
    [SerializeField] private float targetingRange = 3f; // Jarak penargetan
    [SerializeField] private float rotationSpeed = 300f; // Kecepatan rotasi dalam derajat per detik
    [SerializeField] private float bps = 0.5f; // Bullet per second

    private Transform target; // Target saat ini
    private float timeUntilFire; // Waktu sebelum menembak

    private void Update()
    {
        // Jika tidak ada target, cari target
        if (target == null)
        {
            FindTarget();
            return;
        }

        // Periksa apakah target masih dalam jangkauan
        if (!CheckTargetIsInRange())
        {
            target = null;
            return;
        }

        // Tembak jika waktunya sudah cukup
        timeUntilFire += Time.deltaTime;
        if (timeUntilFire >= 0.5f / bps)
        {
            Shoot();
            timeUntilFire = 0f; // Reset timer
        }

        // Putar turret ke arah target
        RotateTowardsTarget();
    }

    private void Shoot()
    {
        if (target == null) return; // Pastikan target masih valid

        GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);

        Bullet bulletScript = bulletObj.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.SetTarget(target);
        }
    }

    private void FindTarget()
    {
        // Cari semua musuh dalam jangkauan menggunakan CircleCast
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, Vector2.zero, 0f, enemyMask);

        // Jika ada musuh dalam jangkauan, pilih musuh pertama sebagai target
        if (hits.Length > 0)
        {
            target = hits[0].transform;
        }
    }

    private void RotateTowardsTarget()
    {
        // Hitung sudut ke target
        Vector2 directionToTarget = target.position - turretRotationPoint.position;
        float angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg - 90f;

        // Tentukan rotasi tujuan
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));

        // Putar turret ke arah target dengan kecepatan rotasi tertentu
        turretRotationPoint.rotation = Quaternion.RotateTowards(
            turretRotationPoint.rotation, targetRotation, rotationSpeed * Time.deltaTime
        );
    }

    private bool CheckTargetIsInRange()
    {
        // Periksa apakah target masih dalam jangkauan
        return Vector2.Distance(target.position, transform.position) <= targetingRange;
    }

    // private void OnDrawGizmosSelected()
    // {
    //     // Gambar lingkaran untuk menunjukkan jangkauan penargetan di Scene View
    //     Handles.color = Color.cyan;
    //     Handles.DrawWireDisc(transform.position, Vector3.forward, targetingRange);
    // }
}