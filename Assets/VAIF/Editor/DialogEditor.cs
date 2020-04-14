/*using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Dialog))]
public class DialogEditor : Editor
{
    protected EventIM.EventSetting selected1;
    protected EventIM.EventSetting selected2;
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        SerializedObject serializedObject = new SerializedObject(target);
        serializedObject.Update();
        //EditorGUILayout.PropertyField(serializedObject.FindProperty("agent"), true);
        //EditorGUILayout.PropertyField(serializedObject.FindProperty("EventID"), true);
        EditorGUILayout.LabelField("isInRange Setting: ");
        selected1 = (EventIM.EventSetting)EditorGUILayout.EnumPopup(selected1);
        EditorGUILayout.LabelField("isLookedAt Setting: ");
        selected2 = (EventIM.EventSetting)EditorGUILayout.EnumPopup(selected2);

        serializedObject.ApplyModifiedProperties();

        EventIM eventEditor = (EventIM)target;

        /**
        if (GUILayout.Button("Add Event"))
        {
            eventEditor.AddEvent(selected.ToString());
        }
        
    }
}*/