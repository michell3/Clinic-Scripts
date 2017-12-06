using UnityEngine;
using System.Collections;

public class AudioScript : MonoBehaviour {

	private AudioSource sound;

	void Awake () {
		sound = GetComponent<AudioSource> ();
		sound.Play ();
		Destroy (gameObject, 5f);
	}
}
