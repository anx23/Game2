using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CharacterUI : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler {
	public Sprite characterImage;
	public string characterName;
	public Text selectText;
	GameObject manager;
	AudioSource audiosourse;
	[SerializeField]
	AudioClip se1;
	// Use this for initialization
	void Start () {
		selectText = GetComponentInChildren<Text> ();
		manager = GameObject.Find ("GameControll");
		selectText.enabled = false;
		audiosourse = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {




	}


	public  void OnPointerEnter (PointerEventData eventData)
	{
		audiosourse.PlayOneShot (se1);
		
		selectText.enabled = true;
		if (manager.GetComponent<GameControll> ().isSelectPlayer) {
			selectText.color = Color.red;
			selectText.text = "1P";
		} else {
			selectText.color = Color.blue;
			selectText.text = "2P";

		}

	}


	public  void OnPointerExit (PointerEventData eventData)
	{
		selectText.enabled = false;
	}





}
