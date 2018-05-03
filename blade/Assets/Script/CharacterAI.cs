using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAI : MonoBehaviour {

	[SerializeField]
	GameObject target;
	bool isRunning;
	Animator anim;
	bool canCombo;
	Rigidbody rigid;
	public float speed =1.0f;
	float walkSpeed=0.5f;
	float dashSpeed=2.0f;
	Vector3 direction;
	bool isMove=false;
	Vector3 dir;
	float time;
	float count;
	bool isDash=false;
	bool isRotate;

	//[SerializeField]
	//GameObject dashEf;
	ParticleSystem ps;
	public bool efect=false;
	bool canMove;
	bool isChase;
	public enum CombatType
	{
		near, //近距離
		mid, //中距離
		far, //遠距離
		//offensive, 
		//deffensive,
		normal,

	}

	public CombatType cState=CombatType.normal;






	// Use this for initialization
	void Start () {
		rigid = GetComponent<Rigidbody> ();
	target = GameObject.FindWithTag ("Player");
		anim = GetComponent<Animator> ();
		ps = GetComponentInChildren<ParticleSystem> ();
		//ps = dashEf.GetComponent<ParticleSystem> ();
		ps.Stop();
		SetTarget ();
	}



	public void SetTarget(){

		target = GameObject.FindWithTag ("Player");
	}

	
	// Update is called once per frame
	void Update () {
		
		Vector3 playerPos = target.transform.position;    //プレイヤーの位置
		 direction = playerPos - transform.position; //方向
		direction = direction.normalized;   //単位化（距離要素を取り除く）

		anim.SetFloat("x",rigid.velocity.x);
		anim.SetFloat("z",rigid.velocity.z);
		float distanse=Vector3.Distance (playerPos, transform.position) ; //相手との距離


		if (distanse <= 0.8f) {
			if(dir==direction){
			isMove = false;
			DashEnd ();
			rigid.velocity = Vector3.zero;
			//StopCoroutine (MidAction());
			StopCoroutine (FarAction());
			//isChase = false;
			}
		}
		if (distanse<=1) {
			isChase = true;
			cState = CombatType.near;
			StopCoroutine (MidAction());
		}
		else if(distanse<=5){

			cState = CombatType.mid;
		}
		else {
			cState = CombatType.far;

		}

		//combat
		switch (cState) {
		case CombatType.near:
			
			StartCoroutine (NearAction());
			break;
		case CombatType.mid:
			StartCoroutine (MidAction ());
			break;
		case CombatType.far:
			StartCoroutine (FarAction());

			FollwTarget(direction);
			break;
		}


		//FollwTarget ();

		if (isMove)
			FollwTarget (dir);

		LookAtTarget();





		if (anim.GetCurrentAnimatorStateInfo (0).IsTag ("Attack")) {
			canMove = false;
			canCombo = true;
			anim.applyRootMotion = true;
		} else {

			canCombo = false;
			anim.applyRootMotion = false;
		}

		if (anim.GetCurrentAnimatorStateInfo (0).IsTag ("Move")) {
			canCombo=false;
			canMove = true;
			anim.ResetTrigger("Combo");


		}

		if (anim.GetCurrentAnimatorStateInfo (0).IsName("DashAttack")) {
			rigid.AddForce (transform.forward*40);

		}


		//移動管理
		if (anim.GetCurrentAnimatorStateInfo (0).IsTag ("Move")||anim.GetCurrentAnimatorStateInfo (0).IsName ("Dash")) {

			canMove = true;

			isRotate = true;
		} else {
			canMove = false;
			isRotate = false;
		}

		if (!canMove)
			rigid.velocity = Vector3.zero;


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

		//近距離行動
	IEnumerator NearAction(){
		if (isRunning)
			yield break;
		isRunning = true;

		int rand = Random.Range (0,100);
		//int attacktpe = Random.Range (0,3);
		yield return new WaitForSeconds (0.4f);


		if (rand < 80) {


			//攻撃
			//コンボ
			if (canCombo) {
				anim.SetTrigger ("Combo");
				//bs = Battlestate.Combo;
				anim.SetFloat ("attack", 1.0f);
				//isCombo = true;
			} else {
				//	bs = Battlestate.Attack;
				anim.SetTrigger ("Attack");
				anim.SetFloat ("attack", 1.0f);
			}
			yield return new WaitForSeconds (0.2f);

		}
		else if(rand<100){
			//後退
			DashStart (transform.forward*-1.0f);
			//rigid.velocity += transform.forward * -speed*Time.deltaTime;
			//dir=transform.forward*-1.0f;
			//rigid.AddForce(transform.forward*-1000*Time.deltaTime);
			yield return new WaitForSeconds (3.0f);
			DashEnd();

		}




		isRunning = false;
	}

	//中距離行動
	IEnumerator MidAction(){
		
		if (isRunning)
			yield break;
		isRunning = true;

		//yield return new WaitForSeconds (8.0f);

		int rand = Random.Range (0,100);
		//int attacktpe = Random.Range (0,3);
		//yield return new WaitForSeconds (1.0f);


		if (rand < 30) {
		//	isMove = true;
			//dir = direction;
			//近づく

			DashStart (direction);
		
			//FollwTarget (direction);
			Debug.Log ("slow");
			yield return new WaitForSeconds (1.0f);
			//isMove = false;
			DashEnd();

		}
		else if(rand<60){
			//ダッシュ攻撃
			DashStart (direction);
			anim.SetTrigger ("DashAttack");
			//FollwTarget (direction);
			Debug.Log ("slow");
			yield return new WaitForSeconds (1.0f);
			//isMove = false;
			DashEnd();

		}
		else if(rand<100){
			//旋回
			int dirnum= Random.Range (0,2);
			Debug.Log ("dash");
			if(!rightObs)
			if (dirnum == 1) {
				//rigid.velocity += transform.right * speed;
				//isMove=true;
				//anim.SetTrigger ("Dash");
			//	dir = transform.right;
				DashStart (transform.right);
				yield return new WaitForSeconds (2.0f);
				//isMove = false;
				//anim.ResetTrigger ("Dash");
				DashEnd();
			}

			if(!leftObs)
			if (dirnum == 2) {
				//rigid.velocity += transform.right * -speed;
				//isMove=true;
				//dir = transform.right;
				DashStart(transform.right*-1.0f);
				yield return new WaitForSeconds (2.0f);
				//isMove = false;
				DashEnd();
			}

		}




		isRunning = false;
	}




	//遠距離行動
	IEnumerator FarAction(){

		if (isRunning)
			yield break;
		isRunning = true;

		//yield return new WaitForSeconds (8.0f);

		int rand = Random.Range (0,100);
		//int attacktpe = Random.Range (0,3);
		yield return new WaitForSeconds (2.0f);

		//ダッシュ
		if (rand < 60) {
			DashStart (direction);
			//rigid.AddForce(transform.forward*-1000*Time.deltaTime);
			yield return new WaitForSeconds (1.0f);
			DashEnd ();
		}





		isRunning = false;
	}






	void LookAtTarget(){
		SetTarget ();
		Vector3 LookDir = target.transform.position - transform.position;
		LookDir.y = 0;
		Quaternion rot = Quaternion.LookRotation (LookDir);
		transform.rotation= Quaternion.Slerp (transform.rotation,rot,Time.deltaTime*2.0f);

	}






	void FollwTarget(Vector3 dir){

			//rigid.MovePosition(target.transform.position*speed*Time.deltaTime);

			rigid.velocity += dir * speed;
	
		if (isDash)
			anim.SetTrigger ("Dash");

	}



	void DashStart(Vector3 dir){
		
		anim.SetFloat("dx",dir.x);
		anim.SetFloat("dz",dir.z);
		Debug.Log ("Dash");
		anim.SetTrigger ("Dash");
		isMove = true;
		speed = dashSpeed;
		//rigid.velocity += transform.forward * -speed*Time.deltaTime;
		this.dir=dir;
		//rigid.AddForce(transform.forward*-1000*Time.deltaTime);


	}

	void DashEnd(){
		anim.ResetTrigger("Dash");
		isMove = false;
		speed = walkSpeed;

	}
	bool rightObs=false;
	bool leftObs=false;
	int layerMask=1<<8;
	RaycastHit ray;
	void SearchWall(){
		


		if (Physics.Raycast (transform.position, transform.right, 1.0f, layerMask)) 
			rightObs = true;
			else
				rightObs=false;
			

		if (Physics.Raycast (transform.position, transform.right*-1.0f, 1.0f, layerMask)) 
			leftObs = true;
		else
			leftObs=false;
	}


}
