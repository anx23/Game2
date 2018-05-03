using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {



	Rigidbody rigid;
	Animator anim;
	bool isparry;

	bool canMove;　//移動できるか
	bool canAttack; //攻撃できるか
 	public bool canCombo=false; //コンボフラグ
//	bool isCombo;
	// Use this for initialization
	public float rotationSpeed = 360.0f;	
	public float speed = 0.5F;
	public float jumpSpeed = 2000.0F;
	public float gravity = 20.0F;
	private Vector3 moveDirection = Vector3.zero;
	Vector3 playerDir;
	 float dashSpeed=100.0f;
	public bool isDash;
	bool isRotate;
	[SerializeField]
	GameObject target; //相手
	//ダッシュエフェクト
	[SerializeField]
	GameObject dashEf;
	ParticleSystem ps;


	[SerializeField]
	bool isGround;
	RaycastHit hit;
	public bool efect=false;
	void Start () {
		anim = GetComponent<Animator> ();
	
		rigid = GetComponent<Rigidbody> ();
	
		//target = GameObject.FindWithTag ("Enemy");
		ps = GetComponentInChildren<ParticleSystem> ();
		//ps=dashEf.GetComponent<ParticleSystem>();
		ps.Stop ();
		SetTarget ();
		//ps.Play ();
	}



	public void SetTarget(){

		target = GameObject.FindWithTag ("Enemy");
	}

	// Update is called once per frame
	void Update () {
		//.移動できる場合
		if (canMove) {

			float v = Input.GetAxis ("Vertical");	// 上下のキー入力を取得
			float h = Input.GetAxis ("Horizontal");	// 左右のキー入力を取得

			Vector3 forward = transform.TransformDirection (Vector3.forward); 
			Vector3 right = transform.TransformDirection (Vector3.right); 


			moveDirection = h * right + v * forward; 

			playerDir = moveDirection * speed * 0.7f;

			moveDirection *= speed;
			anim.SetFloat ("x", h);
			anim.SetFloat ("z", v);
			/*
		if (playerDir.magnitude > 0.1f) {
			Quaternion q = Quaternion.LookRotation (playerDir);			// 向きたい方角をQuaternionn型に直す .
			transform.rotation = Quaternion.RotateTowards (transform.rotation, q, rotationSpeed * Time.deltaTime);	// 向きを q に向けてじわ～っと変化させる.
		}
*/

			//moveDirection.y -= gravity * Time.deltaTime;

			rigid.velocity += moveDirection;


			if (Input.GetKeyDown (KeyCode.Space)) {
				Debug.Log ("Jump");
				anim.applyRootMotion = true;
				anim.SetTrigger ("Jump");
				//moveDirection.y += jumpSpeed;
				rigid.AddForce (transform.up * jumpSpeed);
			} 

		}


		if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hit,0.2f ))
		{
			isGround = true;

		}
		else
		{

			isGround = false;


		}

		if(isRotate)
		LookAtTarget ();


		if(!isGround)
			rigid.AddForce(Physics.gravity*10);  //重力調整

		//ダッシュ
		if (Input.GetKey ("f")||Input.GetMouseButton(1)) {
			rigid.AddForce (moveDirection*dashSpeed);
			anim.SetBool ("Dash",true);
			isDash = true;
			//ps.Play ();
		}

		if (Input.GetKeyUp ("f")||Input.GetMouseButtonUp(1)) {
			rigid.velocity = Vector3.zero;
			anim.SetBool ("Dash",false);
			isDash = false;
			ps.Stop ();
		}

		//攻撃
		if (Input.GetKeyDown ("k")||Input.GetMouseButtonDown(0)) {

			anim.ResetTrigger ("cansel");
			if (GetComponentInChildren<sowd> ().isParried)
				//StopCoroutine (GetComponentInChildren<sowd> ().CanselAnim());
				anim.SetTrigger ("ParryCombo");

			if (isDash) {
				anim.SetTrigger ("DashAttack");

			}
			else if (canCombo) {
				anim.SetFloat ("attack", 1.0f);
				anim.SetTrigger ("Combo");
				//bs = Battlestate.Combo;
				anim.SetFloat ("attack", 1.0f);
				//isCombo = true;
			} else
				//	bs = Battlestate.Attack;
				anim.SetTrigger ("Attack");
			anim.SetFloat ("attack",1.0f);
		}


		/*
		if (Input.GetKey("p")) {
			anim.SetBool ("parry", true);

			isparry = true;
		} else {
			anim.SetBool ("parry", false);

			isparry = false;
		}
*/
		//アニメーション管理
		if (anim.GetCurrentAnimatorStateInfo (0).IsTag ("Attack")) {
			canMove = false;
			canCombo = true;
			anim.applyRootMotion = true;

		} else {

			canCombo = false;
			anim.applyRootMotion = false;
		}

		if (anim.GetCurrentAnimatorStateInfo (0).IsTag ("Move")) {
			canCombo = false;
			canMove = true;
			anim.ResetTrigger ("Combo");

			//anim.SetFloat ("attack",0);
			//	anim.SetFloat ("ComboAttack",0);

		} else {

		}


		if (anim.GetCurrentAnimatorStateInfo (0).IsTag ("Move")||anim.GetCurrentAnimatorStateInfo (0).IsName ("Dash")) {

		

			isRotate = true;
		} else {
			
			isRotate = false;
		}




		if (anim.GetCurrentAnimatorStateInfo (0).IsTag ("Attack")&&anim.GetCurrentAnimatorStateInfo(0).normalizedTime>0.98f) {
			//anim.SetFloat ("attack",0);
			anim.ResetTrigger ("Attack");
			anim.ResetTrigger ("cansel");
			//anim.SetFloat ("ComboAttack", 0);

		} 


		if (anim.GetCurrentAnimatorStateInfo (0).IsTag ("Attack")&&anim.GetCurrentAnimatorStateInfo(0).normalizedTime>0.98f) {
			//anim.SetFloat ("attack",0);
			anim.ResetTrigger ("Attack");
			anim.ResetTrigger ("cansel");
			//anim.SetFloat ("ComboAttack", 0);

		} 

		if (anim.GetCurrentAnimatorStateInfo (0).IsTag ("Attack")&&anim.GetCurrentAnimatorStateInfo(0).normalizedTime<0.1f) {
			//anim.SetFloat ("attack",0);
			//anim.ResetTrigger ("Attack");
			//anim.ResetTrigger ("cansel");
			//anim.SetFloat ("ComboAttack", 0);

		} 

		if (anim.GetCurrentAnimatorStateInfo (0).IsName ("DashAttack")) {
			rigid.AddForce (transform.forward * 40);

		}


		if (anim.GetCurrentAnimatorStateInfo (0).IsName("Jump")&&anim.GetCurrentAnimatorStateInfo(0).normalizedTime>0.98f) {
			//anim.SetFloat ("attack",0);
			anim.applyRootMotion=false;

			//anim.SetFloat ("ComboAttack", 0);

		} 

		if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Dash")) {
			//Debug.Log (rigid.velocity);

			if (rigid.velocity != Vector3.zero) {
				efect = true;
			} else {
				efect = false;
			}

			//	anim.SetFloat ("ComboAttack",0);

		} else {
			efect = false;
			//ps.Stop ();
		}
		if (efect)
			ps.Play ();
		else
			ps.Stop ();
	}

	//スムーズに回転
	void LookAtTarget(){
		//SetTarget ();
		Vector3 LookDir = target.transform.position - transform.position;
		LookDir.y = 0;
		Quaternion rot = Quaternion.LookRotation (LookDir);
		transform.rotation= Quaternion.Slerp (transform.rotation,rot,Time.deltaTime*2.0f);

	}


	void Parry(){
		
			//weapon.canAttack = false;

	}
	/*
	IEnumerator CanselAnim(){
		yield return new WaitForSeconds (0.7f);
		anim.SetTrigger ("cansel");
	}
*/
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
