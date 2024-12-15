using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private GameObject[] enemyPrefab;

    [Header("Attributes")]
    [SerializeField] private int baseEnemies = 8;
    [SerializeField] private float enemiesPerSecond = 0.5f;
    [SerializeField] private float timeBetweenWaves = 5f;
    [SerializeField] private float difficultyScalingFactor = 0.75f;

    [Header("Events")]
    public static UnityEvent onEnemyDestroy = new UnityEvent();

    private int currentWave = 1;
    private float timeSinceLastSpawn;
    private int enemiesAlive = 0; // Musuh yang hidup di wave saat ini
    private int enemiesLeftToSpawn = 0; // Musuh yang tersisa untuk di-spawn
    private bool isSpawning = false;

    private void Awake()
    {
        // Tambahkan listener untuk event Enemy Destroy
        onEnemyDestroy.AddListener(EnemyDestroyed);
    }

    private void Start()
    {
        StartCoroutine(StartWave());
    }

    private void Update()
    {
        if (!isSpawning) return; // Jika tidak sedang spawning, keluar dari Update

        timeSinceLastSpawn += Time.deltaTime;

        // Spawn musuh jika waktunya sudah tepat
        if (timeSinceLastSpawn >= (1f / enemiesPerSecond) && enemiesLeftToSpawn > 0)
        {
            SpawnEnemy();
            enemiesLeftToSpawn--;
            enemiesAlive++; // Tambahkan jumlah musuh hidup
            timeSinceLastSpawn = 0f;
        }

        // Jika semua musuh habis (hidup dan tersisa untuk di-spawn), akhiri wave
        if (enemiesAlive == 0 && enemiesLeftToSpawn == 0)
        {
            EndWave();
        }
    }

    private void EnemyDestroyed()
    {
        enemiesAlive--; // Kurangi jumlah musuh hidup
        Debug.Log($"Enemy Destroyed! Enemies Alive: {enemiesAlive}");
    }

    private IEnumerator StartWave()
    {
        // Tunggu sebelum memulai wave berikutnya
        yield return new WaitForSeconds(timeBetweenWaves);

        // Mulai spawning musuh untuk wave baru
        isSpawning = true;
        enemiesLeftToSpawn = EnemiesPerWave(); // Hitung jumlah musuh per wave
        // Debug.Log($"Wave {currentWave} Started. Enemies to Spawn: {enemiesLeftToSpawn}");
    }

    private void EndWave()
    {
        // Akhiri wave dan siapkan wave berikutnya
        isSpawning = false;
        timeSinceLastSpawn = 0f;
        currentWave++;
        // Debug.Log($"Wave {currentWave - 1} Ended. Starting Next Wave...");
        StartCoroutine(StartWave());
    }

    private void SpawnEnemy()
    {
        // Pilih prefab musuh dan spawn
        GameObject prefabToSpawn = enemyPrefab[0];
        GameObject enemy = Instantiate(prefabToSpawn, LevelManager.main.startPoint.position, Quaternion.identity);

        // Tambahkan komponen Enemy jika belum ada
        if (!enemy.GetComponent<Enemy>())
        {
            enemy.AddComponent<Enemy>();
        }

        // Debug.Log($"Spawned Enemy at {LevelManager.main.startPoint.position}. Enemies Left to Spawn: {enemiesLeftToSpawn - 1}");
    }

    private int EnemiesPerWave()
    {
        // Hitung jumlah musuh untuk wave berdasarkan skala kesulitan
        return Mathf.RoundToInt(baseEnemies * Mathf.Pow(currentWave, difficultyScalingFactor));
    }
}
