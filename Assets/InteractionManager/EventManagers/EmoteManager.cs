using UnityEngine;

public class EmoteManager : MonoBehaviour
{

    protected InteractionManager interactionManager;
    public SkinnedMeshRenderer emotionalFace;
    public Mesh blendshapeContainer;

    public float currentBlnshapeValue, nextBlendshapeValue, transitionBlendshapeValue;
    public float step = 1f;
    public bool startTransition = false;
    public int bsIndex;

    // Use this for initialization
    void Start()
    {
            emotionalFace = getFace().GetComponentInChildren<SkinnedMeshRenderer>();
    }

    public void EmotionBlendshape(Emote emotionDisplay)
    {
        bsIndex = findEmotionBlendshape(emotionDisplay.emotion);
        if (bsIndex == -1)
        {
            Debug.Log("Blendshape not found, check the spelling: " + bsIndex);
            return;
        }
        currentBlnshapeValue = emotionalFace.GetBlendShapeWeight(bsIndex);
        nextBlendshapeValue = emotionDisplay.emotionalValue;
        startTransition = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (currentBlnshapeValue <= nextBlendshapeValue && startTransition && transitionBlendshapeValue <= nextBlendshapeValue)
        {
            Debug.Log("Current v. Next: " + currentBlnshapeValue + " " + nextBlendshapeValue);
            transitionBlendshapeValue++;
            emotionalFace.SetBlendShapeWeight(bsIndex, transitionBlendshapeValue);
        }
        else
        {
            startTransition = false;
            transitionBlendshapeValue = 0;
        }
    }

    int findEmotionBlendshape(string blendshapeName)
    {
        blendshapeContainer = emotionalFace.sharedMesh;
        for (int i = 0; i < blendshapeContainer.blendShapeCount; i++)
        {
            if (blendshapeContainer.GetBlendShapeName(i) == blendshapeName)
            {
                Debug.Log("Found emotion name: " + blendshapeContainer.GetBlendShapeName(i) + " with index " + i);
                return i;
            }
        }
        return -1;
    }
    
    private Transform getFace()
    {
        foreach (Transform child in transform)
        {
            if (child.CompareTag("Face"))
                return child;
        }
        return null;
    }
}
