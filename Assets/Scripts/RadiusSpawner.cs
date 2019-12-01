using UnityEngine;


public class RadiusSpawner : MonoBehaviour
{
    public float SpawnDelay;
    public float Radius;
    public GameObject[] SpawnerPrefabs;
    public bool SpawnParented;

    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= SpawnDelay)
        {
            var radiusPosition = (Vector2)transform.position + Random.insideUnitCircle * Radius;

            if (SpawnParented)
            {
                var n = Random.Range(0, SpawnerPrefabs.Length);
                Instantiate(SpawnerPrefabs[n], radiusPosition, Quaternion.identity, transform);
            }
            else
            {
                var n = Random.Range(0, SpawnerPrefabs.Length);
                Instantiate(SpawnerPrefabs[n], radiusPosition, Quaternion.identity);
            }

            timer = 0;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        WHandles.DrawWireDisk(transform.position, Radius, Color.red);
    }
#endif
}
