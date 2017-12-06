using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainScript : MonoBehaviour {

	public float JointMass = 1f;
	public float JointDrag = 0f;
	public float JointAngularDrag = 0.05f;
	public float ColliderSizeX = 0.01f;
	public float ColliderSizeY = 0.01f;
	public float ColliderSizeZ = 0.01f;
	public bool RootIsKinematic = true;
	public string ChainAxis = "Y";

	public bool UseSpring = true;
	public Vector3 AxisDirection = new Vector3(1f, 0f, 0f);
	public float Damper = 10f;
	public float Spring = 10f;
	public float TargetPosition = 50f;

	private Transform[] jointChain;

	void Awake () {

		jointChain = GetComponentsInChildren<Transform>();

		Rigidbody rb;
		BoxCollider col;
		HingeJoint hj;

		foreach (Transform joint in jointChain) {
			rb = joint.gameObject.AddComponent<Rigidbody>();
			rb.mass = JointMass;
			rb.drag = JointDrag;
			rb.angularDrag = JointAngularDrag;
			joint.parent = transform.parent;
		}

		if (RootIsKinematic) {
			jointChain[0].gameObject.GetComponent<Rigidbody>().isKinematic = true;
		}

		int numJoints = jointChain.Length;
		for (int i = 0; i < numJoints - 1; i++) {
			hj = jointChain[i].gameObject.AddComponent<HingeJoint>();
			hj.connectedBody = jointChain[i + 1].gameObject.GetComponent<Rigidbody>();
			hj.axis = AxisDirection;

			JointSpring hjSpring = hj.spring;
			hjSpring.spring = Spring;
			hjSpring.damper = Damper;
			hjSpring.targetPosition = TargetPosition;
			hj.spring = hjSpring;
			hj.useSpring = UseSpring;

			col = jointChain[i].gameObject.AddComponent<BoxCollider>();
			col.size = new Vector3(ColliderSizeX, ColliderSizeY, ColliderSizeZ);
			if (ChainAxis == "X") {
				col.center = new Vector3(-ColliderSizeX / 2.0f, 0f, 0f);
			} else if (ChainAxis == "Y") {
				col.center = new Vector3(0f, -ColliderSizeY / 2.0f, 0f);
			} else {
				col.center = new Vector3(0f, 0f, -ColliderSizeZ / 2.0f);
			}
		}

		for (int i = numJoints - 1; i > 0; i--) {
			hj = jointChain[i].gameObject.AddComponent<HingeJoint>();
			hj.connectedBody = jointChain[i - 1].gameObject.GetComponent<Rigidbody>();
			hj.axis = -1f * AxisDirection;

			JointSpring hjSpring = hj.spring;
			hjSpring.spring = Spring;
			hjSpring.damper = Damper;
			hjSpring.targetPosition = TargetPosition;
			hj.spring = hjSpring;
			hj.useSpring = UseSpring;
		}
	}
}