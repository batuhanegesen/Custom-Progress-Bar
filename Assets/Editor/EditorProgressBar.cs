using UnityEngine;
using UnityEditor;
using System;
using UnityEditor.SceneManagement;
using TMPro;
using UnityEngine.UI;
using Unity;
using UnityEngine.EventSystems;

[CustomEditor(typeof(BD_ProgressBar))]

public class EditorProgressBar : Editor
{
    BD_ProgressBar bar;
    public override void OnInspectorGUI()
    {
        bar = (BD_ProgressBar)target;

        // bar.CheckPool();

        EditorGUI.BeginChangeCheck();
        DrawLayout();
        if (EditorGUI.EndChangeCheck())
        {
            bar.CheckPool();
            bar.PaintUI();
            EditorApplication.QueuePlayerLoopUpdate();
        }
    }

    public void DrawLayout()
    {

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Size");
        GUILayout.FlexibleSpace(); // Fill Space Beginning
        bar.Size = EditorGUILayout.IntField(bar.Size);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Show Increment");
        GUILayout.FlexibleSpace(); // Fill Space Beginning
        bar.ShowIncrement = EditorGUILayout.Toggle(bar.ShowIncrement);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Bar Title");
        GUILayout.FlexibleSpace(); // Fill Space Beginning
        bar.BarTitle = EditorGUILayout.TextField(bar.BarTitle);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Icon Sprite");
        GUILayout.FlexibleSpace(); // Fill Space Beginning
        bar.IconSprite = (Sprite)EditorGUILayout.ObjectField(bar.IconSprite, typeof(Sprite), false);
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

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Default Value");
        GUILayout.FlexibleSpace(); // Fill Space Beginning
        bar.DefaultLevel = EditorGUILayout.IntField(bar.DefaultLevel);
        EditorGUILayout.EndHorizontal();
    }


    [MenuItem("GameObject/UI/Custom/Progress Bar")]
    public static void CreateBar(MenuCommand menuCommand)
    {
        GameObject newObject = PrefabUtility.InstantiatePrefab(Resources.Load("Custom Bar")) as GameObject;
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
}