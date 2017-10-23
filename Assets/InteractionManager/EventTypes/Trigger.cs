using UnityEngine;

public class Trigger : EventIM
{
    [Tooltip("Mandatory. Name of the script that contains the method you want to trigger as a string.")]
    public Component component;
    [Tooltip("Mandatory. Case sensitive method name you want to execute.")]
    public string method;
    [Tooltip("Optional. If your method has parameters, you can specify them here. Your method might need to typecast" +
        "the strings in this array to handle other types.")]
    public string [] parameters;
    /*
    [Tooltip("Mandatory. Game Object, dragged from the scene hierarchy of the object that contains the component and the method" +
        "you want to trigger.")]
    public GameObject triggerObject;
    [Tooltip("Mandatory. Name of the script that contains the method you want to trigger as a string.")]
    public string component;
    [Tooltip("Mandatory. Case sensitive method name you want to execute.")]
    public string method;
    [Tooltip("Optional. If your method has parameters, you can specify them here. Your method might need to typecast" +
        "the strings in this array to handle other types.")]
    public string [] parameters;
    */
}
