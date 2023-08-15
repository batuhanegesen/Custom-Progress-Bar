using UnityEngine;
using UnityEditor;
using System;
using UnityEditor.SceneManagement;
using TMPro;
using UnityEngine.UI;
using Unity;
using UnityEngine.EventSystems;
using batudev.CustomUI;
using Unity.VisualScripting;

[CustomEditor(typeof(ProgressBar))]

public class EditorProgressBar : Editor
{
    ProgressBar bar;
    public override void OnInspectorGUI()
    {
        bar = (ProgressBar)target;

        EditorGUI.BeginChangeCheck();
        DrawLayout();
        if (EditorGUI.EndChangeCheck())
        {
            bar.AssignProperties();
            bar.CheckPool();
            bar.PaintUI();
            EditorApplication.QueuePlayerLoopUpdate();
        }
    }

    public void DrawLayout()
    {

        EditorGUILayout.BeginHorizontal();
        Texture2D boxTex;
        Sprite barSprite;
        if (bar.IconSprite == null)
        {
            boxTex = Resources.Load("icons8_eye_60px") as Texture2D;
            barSprite = Sprite.Create(boxTex, new Rect(0,0,boxTex.width, boxTex.height), new Vector2(0.5f, 0.5f));
        }
        else{
            barSprite = bar.IconSprite;
            boxTex = bar.IconSprite.texture;
        }

        GUILayout.Box(boxTex, GUILayout.MinWidth(100), GUILayout.MinHeight(100));
        EditorGUILayout.BeginVertical();
        GUILayout.FlexibleSpace(); // Fill Space Beginning
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Icon Sprite");
        GUILayout.FlexibleSpace(); // Fill Space Beginning
        bar.IconSprite = (Sprite)EditorGUILayout.ObjectField(barSprite, typeof(Sprite), false);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Bar Title");
        GUILayout.FlexibleSpace(); // Fill Space Beginning
        bar.BarTitle = EditorGUILayout.TextField(bar.BarTitle);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Initialize Object Pool");
        GUILayout.FlexibleSpace(); // Fill Space Beginning
        bar.ObjectPoolingSize = EditorGUILayout.IntField(bar.ObjectPoolingSize);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Starting Value");
        GUILayout.FlexibleSpace(); // Fill Space Beginning
        bar.DefaultLevel = EditorGUILayout.IntField(bar.DefaultLevel);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Bar Length");
        GUILayout.FlexibleSpace(); // Fill Space Beginning
        bar.Size = EditorGUILayout.IntField(bar.Size);
        EditorGUILayout.EndHorizontal();

        GUILayout.FlexibleSpace(); // Fill Space Beginning
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();


        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Slice Mode");
        GUILayout.FlexibleSpace(); // Fill Space Beginning
        bar.SliceMode = EditorGUILayout.Toggle(bar.SliceMode);
        GUILayout.Label("Show Value");
        bar.ShowIncrement = EditorGUILayout.Toggle(bar.ShowIncrement);
        GUILayout.Label("Show Buttons");
        bar.ShowButtons = EditorGUILayout.Toggle(bar.ShowButtons);
        GUILayout.Label("Show Percentage");
        bar.ShowPercentage = EditorGUILayout.Toggle(bar.ShowPercentage);
        EditorGUILayout.EndHorizontal();


        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Enable Color");
        GUILayout.FlexibleSpace(); // Fill Space Beginning
        bar.enableColor = EditorGUILayout.ColorField(bar.enableColor);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Disable Color");
        GUILayout.FlexibleSpace(); // Fill Space Beginning
        bar.disableColor = EditorGUILayout.ColorField(bar.disableColor);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Bar Level");
        GUILayout.FlexibleSpace(); // Fill Space Beginning
        bar.BarLevel = EditorGUILayout.IntSlider(bar.BarLevel, 0, bar.Size);
        EditorGUILayout.EndHorizontal();


    }


    [MenuItem("GameObject/UI/Custom/Progress Bar")]
    public static void CreateBar(MenuCommand menuCommand)
    {
        GameObject newObject = PrefabUtility.InstantiatePrefab(Resources.Load("Custom Bar")) as GameObject;
        PrefabUtility.UnpackPrefabInstance(newObject, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
        PlaceObject(newObject);
    }

    public static void PlaceObject(GameObject gameObject)
    {

        Canvas canvas = FindObjectOfType<Canvas>();

        // If Canvas doesn't exist, create a new one
        if (canvas == null)
        {
            GameObject canvasObject = new GameObject("Canvas");
            canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObject.AddComponent<CanvasScaler>();
            canvasObject.AddComponent<GraphicRaycaster>();

            StageUtility.PlaceGameObjectInCurrentStage(canvasObject);
            GameObjectUtility.EnsureUniqueNameForSibling(canvasObject);
            Undo.RegisterCreatedObjectUndo(canvasObject, $"Create Object: {canvasObject.name}");
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }

        if (FindObjectOfType<EventSystem>() == null)
        {
            var eventSystem = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));

            StageUtility.PlaceGameObjectInCurrentStage(eventSystem);
            GameObjectUtility.EnsureUniqueNameForSibling(eventSystem);
            Undo.RegisterCreatedObjectUndo(eventSystem, $"Create Object: {eventSystem.name}");
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }

        gameObject.transform.SetParent(canvas.transform, false);
        gameObject.transform.localPosition = Vector3.zero;

        StageUtility.PlaceGameObjectInCurrentStage(gameObject);
        GameObjectUtility.EnsureUniqueNameForSibling(gameObject);

        Undo.RegisterCreatedObjectUndo(gameObject, $"Create Object: {gameObject.name}");
        Selection.activeGameObject = gameObject;

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
    }


    public static void MarkDirty()
    {
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
    }
}
