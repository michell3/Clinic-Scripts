using UnityEngine;
using System;
using System.Collections;

public class SecondHand : MonoBehaviour {

	private AudioSource clockSound;
	private float increment;
	private float prevSeconds;
	private float totalSeconds;
	TimeSpan timespan;

	void Awake () {
		clockSound = GetComponent<AudioSource> ();
		increment = 6f;

		timespan = DateTime.Now.TimeOfDay;
		totalSeconds = Mathf.Floor ((float)timespan.TotalSeconds);
		transform.localRotation = Quaternion.Euler (totalSeconds * -increment, 0f, 0f);
		prevSeconds = totalSeconds;
	}

	void FixedUpdate () {
		timespan = DateTime.Now.TimeOfDay;
		totalSeconds = Mathf.Floor ((float)timespan.TotalSeconds);
		transform.localRotation = Quaternion.Euler (totalSeconds * -increment, 0f, 0f);

		if (totalSeconds - prevSeconds >= 2f) {
			clockSound.Play ();
			prevSeconds = totalSeconds;
		}
	}
}
