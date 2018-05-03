using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour {
	Text infoText;
	AudioSource audiosource;
	[SerializeField]
	AudioClip se2;
	GameObject resultpanel;
	GameControll manager;


	public GameObject player;
	public GameObject enemy;
	public GameObject resultPanel;
	public Text resultText;
	string winName;
	bool isWin;
	[SerializeField]
	AudioClip se3;
	// Use this for initialization
	void Awake(){
		manager = GameObject.Find ("GameControll").GetComponent<GameControll>();
		manager.SetCharacter ();
		//PlaceCharacter ();
	}

	void Start () {
		audiosource = GetComponent<AudioSource> ();
		GameObject go = GameObject.Find ("BattleInfoText");
		infoText = go.GetComponent<Text> ();
		StartCoroutine (BattleCount());
		resultpanel=GameObject.Find ("ResultPanel");
		resultpanel.SetActive (false);



		//PlaceCharacter ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}



	public void SetCharacter(GameObject player,GameObject enemy){
		
		this.player = player;
		this.enemy = enemy;
	}

	void PlaceCharacter(){
		GameObject pos1 = GameObject.Find ("SpawnPosition1");
		GameObject pos2 = GameObject.Find ("SpawnPosition2");
		Instantiate (player,pos1.transform.position,pos1.transform.rotation);
		Instantiate (enemy,pos2.transform.position,pos2.transform.rotation);
		player.tag="Player";
		enemy.tag="Enemy";

		player.AddComponent<PlayerController>();
		//player.GetComponent<PlayerController>().SetTarget();
		enemy.AddComponent<CharacterAI> ();

	
		//enemy.GetComponent<CharacterAI> ().SetTarget ();
		player.GetComponent<Playe_Stautus> ().isPlayer = true;
		enemy.GetComponent<Playe_Stautus> ().isPlayer = false;
		player.GetComponentInChildren<sowd> ().isplayer=true;
		enemy.GetComponentInChildren<sowd> ().isplayer = false;
	}

	//バトル終了
	void EndGame(){

		resultPanel.SetActive (true);
		if (isWin) {
			//winName = playername;
			resultText.text = "YOU WIN";
		} else {
			//winName = enemyname;
			resultText.text = "YOU LOSE";
		}
		audiosource.PlayOneShot (se3);
		//isBattleScene = false;
		//ResetCharacter ();


	}





	IEnumerator BattleCount(){

		infoText.enabled = true;
		infoText.text="READY";

		yield return new WaitForSeconds (1.0f);
		infoText.text = "FIGHT";
		yield return new WaitForSeconds (1.0f);
		infoText.enabled = false;
		audiosource.PlayOneShot (se2);

		StopCoroutine (BattleCount());
	}


}
