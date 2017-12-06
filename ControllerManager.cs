using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ControllerManager : MonoBehaviour {

	// Hand avatar variables
	public GameObject Hand;
	private GameObject hand;
	private Animator handAnimator;

	// Hand sound variables
	public GameObject GrabSound;
	private GameObject grabSound;
	public GameObject ReleaseSound;
	private GameObject releaseSound;
	private TriggerAudio triggerScript;

	// Hand animation state variables
	private bool isGrabbing;
	private GameObject grabbedObject;

	// Vive device variables
	private SteamVR_TrackedObject trackedObject;
	private SteamVR_Controller.Device device;
	private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;

	// Haptic variables
	private ushort pulsePower = 500;

	void Awake () {
		trackedObject = GetComponent<SteamVR_TrackedObject>();

		hand = Hand;
		handAnimator = hand.GetComponent<Animator> ();

		isGrabbing = false;

		grabSound = GrabSound;
		releaseSound = ReleaseSound;

		triggerScript = GetComponent<TriggerAudio> ();
	}

	void FixedUpdate () {
		
		device = SteamVR_Controller.Input((int) trackedObject.index);

		// Grab animation and sound
		if (!isGrabbing && device.GetTouch (triggerButton)) {
			isGrabbing = true;
			Instantiate (grabSound, transform.position, new Quaternion ());
			handAnimator.SetTrigger ("Grab");
		}
		// Release animation, play, sound, and release object
		if (isGrabbing && device.GetTouchUp (triggerButton)) {
			
			isGrabbing = false;
			Instantiate (releaseSound, transform.position, new Quaternion ());
			handAnimator.SetTrigger ("Release");
			isGrabbing = false;

			if (grabbedObject != null) {
				if (grabbedObject.name.Contains ("door") || grabbedObject.name.Contains ("lamp_head")) {
					DoorBehavior doorScript = grabbedObject.GetComponent<DoorBehavior> ();
					doorScript.ReleaseDoor ();
					grabbedObject = null;
				} else {
					grabbedObject.transform.SetParent (null);
					Rigidbody rb = grabbedObject.GetComponent<Rigidbody> ();
					rb.isKinematic = false;
					tossObject (rb);
					grabbedObject = null;
				}
			}
		}

		// Haptics based on animation state
		AnimatorStateInfo animState = handAnimator.GetCurrentAnimatorStateInfo(0);
		if (animState.IsName ("Grab") || animState.IsName ("Release")) {
			device.TriggerHapticPulse (pulsePower);
		}

		// Reset scene
		if (device.GetPressUp (SteamVR_Controller.ButtonMask.Touchpad)) {
			SceneManager.LoadScene ("Main");
		}
	}

	void OnTriggerStay (Collider col) {

		if (device == null) return;

		// Grab a single object
		if (device.GetTouch (triggerButton) && (col.attachedRigidbody != null)
			&& (grabbedObject == null)) {

			if (!col.attachedRigidbody.isKinematic) {
				grabbedObject = col.gameObject;
				Debug.Log ("grabbedObject: " + grabbedObject.name);

				col.attachedRigidbody.isKinematic = true;
				grabbedObject.transform.SetParent (gameObject.transform);

				TriggerAudio grabbedTriggerScript = grabbedObject.GetComponent<TriggerAudio> ();
				if (grabbedTriggerScript != null) {
					grabbedTriggerScript.MakeContact ();
				}

			} else if (col.gameObject.name.Contains("door") || col.gameObject.name.Contains("lamp_head")) {
				grabbedObject = col.gameObject;
				Debug.Log ("grabbedObject: " + grabbedObject.name);

				DoorBehavior doorScript = grabbedObject.GetComponent<DoorBehavior>();
				doorScript.GrabDoor (gameObject);
			}
		}
//		// Release/toss an object
//		if ((col.gameObject == grabbedObject) && device.GetTouchUp (triggerButton)) {
//
//			if (grabbedObject.name.Contains ("door")) {
//				DoorBehavior doorScript = grabbedObject.GetComponent<DoorBehavior>();
//				doorScript.ReleaseDoor ();
//				grabbedObject = null;
//			} else {
//				grabbedObject.transform.SetParent (null);
//				col.attachedRigidbody.isKinematic = false;
//				tossObject (col.attachedRigidbody);
//				grabbedObject = null;
//			}
//		}
	}

	void tossObject(Rigidbody rigidBody) {
		// Toss an object
		if (rigidBody == null) return;

		// Play a whoosh sound
		triggerScript.PlayRandomAudio(transform.position);

		Transform origin = trackedObject.origin ? trackedObject.origin : trackedObject.transform.parent;
		if (origin != null) {
			rigidBody.velocity = origin.TransformVector (device.velocity);
			rigidBody.angularVelocity = origin.TransformVector (device.angularVelocity);
		} else {
			rigidBody.velocity = device.velocity;
			rigidBody.angularVelocity = device.angularVelocity;
		}
	}
}
