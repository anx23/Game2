using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sowd : MonoBehaviour {

	public float attack = 5;
	public bool canAttack;
	public GameObject bloodeffect;
	Animator anim;
	public bool isplayer=true;
	// Use this for initialization
	[SerializeField]
	GameObject owner;

	private AudioSource audioSourse;

	[SerializeField]
	public AudioClip audio1;
	public AudioClip audio2;

	public bool isGuard=false;
	public bool isParried=false;
	bool isImpact;

	void Start () {
		owner = transform.root.gameObject;
		anim = owner.GetComponent<Animator> ();
		audioSourse = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("k")) {
			//anim.SetFloat ("attack",0);
			StopCoroutine(CanselAnim());
			anim.ResetTrigger ("cansel");
			//anim.SetFloat ("ComboAttack", 0);

		} 

		if (anim.GetCurrentAnimatorStateInfo (0).IsName ("DashAttack")) {
			isImpact = true;
		}
		if (anim.GetCurrentAnimatorStateInfo (0).IsTag ("Move")) {
			isImpact = false;
		}

	}




	void OnTriggerEnter ( Collider col ) {



		if (col.gameObject.tag == "Sowd") {

			Debug.Log ("hit");
			anim.SetFloat ("attack", -1.0f);
			StartCoroutine (CanselAnim());
			audioSourse.PlayOneShot (audio1);
		}

	
			if (isplayer) {
				if (col.gameObject.tag == "Enemy") {
					//プレイヤーと衝突した時
					Attack (col.gameObject); //攻撃する
					audioSourse.PlayOneShot (audio2);
				if(isImpact)
					col.GetComponent<Rigidbody>().AddForce(col.transform.forward*-3000);
				}
			} else {
				if (col.gameObject.tag == "Player") {
					//プレイヤーと衝突した時
					Attack (col.gameObject); //攻撃する
					//col.GetComponent<Rigidbody>().AddForce(col.transform.forward*-1000);
					Debug.Log ("hit");
				if(isImpact)
					col.GetComponent<Rigidbody>().AddForce(col.transform.forward*-3000);
					audioSourse.PlayOneShot (audio2);
				}

			}




	}

	//跳ね返し
	public IEnumerator CanselAnim(){
		isParried = true;
		yield return new WaitForSeconds (1.0f);
		isParried = false;
		anim.SetTrigger ("cansel");

	}




	/*

	void OnCollisionEnter ( Collision col ) {


		if (isplayer) {
			if (col.gameObject.tag == "Enemy") {
				//プレイヤーと衝突した時
				Attack (col.gameObject); //攻撃する
				Debug.Log("hit");
			}
		} else {
			if(col.gameObject.tag == "Player"){
				//プレイヤーと衝突した時
				Attack(col.gameObject); //攻撃する
				Debug.Log("hit");
			}

		}

	}
*/







	//攻撃する際に呼び出す（なんとなくpublicにしてある）


	public void Attack ( GameObject hit ){
		hit.gameObject.SendMessage("Damage", attack);   //相手の"Damage"関数を呼び出す
	}




}
