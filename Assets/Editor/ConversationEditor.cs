using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ConversationIM))]
public class ConversationEditor : Editor
{
    protected ConversationIM.ConversationType convType = new ConversationIM.ConversationType();
    protected ConversationIM.ConversationType selected;
    public override void OnInspectorGUI()
    {
        //DrawDefaultInspector();
        SerializedObject serializedObject = new SerializedObject(target);
        serializedObject.Update();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("conversation"), true);
        //EditorGUILayout.PropertyField(serializedObject.FindProperty("eventID"), true);
        EditorGUILayout.LabelField("Add a new Conversation of type: ");
        selected = (ConversationIM.ConversationType)EditorGUILayout.EnumPopup(selected);

        serializedObject.ApplyModifiedProperties();

        ConversationIM convEditor = (ConversationIM)target;
        
        if (GUILayout.Button("Add Conversation"))
        {
            convEditor.AddConversation(selected.ToString());
        }
    }
}