using UnityEngine;
using System;
using System.Collections.Generic;
using Crosstales.RTVoice;

public class DialogManager : MonoBehaviour
{
    
    protected List<AudioClip> dialogs = new List<AudioClip>();
    protected UnityEngine.Object[] assetDialogs;

    protected AudioSource source;
    protected AudioClip currentDialog;
    private Dialog dialog;

    private string TAG = "DM ";

    void Start()
    {
        #if CT_RTV
            Debug.Log("RTV installed: " + Crosstales.RTVoice.Util.Constants.ASSET_VERSION);
        #else
            Debug.LogWarning("RTV NOT installed!");
        #endif
        source = GetComponentInChildren<AudioSource>();
        assetDialogs = Resources.LoadAll("_Dialogs", typeof(AudioClip));
        foreach (AudioClip a in assetDialogs)
            dialogs.Add(a);
    }

    /* Start speaking. */
    public void Speak(Dialog d) {
        dialog = d;
        dialog.start();
        if ( d.usingTTS )
            useTTS();
        else
            useAudioFile();
    }

    /* If we are using TTS, access the asset to generate speech from text.
     * IMPORTANT: VAIF is designed to work with the */
    private void useTTS()
    {
        if (findFile() == null) //need to generate the file?
        {
            //if the agent is a female, get all the female voices; otherwise, get all the male voices
            Crosstales.RTVoice.Model.Voice agentVoice = (dialog.agent.GetComponent<AgentStatusManager>().isFemale) ? Speaker.VoiceForGender(Crosstales.RTVoice.Model.Enum.Gender.FEMALE) : Speaker.VoiceForGender(Crosstales.RTVoice.Model.Enum.Gender.MALE);
            if (Speaker.areVoicesReady)
                Speaker.Speak(dialog.dialogText, source, agentVoice, true, 1, 1, 1, "Assets\\VAIF\\Resources\\_Dialogs\\" + dialog.IDescription);

            //generate audio file
            Speaker.OnSpeakAudioGenerationStart += Speaker_OnSpeakAudioGenerationStart;
            Speaker.OnSpeakAudioGenerationComplete += Speaker_OnSpeakAudioGenerationComplete;
        }
        else //play the audio file
        {
            dialog.audioFile = findFile();
            useAudioFile();
        }
    }

    /* Start creating and save the audio file to play with phonemes. */
    private void Speaker_OnSpeakAudioGenerationStart(Crosstales.RTVoice.Model.Wrapper wrapper)
    {
        Debug.Log(TAG + "creating audio file: " + wrapper.OutputFile);
    }
    private void Speaker_OnSpeakAudioGenerationComplete(Crosstales.RTVoice.Model.Wrapper wrapper)
    {
        //from https://docs.unity3d.com/ScriptReference/AudioClip.Create.html
        int samplerate = 44100;
        Debug.Log(TAG + "created audio file: " + wrapper.OutputFile);
        dialog.audioFileName = wrapper.OutputFile; //save??
        dialog.audioFile = AudioClip.Create(wrapper.OutputFile, 2 * samplerate, 1, samplerate, true);
        currentDialog = dialog.audioFile;
        dialogs.Add(dialog.audioFile); //this is the audioclip to play
        Invoke("NotSpeaking", (float)0.75);
    }

    /* Do we already have the audio file for this event instance? */
    private AudioClip findFile()
    {
        foreach ( AudioClip ac in dialogs)
            if (ac.name == dialog.IDescription) return ac;
        return null;
    }


    /* If we are not using TTS, we are using an audio file.
     * Play the audio file for the agent to make an utterance. */
    private void useAudioFile()
    {
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
            Debug.Log(TAG + "Dialog audio not found for: " + dialog.audioFileName);
        }
    }

    /* End the audio clip (stop speaking). */
    void NotSpeaking()
    {
        Debug.Log(TAG + "Finished Dialog: " + currentDialog.name + ", length: " + source.clip.length);
        dialog.finish();
    }
}
