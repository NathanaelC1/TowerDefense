using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Plot : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Tilemap tilemap; // Referensi Tilemap
    [SerializeField] private Color hoverColor; // Warna hover

    [Header("Offsets")]
    [SerializeField] private Vector2 positionOffset = Vector2.zero; // Offset posisi untuk tower

    private Vector3Int lastMousePosition; // Menyimpan posisi tile terakhir
    private Color defaultColor; // Warna default tile

    // Menyimpan status tile apakah sudah ditempati tower
    private Dictionary<Vector3Int, bool> occupiedTiles = new Dictionary<Vector3Int, bool>();

    private void Start()
    {
        if (tilemap == null)
        {
            Debug.LogError("Tilemap reference is missing!");
            return;
        }

        lastMousePosition = new Vector3Int(int.MaxValue, int.MaxValue, int.MaxValue); // Inisialisasi posisi tidak valid
    }

    private void Update()
    {
        // Ambil posisi mouse di dunia
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPosition.z = 0; // Atur Z ke 0 untuk keselarasan dengan tilemap
        Vector3Int gridPosition = tilemap.WorldToCell(worldPosition); // Konversi ke posisi grid tilemap

        // Jika mouse berpindah ke tile yang berbeda
        if (gridPosition != lastMousePosition)
        {
            ResetTileColor(); // Reset warna tile sebelumnya
            HighlightTile(gridPosition); // Highlight tile baru
            lastMousePosition = gridPosition; // Update posisi terakhir
        }

        // Ketika mouse klik kiri ditekan
        if (Input.GetMouseButtonDown(0))
        {
            BuildTower(gridPosition); // Bangun tower di tile yang diklik
        }
    }

    private void HighlightTile(Vector3Int gridPosition)
    {
        if (tilemap.HasTile(gridPosition))
        {
            // Simpan warna default tile dan ubah warnanya
            defaultColor = tilemap.GetColor(gridPosition);
            tilemap.SetTileFlags(gridPosition, TileFlags.None); // Pastikan tile dapat diubah warnanya
            tilemap.SetColor(gridPosition, hoverColor);
        }
    }

    private void ResetTileColor()
    {
        if (tilemap.HasTile(lastMousePosition))
        {
            tilemap.SetTileFlags(lastMousePosition, TileFlags.None); // Pastikan tile dapat diubah warnanya
            tilemap.SetColor(lastMousePosition, defaultColor); // Kembalikan warna default
        }
    }

    private void BuildTower(Vector3Int gridPosition)
    {
        if (!tilemap.HasTile(gridPosition)) return; // Cegah membangun di tile kosong

        // Cek apakah tile sudah ditempati
        if (occupiedTiles.ContainsKey(gridPosition) && occupiedTiles[gridPosition])
        {
            Debug.LogWarning($"Tile at {gridPosition} is already occupied!");
            return;
        }

        // Dapatkan posisi tengah tile
        Vector3 spawnPosition = tilemap.GetCellCenterWorld(gridPosition);

        // Terapkan offset pada posisi tower
        spawnPosition.x += positionOffset.x;
        spawnPosition.y += positionOffset.y;

        // Bangun tower di posisi yang sudah disesuaikan
        Tower towerToBuild =  BuildManager.main.GetSelectedTower();

        if(towerToBuild.cost > LevelManager.main.currency)
        {
            Debug.Log("Not Affordable");  
            return;
        }

        Instantiate(towerToBuild.prefab, spawnPosition, Quaternion.identity);

        LevelManager.main.SpendCurrency(towerToBuild.cost);

        Debug.Log($"Tower built at: {spawnPosition}");

        // Tandai tile sebagai sudah ditempati
        occupiedTiles[gridPosition] = true;
    }
}
