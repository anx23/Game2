using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMove : MonoBehaviour {
	GameObject manager;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public 	void StartGame(){
		
		SceneManager.LoadScene ("Select");

	}

	public void OnReturnMenu(){
		manager = GameObject.Find ("GameControll");

		manager.GetComponent<GameControll> ().ResetCharacter ();
		//Destroy (manager.gameObject);
		//GameObject go = GameObject.FindWithTag ("Player");
		//GameObject go1 = GameObject.FindWithTag ("Enemy");
		//GameObject go2=	GameObject.FindWithTag ("MainCamera");
		//Destroy (go2);
		//Destroy (go);
		//Destroy (go1);

	//SceneManager.LoadScene ("BattleScene");
		SceneManager.LoadScene ("Select");

	}






}
