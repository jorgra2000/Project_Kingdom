using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private float timeBetweenSpawns = 1f;
    private GameManager gameManager;
    private int pathTextureIndex = 2;

    private bool spawning = false;

    private void Start()
    {
        gameManager = GetComponent<GameManager>();
    }

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

        int maxAttempts = 300;

        while (gameManager.IsNight && maxAttempts > 0)
        {
            float angle = Random.Range(0f, 2f * Mathf.PI);
            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;
            Vector3 spawnPos = center + new Vector3(x, 0, z);

            spawnPos.y = terrain.SampleHeight(spawnPos) + terrain.transform.position.y;

            if (IsOnPath(terrain, spawnPos, pathTextureIndex, 0.5f))
            {
                Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
                yield return new WaitForSeconds(timeBetweenSpawns);
            }

            maxAttempts--;
            yield return null;
        }

        spawning = false;
    }

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
