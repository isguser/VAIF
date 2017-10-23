using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EventIM))]
public class EventEditor : Editor
{
    protected EventIM.EventType eventType = new EventIM.EventType();
    protected EventIM.EventType selected;
    public override void OnInspectorGUI()
    {
        //DrawDefaultInspector();
        SerializedObject serializedObject = new SerializedObject(target);
        serializedObject.Update();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("agent"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("eventID"), true);
        EditorGUILayout.LabelField("Add a new event of type: ");
        selected = (EventIM.EventType)EditorGUILayout.EnumPopup(selected);

        serializedObject.ApplyModifiedProperties();

        EventIM eventEditor = (EventIM)target;
        
        if (GUILayout.Button("Add Event"))
        {
            eventEditor.AddEvent(selected.ToString());
        }
    }
}