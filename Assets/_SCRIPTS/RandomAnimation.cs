using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAnimation : MonoBehaviour {
	public List<AnimationClip> animations;
	void Start () {
		Animator animator = GetComponent<Animator>();
		if (!animator) {
			Debug.LogError("Missing animator for random animation");
			return;
		}
		if (animations.Count == 0) {
			Debug.LogError("No animations for random animation");
			return;
		}
		AnimatorOverrideController overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
		animator.runtimeAnimatorController = overrideController;
		
		overrideController["Idle1"] = animations[Random.Range(0, animations.Count)];
	}
}
