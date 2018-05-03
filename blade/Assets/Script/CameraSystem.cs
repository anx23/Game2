using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSystem : MonoBehaviour {




	public float speed = 10.0f; 
	public Transform camerapos1;
	public Transform camerapos2;
	public Transform cameraposes;
	public GameObject target;    // ターゲットへの参照
	public Vector3 offset;     // 相対座標
	GameObject myCamera;
	float maxClosed;


	Quaternion camerarot;
	//GameObject playerdirection;

	public float rotSpeed = 10.0f; 
	float maxAngle = 20; // 最大回転角度
	float minAngle = -30; // 最小回転角度
	float rotspeed = 0.5f; // 回転スピード(お好みで調整してください)
	[SerializeField]
	bool hitWall=false;

	void Start ()
	{
		//自分自身とtargetとの相対距離を求める
		//
		target=GameObject.FindWithTag("Player");
		offset = transform.position - target.transform.position;
		myCamera = GameObject.FindWithTag ("MainCamera");
		maxClosed = Vector3.Distance (target.transform.position, camerapos2.transform.position);

		camerarot = transform.rotation;
	//	playerdirection = GameObject.Find ("PlayerDirection");
		SetTarget();
	}




	void Update ()
	{
		
		//SetTarget ();
		float axisH = Input.GetAxis( "Mouse X" ); 
		transform.Rotate (0,axisH*2.0f,0,Space.World);
		//target.transform.Rotate (0,axisH*2.0f,0,Space.World);
	//	cameraposes.transform.Rotate(0,axisH*2.0f,0,Space.World);
		float axisV = Input.GetAxis("Mouse Y")*rotSpeed;
		/////
		//transform.LookAt(target.transform.position);

		//	transform.rotation = Quaternion.Euler(transform.eulerAngles.x, Mathf.Repeat(transform.eulerAngles.y + axisH, 360f), transform.eulerAngles.z);

		//　カメラの角度が限界角度を超えてなければカメラの角度を更新する

		float rot = Mathf.Repeat(transform.eulerAngles.x + axisV, 360f);

		if(20 > Quaternion.Angle(camerarot, Quaternion.Euler(rot, 0, 0))) {
			transform.rotation = Quaternion.Euler(rot, transform.eulerAngles.y, transform.eulerAngles.z);
		}







	}







	void FixedUpdate ()
	{

		// 自分自身の座標に、targetの座標に相対座標を足した値を設定する
		//	transform.position =Vector3.Lerp(transform.position, target.position + offset,Time.deltaTime);

		AvoidWall ();
		//WallCloseStop ();
		transform.position = target.transform.position+offset;
		if(!hitWall){
		if (target.GetComponent<PlayerController> ().isDash) {
			if (Vector3.Distance (target.transform.position, camerapos2.position) >= maxClosed) {
				Camera.main.transform.position = Vector3.Lerp (Camera.main.transform.position, camerapos2.position, Time.deltaTime*5);

			}
			else{
				//Camera.main.transform.position = Vector3.Lerp (Camera.main.transform.position, camerapos1.position, Time.deltaTime*5);
				//transform.position = camerapos2.position;
				}

		} else {
			//transform.position = target.transform.position+offset;
			Camera.main.transform.position = Vector3.Lerp (Camera.main.transform.position, camerapos1.position, Time.deltaTime*5);
			//Camera.main.transform.position = Vector3.Lerp (Camera.main.transform.position, camerapos1.position, Time.deltaTime*5);
			//Camera.main.transform.position = camerapos1.position;
		}
		//	Vector3 axis = transform.TransformDirection (Vector3.up); 
		//	transform.RotateAround (target.position, axis, speed * Input.GetAxis("Horizontal2")); 
		}

	}

	public void SetTarget(){

		target = GameObject.FindWithTag ("Player");
	}

	RaycastHit hit;
	int layerMask = 1 << 8;//wallだけ
	void AvoidWall(){
		if (Physics.Linecast (target.transform.position + Vector3.up, myCamera.transform.position, out hit, layerMask)) {
			Debug.Log ("Wall");
			myCamera.transform.position = Vector3.Lerp (myCamera.transform.position, hit.point, speed * Time.deltaTime);
			hitWall=true;
			//　障害物と接触してなければ元のカメラ位置に移動
		} else {
			//　元の位置ではない時だけ元の位置に移動
			//if (transform.localPosition != cameraPrePosition) {
			//	transform.localPosition = Vector3.Lerp (transform.localPosition, cameraPrePosition, cameraMoveSpeed * Time.deltaTime);
			//}
			hitWall=false;
		}
		//　レイ

		Debug.DrawLine (target.transform.position + Vector3.up, myCamera.transform.position, Color.red, 0f,false);

	}

	void DashCamera(){


		//target.GetComponent<>

	}





}
