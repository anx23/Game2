using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Playe_Stautus : MonoBehaviour {

	public int level=1;
	public int maxhp=100;
	public int maxmp=100;
	public int hp=100;
	public int mp=100;
	public Slider hpvar;
	public Slider mpvar;
	public Text levelText;

	Animator anim;
	public	bool isPlayer=true;
	bool isDead;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		if (isPlayer)
			hpvar=GameObject.Find ("PlayerHp").GetComponent<Slider>();
		else
			hpvar=GameObject.Find ("EnemyHp").GetComponent<Slider>();
		
	
		//SetLevel ();
		//StartCoroutine ("MpUp");
	}
	
	// Update is called once per frame
	void Update () {
		if(hpvar!=null)
		hpvar.value = hp;
		if(mpvar!=null)
		mpvar.value = mp;
		if(levelText!=null)
		levelText.text = level.ToString();

	}



	IEnumerator MpUp(){
		while (true) {

			yield return new WaitForSeconds (1.0f);
			if (mp < 100)
				mp += 1;


		}
	}







	void LevelUp(){

		level++;
		SetLevel ();
	}

	void SetLevel(){


		switch (level) {
		case 1:
			maxhp = 100;
			maxmp = 100;
			break;
		case 2:
			maxhp = 110;
			maxmp = 110;
			break;
		case 3:
			maxhp = 120;
			maxmp = 120;
			break;
		case 4:
			maxhp = 130;
			maxmp = 130;
			break;
		case 5:
			maxhp = 140;
			maxmp = 140;
			break;
		case 6:
			maxhp = 150;
			maxmp = 140;
			break;
		case 7:
			maxhp = 155;
			maxmp = 145;
			break;


		}

	}



	public void Damage ( int damage ) {
		if (!isDead) {
			hp -= damage; //体力から差し引く
			anim.SetTrigger ("Damage");
			if (hp <= 0) {
				//体力が0以下になった時
				hp = 0;
				Dead (); //死亡処理
			}
		}
	}

	//死亡処理
	public void Dead () {
		isDead = true;
		anim.SetTrigger ("Dead");
		if (isPlayer)
			GetComponent<PlayerController> ().enabled = false;
		else
			GetComponent<CharacterAI> ().enabled = false;
		
		//GameOver(); //ゲームオーバーにする
	}

	public void GameOver () {
		
	}

}
