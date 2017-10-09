using UnityEngine;

public class TriggerManager : MonoBehaviour
{
    private void Start()
    {
        
    }

    public void Trigger(Trigger trigger)
    {
        Component comp = trigger.component;
        comp.SendMessage(trigger.method, trigger.parameters);
    }
}
