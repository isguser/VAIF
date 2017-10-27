using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public List<AnimationClip> animations = new List<AnimationClip>();
    protected Object[] assetAnimations;

    protected Animator animator;
    protected Animate animate;
    protected bool startAnimation = false;
    protected float endTime;
    protected bool looping;
    protected float animationLength = 0f;
    
    private void Start()
    {
        animate = new Animate();
        animator = gameObject.GetComponent<Animator>();
        assetAnimations = Resources.LoadAll("_Animations", typeof(AnimationClip));
        foreach (AnimationClip a in assetAnimations)
        {
            if(a.name.ToLower() != "idle")
            {
                animations.Add(a);
            }
        }
    }

    
    public void PlayAnimation(Animate anim)
    {
        CancelInvoke("ReplayAnimation");
        animator = gameObject.GetComponent<Animator>();
        animate.loop = anim.loop;
        animate.toIdle = anim.toIdle;
        animate.animation = anim.animation;
        animate.timeout = anim.timeout;
        Invoke("PlayAnimationDelayedTimeout", anim.delay);
    }

    public void PlayAnimationDelayedTimeout()
    {
        startAnimation = true;
        if (animate.timeout > 0)
        {
            Invoke("ToIdle", animate.timeout);
        }
    }

    private void Update()
    {
        if (startAnimation)
        {
            Debug.Log("Playing animation: " + animate.animation);
            int index = findAnimation(animate.animation);
            if (looping)
            {
                Debug.Log(index);
                animator.SetInteger("pathIndex", index);
                startAnimation = false;
                looping = false;
                Invoke("ReplayAnimation", animationLength);
            }

            if(!looping)
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

    public void ReplayAnimation()
    {
        looping = true;
        startAnimation = true;
    }

	public void ToIdle()
    {
        animator.SetInteger("pathIndex", -1);
	}
}
