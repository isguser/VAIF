using UnityEngine;
using System;
using System.Collections.Generic;

public class DialogManager : MonoBehaviour
{
    protected List<AudioClip> dialogs = new List<AudioClip>();
    protected UnityEngine.Object[] assetDialogs;

    protected AudioSource source;
    protected AudioClip currentDialog;
    private Dialog dialog;

    private string TAG = "DM";

    void Start()
    {
        source = GetComponentInChildren<AudioSource>();
        assetDialogs = Resources.LoadAll("_Dialogs", typeof(AudioClip));
        foreach (AudioClip a in assetDialogs)
            dialogs.Add(a);
    }

    public void Speak(Dialog d)
    {
        dialog = d;
        dialog.start();
        //source.Stop();
        if (dialog.audioFile != null)
        {
            currentDialog = dialogs.Find((AudioClip a) => { return a.name == dialog.audioFile.name; });
        }
        else if (dialog.audioFileName != "")
        {
            currentDialog = dialogs.Find((AudioClip a) => { return a.name == dialog.audioFileName; });
        }

        if (currentDialog != null)
        {
            source.clip = currentDialog;
            source.Play();
            Invoke("NotSpeaking", source.clip.length);
        }
        else
        {
            Debug.Log(TAG + " Dialog audio not found for: " + dialog.audioFileName);
        }
    }

    void NotSpeaking()
    {
        Debug.Log(TAG + " Finished Dialog: " + currentDialog.name + ", length: " + source.clip.length);
        dialog.finish();
    }
}
