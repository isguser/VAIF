using UnityEngine;
using System;
using System.Collections.Generic;

public class DialogManager : MonoBehaviour
{
    protected List <AudioClip> dialogs =  new List<AudioClip>();
    protected UnityEngine.Object[] assetDialogs;

    protected AudioSource source;
    protected InteractionManager interactionManager;
    protected AgentStatusManager agentStatus;

    protected AudioClip currentDialog;

    void Start ()
    {
		source = GetComponentInChildren<AudioSource>();
        interactionManager = FindObjectOfType<InteractionManager>();

        assetDialogs = Resources.LoadAll("_Dialogs", typeof(AudioClip));

        foreach (AudioClip a in assetDialogs)
        {
            dialogs.Add(a);
        }
    }

    public void Speak(Dialog dial)
    {
        interactionManager.isSpeaking = true;
        agentStatus = dial.agent.GetComponent<AgentStatusManager>();
        agentStatus.isSpeaking = true;

        source.Stop();
        if (dial.audioFile != null)
        {
            currentDialog = dialogs.Find((AudioClip a) => { return a.name == dial.audioFile.name; });
        }
        else if(dial.audioFileName != "")
        {
            currentDialog = dialogs.Find((AudioClip a) => { return a.name == dial.audioFileName; });
        }

        if (currentDialog != null)
        {
			source.clip = currentDialog;
			source.Play();
			Debug.Log("Dialog: " + currentDialog.name + " with length: " + source.clip.length);
			Invoke("NotSpeaking", source.clip.length);
		}

		else
        {
			Debug.Log("Dialog audio not found for: " + dial.audioFileName);
		}
	}

	void NotSpeaking()
    {
        agentStatus.isSpeaking = false;
        interactionManager.isSpeaking = false;
	}
}
