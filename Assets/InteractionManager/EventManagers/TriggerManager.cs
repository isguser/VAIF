using UnityEngine;

public class TriggerManager : MonoBehaviour
{
    private void Start()
    {

    }

    public void Trigger(Trigger trigger)
    {
        trigger.start();
        Component comp = trigger.component;
        comp.SendMessage(trigger.method, trigger.parameters);
        trigger.finish();
    }
}
