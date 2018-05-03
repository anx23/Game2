using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameControll : MonoBehaviour {

	//List<GameObject> characters=new List<GameObject>();
	[SerializeField]
	 GameObject player;
	[SerializeField]
	GameObject enemy;
	GameObject playerUI;
	GameObject enemyUI;
	[SerializeField]
	Image playerImage;
	[SerializeField]
	Image enemyImage;
	[SerializeField]
	string playername;
	[SerializeField]
	string enemyname;
	[SerializeField]
	public bool isSelectPlayer=true;
	#region Result
	public GameObject resultPanel;
	public Text resultText;
	string winName;
	public Text infoText;
	#endregion
	bool isWin;
	bool isBattleScene=false;
	//音
	AudioSource audiosource;
	[SerializeField]
	AudioClip se1;
	[SerializeField]
	AudioClip se2;
	[SerializeField]
	AudioClip se3;


	public bool isFirst=true;
	// Use this for initialization
	void Awake(){
		//SceneManager.sceneLoaded += OnActiveSceneChanged;//イベントにメソッドを登録
	//	DontDestroyOnLoad (this.gameObject);
	}

	void Start () {
		
		audiosource = GetComponent<AudioSource> ();



		DontDestroyOnLoad (this.gameObject);
		isBattleScene = false;
		isSelectPlayer=true;
	}
	
	// Update is called once per frame
	void Update () {
		if (isBattleScene) {
			if (player.GetComponent<Playe_Stautus> ().hp == 0) {
				isWin = false;
				EndGame ();
			}
			if (enemy.GetComponent<Playe_Stautus> ().hp == 0) {
				isWin = true;
				EndGame ();
			}


		}



	}
	//キャラクター選択
	public void SelectCharacter(GameObject go){
		
		if (isSelectPlayer) {
			playerUI = go;

			playerImage.sprite = go.GetComponent<CharacterUI> ().characterImage;
			playername = go.GetComponent<CharacterUI> ().characterName;
		} else {
			enemyUI = go;
			enemyImage.sprite = go.GetComponent<CharacterUI> ().characterImage;
			enemyname= go.GetComponent<CharacterUI> ().characterName;
		}
		audiosource.PlayOneShot (se1);
	}

	public void GoBattle(){
		if (playerUI != null && enemyUI != null) {
			
			SceneManager.LoadScene ("BattleScene");
			//GameObject go1 =	Instantiate (Resources.Load ("Character/" + playername)as GameObject);
			//GameObject g02 =	Instantiate (Resources.Load ("Character/" + enemyname)as GameObject);
		}
	}

	public void ChangeSelecter(){

		isSelectPlayer = !isSelectPlayer;
	}

	//バトル終了
	void EndGame(){

		resultPanel.SetActive (true);
		if (isWin) {
			winName = playername;
			resultText.text = "YOU WIN";
		} else {
			winName = enemyname;
			resultText.text = "YOU LOSE";
		}
		audiosource.PlayOneShot (se3);
		//isBattleScene = false;
		//ResetCharacter ();


	}




	public void ResetCharacter(){
	//	Destroy (player.gameObject);
		//Destroy (enemy.gameObject);
		isBattleScene=false;
		//GameObject go = GameObject.FindWithTag ("Player");
		//GameObject go1 = GameObject.FindWithTag ("Enemy");
		//isFirst=false;
		//Destroy (player);
		//Destroy (enemy);
		Debug.Log (isFirst);
		Destroy(this.gameObject);

		//SceneManager.LoadScene ("Select");







	}

	/*
	private void OnActiveSceneChanged (Scene scene, LoadSceneMode sceneMode){
		


		if (scene.buildIndex == 2) {//戦闘シーンであれば
			/*
			if (isFirst) {
				GameObject go1 = GameObject.FindWithTag ("Player");
				GameObject go2 = GameObject.FindWithTag ("Enemy");
				//GameObject go2=	GameObject.FindWithTag ("MainCamera");
				if (go2 != null && go1 != null) {
					Destroy (go2);

					Destroy (go1);
				}
				isFirst = false;
			}

			Debug.Log (scene.buildIndex);
			//生成
*/

			//player= Instantiate (Resources.Load("Character/"+this.playername)as GameObject,pos1.transform.position,pos1.transform.rotation);
			//enemy=	Instantiate (Resources.Load("Character/"+this.enemyname)as GameObject,pos2.transform.position,pos2.transform.rotation);
			//player= Resources.Load("Character/"+this.playername)as GameObject;
			//enemy=	Resources.Load("Character/"+this.enemyname)as GameObject;
			//GameObject go1 = GameObject.Find ("BattleManager");
			//BattleManager bm = go1.GetComponent<BattleManager> ();
			//bm.SetCharacter (player,enemy);
			//GameObject go1=	Instantiate (Resources.Load("Character"+playername)as GameObject);
			//GameObject g02=	Instantiate (Resources.Load("Character"+enemyname)as GameObject);




			//StartCoroutine (BattleCount());

			//GameObject go = GameObject.Find ("BattleInfoText");
			//infoText = go.GetComponent<Text> ();
			//StartCoroutine (BattleCount());
	//	}
//	}
//*/

	public void SetCharacter(){
		GameObject pos1 = GameObject.Find ("SpawnPosition1");
		GameObject pos2 = GameObject.Find ("SpawnPosition2");
		player= Instantiate (Resources.Load("Character/"+this.playername)as GameObject,pos1.transform.position,pos1.transform.rotation);
		enemy =	Instantiate (Resources.Load ("Character/" + this.enemyname)as GameObject, pos2.transform.position, pos2.transform.rotation);
		//Instantiate (player,pos1.transform.position,pos1.transform.rotation);
		//Instantiate (enemy,pos2.transform.position,pos2.transform.rotation);
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
		isBattleScene = true;


		resultPanel=GameObject.Find("ResultPanel");
		resultText=GameObject.Find("ResultText").GetComponent<Text>();

		//resultPanel.SetActive (false);
		GameObject go = GameObject.Find ("BattleInfoText");
		infoText = go.GetComponent<Text> ();

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
