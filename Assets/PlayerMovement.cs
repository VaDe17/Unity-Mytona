using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour {

	Rigidbody rb;
	Transform tr;
	float h;
	float v;
	float mx;
	bool j;
	bool ground = false;
	float JH =60f;
	float BPC = 1;
	float BPJH;
	float MSF=400f;
	float MSR=250f;
	float LvlIndex = 1;


	float XSensitivity = 5f;
	float YSensitivity = 5f;
	bool clampVerticalRotation = true;
	float MinimumX = -90F;
	float MaximumX = 90F;
	bool smooth;
	float smoothTime = 5f;

	private Quaternion m_CharacterTargetRot;
	private Quaternion m_CameraTargetRot;  

	public Transform camera;

	void Start () {
		rb = GetComponent<Rigidbody> ();
		tr = GetComponent<Transform> ();
		m_CharacterTargetRot = tr.localRotation;
		m_CameraTargetRot = camera.localRotation;
		Cursor.visible = false;

	}


	void Update () {
		v = Input.GetAxis ("Vertical");
		h = Input.GetAxis ("Horizontal");
		float yRot = Input.GetAxis("Mouse X") * 3;
		float xRot = Input.GetAxis("Mouse Y") * 3;

		m_CharacterTargetRot *= Quaternion.Euler (0f, yRot, 0f);
		m_CameraTargetRot *= Quaternion.Euler (-xRot, 0f, 0f);

		rb.AddForce (tr.forward * v * MSF);
		rb.AddForce (tr.right * h * MSR);

		if (Input.GetKeyDown (KeyCode.Space) == true && ground == true) {
			Vector3 jump = new Vector3 (0, JH, 0);
			rb.AddForce(jump, ForceMode.Impulse);
			ground = false;
		}
		if (Input.GetKeyDown (KeyCode.S) == true) {
			Vector3 moveBack = new Vector3 (0, MSF/2, 0);
			rb.AddForce(moveBack);
		}


		tr.localRotation = Quaternion.Slerp (tr.localRotation, m_CharacterTargetRot,smoothTime * Time.deltaTime);
		camera.localRotation = Quaternion.Slerp (camera.localRotation,	m_CameraTargetRot, smoothTime * Time.deltaTime );
			
		rb.AddForce(Physics.gravity * rb.mass *4);
		
	}

	void OnCollisionEnter(Collision arg){
		if (arg.gameObject.tag == "Floor") {
			ground = true;
			BPC = 0;
			MSF = 400f;
			MSR = 250f;

		}
		if (arg.gameObject.tag == "PK") {
			Application.LoadLevel("level1");
			LvlIndex = 1;
		}
		if (arg.gameObject.tag == "BP") {
			ground = false;
			if (BPC < 2) {
				BPC++;
			}
			BPJH = JH * BPC;
			Vector3 jump = new Vector3 (0, BPJH , 0);
			rb.AddForce(jump, ForceMode.Impulse);
		}
		if (arg.gameObject.tag == "SB") {
			MSR = 1000f;
			MSF = 350f;
			ground = true;
		}
		if (arg.gameObject.tag == "Finish") {
			nextLvl ();
		}
		if (arg.gameObject.tag == "Door") {
			Destroy (arg.gameObject);
		}
	}

	void nextLvl() {
		LvlIndex= LvlIndex+1;
		Application.LoadLevel( "level"+LvlIndex);
	}
}
