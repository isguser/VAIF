using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    [Tooltip("Mandatory. The complete list of animations for this agent. Only these animations can be called in an event with this agent.")]
    public List<AnimationClip> animations = new List<AnimationClip>();
    protected Object[] assetAnimations;

    protected Animator animator;
    protected Animate animate;
    protected bool startAnimation = false;
    protected float endTime;
    protected bool looping;
    protected float animationLength = 0f;

    private string TAG = "AM ";

    private void Start()
    {
        animate = new Animate();
        animator = gameObject.GetComponent<Animator>();
        assetAnimations = Resources.LoadAll("_Animations", typeof(AnimationClip));
        foreach (AnimationClip a in assetAnimations)
        {
            if (a.name.ToLower() != "idle")
            {
                animations.Add(a);
            }
        }
    }

    /* Start running the animation. */
    public void PlayAnimation(Animate anim)
    {
        animate = anim;
        animate.start();
        CancelInvoke("ReplayAnimation");
        animator = gameObject.GetComponent<Animator>();
        animate.loop = anim.loop;
        animate.toIdle = anim.toIdle;
        animate.animation = anim.animation;
        animate.timeout = anim.timeout;
        Invoke("PlayAnimationDelayedTimeout", anim.delay);
    }

    /* Play animation until timeout time from Unity. */
    public void PlayAnimationDelayedTimeout()
    {
        startAnimation = true;
        if (animate.timeout > 0)
        {
            Invoke("ToIdle", animate.timeout);
        }
    }

    /* Called once on every frame */
    private void Update()
    {
        if (startAnimation)
        {
            //Debug.Log(TAG + " Playing animation: " + animate.animation);
            int index = findAnimation(animate.animation);
            if (animate.loop)
            {
                animator.SetInteger("pathIndex", index);
                startAnimation = false;
                looping = false;
                Invoke("ReplayAnimation", animationLength);
            }

            if (!animate.loop)
            {
                animator.SetInteger("pathIndex", index);
                startAnimation = false;
                if (animate.toIdle)
                {
                    Invoke("ToIdle", animationLength);
                }
            }
        }
    }

    /* Search for the animation to play in this list of animations. */
    public int findAnimation(string animation)
    {
        int index = 0;
        foreach (AnimationClip c in animations)
        {
            if (c.name.Equals(animation))
            {
                animationLength = c.length;
                break;
            }
            index++;
        }
        return index;
    }

    /* Replay the animation. */
    public void ReplayAnimation()
    {
        looping = true;
        startAnimation = true;
        animate.start();
    }

    /* Stop playing the animation. */
    public void ToIdle()
    {
        Debug.Log(TAG + name + " finished playing.");
        animator.SetInteger("pathIndex", -1);
        animate.finish();
    }
}
