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
        listView.style.flexDirection = FlexDirection.Row;
        listView.style.flexWrap = Wrap.Wrap;
        listView.style.flexGrow = 1f;

        root.Add(prefabCollectionField);
        root.Add(parentField);
        root.Add(listView);
    }

    private void ListViewOnOnSelectionChanged(List<object> obj)
    {
        prefabCollectionField.value = obj[0] as PrefabCollectionAsset;
    }

    private void BindItem(VisualElement e, int i)
    {
        var asset = prefabAssets[i];
        var image = (Image) e.ElementAt(0);
        var label = (Label) e.ElementAt(1);
        image.image = prefabAssets[i].GetPrefabImage();// AssetPreview.GetAssetPreview(asset.Prefabs[0]);
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
        obj.transform.ScaleX(UnityEngine.Random.value > 0.5f ? -1 : 1);
    }
}
