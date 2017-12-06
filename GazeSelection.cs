using UnityEngine;
using System.Collections;

public class GazeSelection : MonoBehaviour {

	public GameObject Mannequin;
	private GameObject mannequin;
	private Animator mannequinAnimator;
	public GameObject AudioObject;
	private AudioSource mannequinAudio;

	private bool isOpening;

	void Start () {
		mannequin = Mannequin;
		mannequinAnimator = mannequin.GetComponent<Animator> ();
		mannequinAudio = AudioObject.GetComponent<AudioSource> ();

		isOpening = false;
	}

	void Update () {
		
		Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
		RaycastHit hit;

		if (Physics.Raycast (ray, out hit, 50)) {

			string hitObjectName = hit.collider.gameObject.name;
//			Debug.Log ("hitObject: " + hitObjectName);
			if (!isOpening && hitObjectName == "char_skin") {
				mannequinAnimator.SetTrigger ("Open");
				isOpening = true;
				mannequinAudio.Play ();
			}
		}
	}
}