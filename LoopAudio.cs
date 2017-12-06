using UnityEngine;
using System.Collections;

public class LoopAudio : MonoBehaviour {

	private AudioSource sound;

	void Awake () {
		sound = GetComponent<AudioSource>();
		sound.loop = true;
	}

	void FixedUpdate () {
		if (sound) {
			if (!sound.isPlaying) {
				sound.Play();
			}
			sound.loop = true;
		}
	}
}
