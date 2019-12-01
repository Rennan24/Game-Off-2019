using UnityEngine;


public class RadiusSpawner : MonoBehaviour
{
    public float SpawnDelay;
    public float Radius;
    public EnemySpawnData[] SpawnerPrefabs;
    public bool SpawnParented;

    public Transform Player;
    public float RadiusAway = 7f;

    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= SpawnDelay)
        {

            // Prevents enemies from spawning on top of the player
            Vector2 radiusPos;
            do
            {
                radiusPos = (Vector2)transform.position + Random.insideUnitCircle * Radius;
            } while (Vector2.Distance(Player.position, radiusPos) < RadiusAway);

            var totalProbabilty = 0f;
            for (int i = 0; i < SpawnerPrefabs.Length; i++)
                totalProbabilty += SpawnerPrefabs[i].Probability;

            var probabilty = Random.Range(0, totalProbabilty);

            foreach (var spawnData in SpawnerPrefabs)
            {
                if (probabilty > spawnData.Probability)
                {
                    probabilty -= spawnData.Probability;
                    continue;
                }

                if (SpawnParented)
                    Instantiate(spawnData.Enemy, radiusPos, Quaternion.identity, transform);
                else
                    Instantiate(spawnData.Enemy, radiusPos, Quaternion.identity);

                break;
            }

            timer = 0;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        WHandles.DrawWireDisk(transform.position, Radius, Color.red);
        WHandles.DrawWireDisk(Player.position, RadiusAway, Color.green);
    }
#endif
}
