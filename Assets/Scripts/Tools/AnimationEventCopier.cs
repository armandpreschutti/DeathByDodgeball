using UnityEditor;
using UnityEngine;

public class AnimationEventCopier : EditorWindow
{
    private AnimationClip sourceObject;
    private AnimationClip targetObject;

    [MenuItem("Window/Animation Event Copier")]
    static void Init()
    {
        GetWindow<AnimationEventCopier>("Animation Event Copier");
    }

    void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        sourceObject = EditorGUILayout.ObjectField("Source", sourceObject, typeof(AnimationClip), true) as AnimationClip;
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        targetObject = EditorGUILayout.ObjectField("Target", targetObject, typeof(AnimationClip), true) as AnimationClip;
        EditorGUILayout.EndHorizontal();

        if (sourceObject != null && targetObject != null)
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Copy"))
                CopyData();
            EditorGUILayout.EndHorizontal();
        }
        else
        {
            // Ensure that GUILayout.EndHorizontal is called for every GUILayout.BeginHorizontal
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }
    }

    void CopyData()
    {
        Undo.RegisterCompleteObjectUndo(targetObject, "Copy Animation Events");

        if (sourceObject && targetObject)
        {
            var events = AnimationUtility.GetAnimationEvents(sourceObject);

            if (events != null)
            {
                AnimationUtility.SetAnimationEvents(targetObject, null);
                AnimationUtility.SetAnimationEvents(targetObject, events);
            }
            else
            {
                Debug.LogWarning("Source animation has no events to copy.");
            }
        }
    }
}
