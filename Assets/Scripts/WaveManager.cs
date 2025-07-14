using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private Enemy enemyPrefab;
    private int enemiesToSpawnAtNight = 10;
    [SerializeField] private float timeBetweenSpawns = 1f;
    private int pathTextureIndex = 2; // Índice de la textura de tierra en el Terrain Layer

    private bool spawning = false;

    public void StartWavesInSafeZone(float radius, Vector3 center)
    {
        if (!spawning)
            StartCoroutine(SpawnEnemies(radius, center));
    }

    IEnumerator SpawnEnemies(float radius, Vector3 center)
    {
        spawning = true;

        Terrain terrain = Terrain.activeTerrain;
        if (terrain == null)
        {
            Debug.LogWarning("No active Terrain found.");
            yield break;
        }

        int spawned = 0;
        int maxAttempts = 200; // Para evitar bucle infinito

        while (spawned < enemiesToSpawnAtNight && maxAttempts > 0)
        {
            Vector2 randomPos2D = Random.insideUnitCircle * radius;
            Vector3 spawnPos = center + new Vector3(randomPos2D.x, 0, randomPos2D.y);

            // Ajustamos Y al terreno
            spawnPos.y = terrain.SampleHeight(spawnPos) + terrain.transform.position.y;

            if (IsOnPath(terrain, spawnPos, pathTextureIndex, 0.5f))
            {
                Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
                spawned++;
                yield return new WaitForSeconds(timeBetweenSpawns);
            }

            maxAttempts--;
            yield return null;
        }

        spawning = false;
    }

    /// <summary>
    /// Comprueba si el punto está sobre la textura de camino/tierra.
    /// </summary>
    bool IsOnPath(Terrain terrain, Vector3 worldPos, int textureIndex, float threshold = 0.5f)
    {
        TerrainData data = terrain.terrainData;
        Vector3 terrainLocalPos = worldPos - terrain.transform.position;

        int mapX = Mathf.FloorToInt((terrainLocalPos.x / data.size.x) * data.alphamapWidth);
        int mapZ = Mathf.FloorToInt((terrainLocalPos.z / data.size.z) * data.alphamapHeight);

        if (mapX < 0 || mapX >= data.alphamapWidth || mapZ < 0 || mapZ >= data.alphamapHeight)
            return false;

        float[,,] splatmap = data.GetAlphamaps(mapX, mapZ, 1, 1);
        float blendValue = splatmap[0, 0, textureIndex];

        return blendValue >= threshold;
    }
}
