using UnityEngine;
//#if UNITY_EDITOR
//using UnityEditor;
//#endif

[CreateAssetMenu(fileName = "Loot Table")]
public class LootTable: ScriptableObject
{
    public LootDrop[] LootDrops;

    private float totalProbabilty;

    public void OnEnable()
    {
        for (int i = 0; i < LootDrops.Length; i++)
            totalProbabilty += LootDrops[i].Probability;
    }

    public GameObject GetDrop()
    {
        var probabilty = UnityEngine.Random.Range(0, totalProbabilty);

        foreach (var LootDrop in LootDrops)
        {
            if (probabilty > LootDrop.Probability)
            {
                probabilty -= LootDrop.Probability;
                continue;
            }

            return LootDrop.Drop;
        }

        return null;
    }
}

[System.Serializable]
public struct LootDrop
{
    public float Probability;
    public GameObject Drop;
}


//#if UNITY_EDITOR
//[CustomPropertyDrawer(typeof(LootDrop))]
//public class LootDropDrawer : PropertyDrawer
//{
//    public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
//    {
//        if (property.serializedObject.isEditingMultipleObjects) return;
//
//        var probProperty = property.FindPropertyRelative("Probability");
//        var dropProperty = property.FindPropertyRelative("Drop");
//
//        label = EditorGUI.BeginProperty(rect, label, property);
//        rect = EditorGUI.PrefixLabel(rect, GUIUtility.GetControlID(FocusType.Passive), label);
//
//        EditorGUI.PropertyField(rect, dropProperty, label);
////        var halfWidth = rect.width * 0.5f;
////        float tempLabelWidth = EditorGUIUtility.labelWidth;
////        EditorGUIUtility.labelWidth = 28f;
////        var start = new Rect(rect.x, rect.y, halfWidth - 10, rect.height);
////        var end = new Rect(rect.x + halfWidth, rect.y, halfWidth, rect.height);
////        EditorGUI.PropertyField(start, minProperty, new GUIContent("Min:"));
////        EditorGUI.PropertyField(end, maxProperty, new GUIContent("Max:"));
////        EditorGUIUtility.labelWidth = tempLabelWidth;
////        EditorGUI.EndProperty();
//    }
//}
//#endif
