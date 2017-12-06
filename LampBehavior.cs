using UnityEngine;
using System.Collections;

public class LmpBehavior : MonoBehaviour {

	private GameObject hand;

	public float MinAngle = -90f;
	private float minAngle;
	public float MaxAngle = 90f;
	private float maxAngle;
	public float OffsetAngle = 90f;
	private float offsetAngle;

	private Quaternion baseRotation;
	private Quaternion targetRotation;
	private bool isMovingDoor = false;

	private TriggerAudio triggerScript;

	void Awake () {
		minAngle = MinAngle;
		maxAngle = MaxAngle;
		offsetAngle = OffsetAngle;

		baseRotation = transform.rotation;
		triggerScript = GetComponent<TriggerAudio> ();
	}

	void Update () {
		if (isMovingDoor) {
			MoveLamp (hand.transform.position);
		}
	}

	private void MoveLamp(Vector3 targetPos) {
		Vector3 look = targetPos - transform.position;
//		look.y = 0f;

		Quaternion q = Quaternion.LookRotation(look);
		q *= Quaternion.Euler (0f, offsetAngle, 0f);
		if (minAngle <= Quaternion.Angle(q, baseRotation) &&
			Quaternion.Angle(q, baseRotation) <= maxAngle) {
			targetRotation = q;
			transform.rotation = targetRotation;
		}
	}

	public void GrabDoor(GameObject Hand) {
		hand = Hand;
		isMovingDoor = true;
		triggerScript.PlayAudioIndex (0);
	}

	public void ReleaseDoor() {
		isMovingDoor = false;
	}
}
