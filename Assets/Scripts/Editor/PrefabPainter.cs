using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class PrefabPainter : EditorWindow
{
    private static PrefabPainter window;

    private List<PrefabCollectionAsset> prefabAssets;
    private ObjectField prefabCollectionField;
    private ObjectField parentField;

    private PrefabCollectionAsset curAsset;

    private SerializedObject curObj;

    private Foldout foldout;

    private bool flipX;

    private const string UXLMPath = "Assets/Scripts/Editor/UIElements/PrefabPainter.uxml";
    private const string USSPath = "Assets/Scripts/Editor/UIElements/PrefabPainter.uss";

    [MenuItem("Tools/Prefab Painter")]
    public static void OpenWindow()
    {
        window = GetWindow<PrefabPainter>();
    }

    private void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
        CreateGUI();
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    private void CreateGUI()
    {
        var root = rootVisualElement;

        // Setting up the GUI from UXML and USS
//        var tree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(UXLMPath);
//        var uss = AssetDatabase.LoadAssetAtPath<StyleSheet>(USSPath);
//        tree.CloneTree(root);
//        root.styleSheets.Add(uss);
//
//        foldout = root.Q<Foldout>();
//
//        var prefabCollectionField = root.Q<ObjectField>("PrefabCollection");
//        var parentField = root.Q<ObjectField>("Parent");
//
//
//        parentField.objectType = typeof(GameObject);
//
//        prefabCollectionField.objectType = typeof(PrefabCollectionAsset);
//        prefabCollectionField.RegisterValueChangedCallback(Callback);

        prefabCollectionField = new ObjectField("Prefab Collection Asset") {
            objectType = typeof(PrefabCollectionAsset),
            allowSceneObjects = false,
        };

        parentField = new ObjectField("Parent Field") {
            objectType = typeof(GameObject),
            allowSceneObjects = true,
        };


        var assetIDs = AssetDatabase.FindAssets("t:PrefabCollectionAsset");
        prefabAssets = new List<PrefabCollectionAsset>(assetIDs.Length);

        foreach (var ID in assetIDs)
        {
            var path = AssetDatabase.GUIDToAssetPath(ID);
            var asset = AssetDatabase.LoadAssetAtPath<PrefabCollectionAsset>(path);
            prefabAssets.Add(asset);
        }

        var listView = new ListView(prefabAssets, 100, MakeItem, BindItem);
        listView.onSelectionChanged += ListViewOnOnSelectionChanged;
        listView.selectionType = SelectionType.Single;
        listView.style.flexGrow = 1f;

        listView.contentContainer.style.flexDirection = FlexDirection.Row;
        listView.contentContainer.style.flexWrap = Wrap.Wrap;

        var toggle = new Toggle("Flippable");

        toggle.RegisterValueChangedCallback(evt => flipX = evt.newValue);

        root.Add(prefabCollectionField);
        root.Add(parentField);
        root.Add(toggle);
        root.Add(listView);
    }

//    private void Callback(ChangeEvent<Object> evt)
//    {
//        var prefabCol = (PrefabCollectionAsset) evt.newValue;
//        curObj = new SerializedObject(prefabCol);
//        foldout.Bind(curObj);
//    }

    private void ListViewOnOnSelectionChanged(List<object> obj)
    {
        prefabCollectionField.value = obj[0] as PrefabCollectionAsset;
    }

    private void BindItem(VisualElement e, int i)
    {
        var asset = prefabAssets[i];
        var image = (Image) e.ElementAt(0);
        var label = (Label) e.ElementAt(1);
        image.image = prefabAssets[i].GetPrefabImage();
        label.text = asset.name;
    }

    private VisualElement MakeItem()
    {
        var box = new Box {
            style = { width = 150, height = 150 }
        };

        var image = new Image {
            scaleMode = ScaleMode.ScaleToFit,
        };
        var label = new Label {
            style = { alignSelf = Align.Center }
        };

        box.Add(image);
        box.Add(label);
        return box;
    }

    private void OnSceneGUI(SceneView view)
    {
        var e = Event.current;

        switch (e.type)
        {
            case EventType.MouseDown:
                MousePressed(e, view);
                break;
        }
    }

    private void MousePressed(Event e, SceneView view)
    {
        var prefabCollection = (PrefabCollectionAsset) prefabCollectionField.value;
        var prefab = prefabCollection.GetRandomPrefab();
        var mouseButton = (MouseButton) e.button;

        if (prefab == null || mouseButton != MouseButton.RightMouse)
            return;

        var mult = EditorGUIUtility.pixelsPerPoint;
        Vector3 mousePos = e.mousePosition;
        mousePos.y = view.camera.pixelHeight - mousePos.y * mult;
        mousePos.z = -view.camera.transform.position.z;
        mousePos.x *= mult;

        var worldPos = view.camera.ScreenToWorldPoint(mousePos);

        var parent = (parentField.value as GameObject)?.transform;

        var obj = PrefabUtility.InstantiatePrefab(prefab, parent) as GameObject;
        Undo.RegisterCreatedObjectUndo(obj, "Object Created");

        obj.transform.position = worldPos;
        obj.transform.ScaleX(flipX && UnityEngine.Random.value > 0.5f ? -1 : 1);
        obj.transform.localScale *= prefabCollection.GetRandomSize();
        obj.transform.rotation = Quaternion.Euler(0, 0, prefabCollection.GetRandomRotation());
    }
}
