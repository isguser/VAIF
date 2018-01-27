using UnityEngine;

public class TriggerManager : MonoBehaviour
{
    private void Start()
    {
        
    }

    public void Trigger(Trigger trigger)
    {
        trigger.started = true;
        Component comp = trigger.component;
        comp.SendMessage(trigger.method, trigger.parameters);
        trigger.isDone = false;
    }
}
