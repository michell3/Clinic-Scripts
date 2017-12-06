using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TriggerAudio : MonoBehaviour {

	public GameObject SoundPrefab1;
	public GameObject SoundPrefab2;
	public GameObject SoundPrefab3;
	public GameObject SoundPrefab4;

	private List<GameObject> soundList = new List<GameObject>();

	public bool IsCollider;
	private bool isCollider;

	public float TimerVal = 0.5f;
	private float timerVal;
	private float timer;

	public bool ContactMade = false;
	private bool contactMade;

	void Awake () {
		if (SoundPrefab1 != null) soundList.Add (SoundPrefab1);
		if (SoundPrefab2 != null) soundList.Add (SoundPrefab2);
		if (SoundPrefab3 != null) soundList.Add (SoundPrefab3);
		if (SoundPrefab4 != null) soundList.Add (SoundPrefab4);

		isCollider = IsCollider;

		timerVal = TimerVal;
		timer = timerVal;

		contactMade = ContactMade;
	}

	void Update() {
		if (timer >= 0f) {
			timer -= Time.deltaTime;
		}
	}

	void OnCollisionEnter(Collision col) {
		if (contactMade && (timer < 0f) && isCollider) {
			ContactPoint contact = col.contacts [0];
			Vector3 pos = contact.point;
			PlayRandomAudio (pos);
			timer = timerVal;
		}
	}

	public void MakeContact() {
		contactMade = true;
	}

	public bool GetContact() {
		return contactMade;
	}

	public void PlayRandomAudio(Vector3 pos) {
		int randomIndex = (int) Mathf.Floor(Random.value * soundList.Count);
		Instantiate (soundList[randomIndex], pos, new Quaternion ());
	}

	public void PlayAudioIndex(int i) {
		Instantiate (soundList[i], transform.position, new Quaternion ());
	}
}
