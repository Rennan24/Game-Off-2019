using UnityEngine;


public class RadiusSpawner : MonoBehaviour
{
    public float SpawnDelay;
    public float Radius;
    public GameObject SpawnerPrefab;

    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= SpawnDelay)
        {
            var radiusPosition = (Vector2)transform.position + Random.insideUnitCircle * Radius;
            Instantiate(SpawnerPrefab, radiusPosition, Quaternion.identity, transform);
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
