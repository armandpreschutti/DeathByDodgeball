using UnityEngine;

public class PawnAnimationEventHandler : MonoBehaviour
{
  /*  Animator animator;
    

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        // Find the animation clip by name in the Animator
        AnimationClip clip = animator.GetCurrentAnimatorClipInfo(0);

        if (clip != null)
        {
            Debug.LogError("Found dodge aniation clip");
            // Create and add the AnimationEvent
            AnimationEvent animEvent = new AnimationEvent
            {
                time = 1.5f, // Set the time (in seconds) for the event
                functionName = "OnAnimationEvent", // Function to call
                stringParameter = "Hello, World!" // Optional parameter
            };

            clip.AddEvent(animEvent);
        }
        else
        {
            Debug.LogError("Animation clip not found!");
        }
    }

    AnimationClip GetAnimationClipByName(string clipName)
    {
        foreach (var clip in animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == clipName)
            {
                return clip;
            }
        }
        return null;
    }

    void OnAnimationEvent(string message)
    {
        Debug.Log("Animation Event Triggered: " + message);
    }*/
}
