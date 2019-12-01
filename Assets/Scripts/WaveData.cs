using System.Collections;
using UnityEngine;

[CreateAssetMenu]
public class WaveData : ScriptableObject
{
    public float SpawnDelay;
    public float SpawnAmountMax;

    public int SpawnsLeft;
    public EnemySpawnData[] EnemySpawnData;

    private float totalProbabilty;

    private void OnEnable()
    {
        for (int i = 0; i < EnemySpawnData.Length; i++)
            totalProbabilty += EnemySpawnData[i].Probability;
    }

    public bool IsComplete() => SpawnsLeft <= 0;

    public IEnumerator BeginWave(WaveController waveController)
    {
        do
        {
            waveController.EnemiesLeft += Spawn();
            yield return new WaitForSeconds(SpawnDelay);
        } while (!IsComplete());
    }

    public int Spawn()
    {
        var spawnAmount = (int) Mathf.Min(SpawnAmountMax, SpawnsLeft);

        for (int i = 0; i < spawnAmount; i++)
        {
            var probabilty = Random.Range(0, totalProbabilty);

            foreach (var spawnData in EnemySpawnData)
            {
                if (probabilty > spawnData.Probability)
                {
                    probabilty -= spawnData.Probability;
                    continue;
                }

                Instantiate(spawnData.Enemy);
                SpawnsLeft--;
            }
        }

        return spawnAmount;
    }
}

[System.Serializable]
public struct EnemySpawnData
{
    public float Probability;
    public GameObject Enemy;
}
