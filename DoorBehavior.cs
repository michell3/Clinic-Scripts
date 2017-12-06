using UnityEngine;
using System.Collections;

public class DoorBehavior : MonoBehaviour {

	private GameObject hand;

	public float MinAngle = 0f;
	private float minAngle;
	public float MaxAngle = 150f;
	private float maxAngle;
	public float OffsetX = 0f;
	private float offsetX;
	public float OffsetY = 270f;
	private float offsetY;
	public float OffsetZ = 0f;
	private float offsetZ;

	public bool limitY = true;

	private Quaternion baseRotation;
	private Quaternion targetRotation;
	private bool isMovingDoor = false;

	private TriggerAudio triggerScript;

	void Awake () {
		minAngle = MinAngle;
		maxAngle = MaxAngle;
		offsetX = OffsetX;
		offsetY = OffsetY;
		offsetZ = OffsetZ;

		baseRotation = transform.rotation;
		triggerScript = GetComponent<TriggerAudio> ();
	}

	void Update () {
		if (isMovingDoor) {
			MoveDoor (hand.transform.position);
		}
	}

	private void MoveDoor(Vector3 targetPos) {
		Vector3 look = targetPos - transform.position;

		if (limitY) {
			look.y = 0f;
		}

		Quaternion q = Quaternion.LookRotation(look);
		q *= Quaternion.Euler (offsetX, offsetY, offsetZ);
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
