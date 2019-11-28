using UnityEngine;

[CreateAssetMenu(menuName = "Reborn/PrefabCollection")]
public class PrefabCollectionAsset : ScriptableObject
{
    public GameObject[] Prefabs;

    public GameObject GetRandomPrefab()
    {
        var i = Random.Range(0, Prefabs.Length);
        return Prefabs[i];
    }
}
