using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Event))]
public class EventEditor : Editor
{
    protected Event.EventType eventType = new Event.EventType();
    protected Event.EventType selected;
    public override void OnInspectorGUI()
    {
        //DrawDefaultInspector();
        SerializedObject serializedObject = new SerializedObject(target);
        serializedObject.Update();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("agent"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("eventID"), true);
        EditorGUILayout.LabelField("Add a new event of type: ");
        selected = (Event.EventType)EditorGUILayout.EnumPopup(selected);

        serializedObject.ApplyModifiedProperties();

        Event eventEditor = (Event)target;
        
        if (GUILayout.Button("Add Event"))
        {
            eventEditor.AddEvent(selected.ToString());
        }
    }
}