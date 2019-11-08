using UnityEngine;
using UnityEngine.Assertions;

public class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Inst
    {
        get
        {
            Assert.IsNotNull(_inst, $"Singleton {typeof(T)} is null!");
            return _inst;
        }
    }

    private static T _inst;

    public virtual void Awake()
    {
        Assert.IsNull(_inst, $"There are more than one Singleton {typeof(T)}");
        _inst = GetComponent<T>();
    }
}
