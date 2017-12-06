using UnityEngine;
using System.Collections;

public class ChildrenAudio : MonoBehaviour {

	public float MaxVel = 30f;
	private float maxVel;

	private TriggerAudio triggerScript;

	private Rigidbody[] allChildren;

	public float TimerVal = 1f;
	private float timerVal;
	private float timer;

	void Awake () {
		allChildren = gameObject.GetComponentsInChildren<Rigidbody>();
		triggerScript = GetComponent<TriggerAudio> ();

		maxVel = MaxVel;

		timerVal = TimerVal;
		timer = timerVal;
	}

	void Update () {
		if (triggerScript.GetContact ()) {
			if (timer >= 0f) {
				timer -= Time.deltaTime;
			} else {
				foreach (Rigidbody rb in allChildren) {
					Vector3 vel = rb.angularVelocity;
					if (vel.magnitude > maxVel) {
						triggerScript.PlayRandomAudio (rb.position);
						timer = timerVal;
						break;
					}
				}
			}
		}
	}
}
