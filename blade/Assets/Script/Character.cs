using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {


	Rigidbody rigid;
	Animator anim;
	bool isparry;
	private GameObject equipholder;
	sowd weapon;
	bool canMove;
	bool canAttack;
	bool canCombo=false;
	bool isCombo;
	// Use this for initialization
	public float rotationSpeed = 180.0f;	
	public float speed = 6.0F;
	public float jumpSpeed = 8.0F;
	public float gravity = 20.0F;
	private Vector3 moveDirection = Vector3.zero;
	Vector3 playerDir;
	public float dashSpeed=10.0f;

	enum Battlestate{
		None,
		Normal,
		Attack,
		Combo,
	}
	[SerializeField]
	Battlestate bs;
	[SerializeField]
	bool isGround;
	RaycastHit hit;

	void Start () {
		anim = GetComponent<Animator> ();
		bs = Battlestate.None;
		rigid = GetComponent<Rigidbody> ();
		//	equipholder = GameObject.Find ("EquipHolder");
		//weapon = equipholder.GetComponentInChildren<sowd> ();
	}



	// Update is called once per frame
	void Update () {


		float v = Input.GetAxis ("Vertical");	// 上下のキー入力を取得
		float h = Input.GetAxis ("Horizontal");	// 左右のキー入力を取得

		Vector3 forward = transform.TransformDirection (Vector3.forward); 
		Vector3 right = transform.TransformDirection (Vector3.right); 


		moveDirection = h * right + v * forward; 

		playerDir = moveDirection*speed*0.7f;

		moveDirection *= speed;
		anim.SetFloat ("x",h);
		anim.SetFloat ("z",v);
		/*
		if (playerDir.magnitude > 0.1f) {
			Quaternion q = Quaternion.LookRotation (playerDir);			// 向きたい方角をQuaternionn型に直す .
			transform.rotation = Quaternion.RotateTowards (transform.rotation, q, rotationSpeed * Time.deltaTime);	// 向きを q に向けてじわ～っと変化させる.
		}
*/

		//moveDirection.y -= gravity * Time.deltaTime;

		rigid.velocity += moveDirection;


		if (Input.GetKeyDown(KeyCode.Space)) {
			Debug.Log ("Jump");
			//moveDirection.y += jumpSpeed;
			rigid.AddForce (transform.up*jumpSpeed);
		} 




		if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hit,0.2f ))
		{
			isGround = true;

		}
		else
		{

			isGround = false;


		}



		if(!isGround)
			rigid.AddForce(Physics.gravity*10);


		if (Input.GetKey ("d")) {
			rigid.AddForce (moveDirection*dashSpeed);
			anim.SetBool ("Dash",true);
		}

		if (Input.GetKeyUp ("d")) {
			rigid.velocity = Vector3.zero;
			anim.SetBool ("Dash",false);
		}


		if (Input.GetKeyDown ("k")) {
			if (canCombo) {
				anim.SetTrigger ("Combo");
				//bs = Battlestate.Combo;
				anim.SetFloat ("attack", 1.0f);
				//isCombo = true;
			} else
				//	bs = Battlestate.Attack;
				anim.SetTrigger ("Attack");
			anim.SetFloat ("attack",1.0f);
		}

		if (Input.GetKeyDown ("l")) {


			anim.SetFloat ("attack", -1.0f);
			StartCoroutine (CanselAnim ());


		}


		if (Input.GetKey("p")) {
			anim.SetBool ("parry", true);
			isparry = true;
		} else {
			anim.SetBool ("parry", false);
			isparry = false;
		}


		if (anim.GetCurrentAnimatorStateInfo (0).IsTag ("Attack")) {
			canMove = false;
			canCombo = true;

		} else {
			canMove = false;
			canCombo = false;
		}

		if (anim.GetCurrentAnimatorStateInfo (0).IsTag ("Move")) {
			isCombo = false;
			bs = Battlestate.Normal;
			//anim.SetFloat ("attack",0);
			//	anim.SetFloat ("ComboAttack",0);

		}

		if (anim.GetCurrentAnimatorStateInfo (0).IsTag ("Attack")&&anim.GetCurrentAnimatorStateInfo(0).normalizedTime>0.98f) {
			//anim.SetFloat ("attack",0);
			anim.ResetTrigger ("Attack");

			//anim.SetFloat ("ComboAttack", 0);

		} 




	}

	public void Move(Vector3 dir){

		rigid.velocity += dir;
	}



	void Parry(){
		if (isparry)
			weapon.canAttack = false;

	}

	IEnumerator CanselAnim(){
		yield return new WaitForSeconds (0.7f);
		anim.SetTrigger ("cansel");
	}

	/*
	void OnTriggerEnter(Collider col){
		if (col.gameObject.tag == "Sowd") {

			Debug.Log ("hit");
			anim.SetFloat ("attack", -1.0f);
			StartCoroutine (CanselAnim());
		}
	}
*/

















}
