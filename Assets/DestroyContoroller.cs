using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyContoroller : MonoBehaviour {

	//Unityちゃんのオブジェクト
	private GameObject unitychan;
	//Unityちゃんとデストロイヤーの距離
	private float difference;
	//デストロイヤーの位置取得
	private float xpoint;
	private float ypoint;


	// Use this for initialization
	void Start () {
		xpoint = this.transform.position.x;
		ypoint = this.transform.position.y;
		//Unityちゃんのオブジェクトを取得
		this.unitychan = GameObject.Find("unitychan");
		//Unityちゃんとデストロイヤーの位置（ｚ座標）の差を求める
		this.difference=unitychan.transform.position.z - this.transform.position.z;

	}
	
	// Update is called once per frame
	void Update () {
		//Unityちゃんの位置に合わせてデストロイヤーの位置を移動
		this.transform.position=new Vector3(xpoint,ypoint, this.unitychan.transform.position.z-difference);

	}
	//トリガーモードで他のオブジェクトと接触した場合の処理
	void OnTriggerEnter(Collider other){

		//障害物に衝突した場合
		if (other.gameObject.tag == "CarTag" || other.gameObject.tag == "TrafficConeTag" || other.gameObject.tag == "CoinTag") {
			
			//接触したオブジェクトを破壊
			Destroy(other.gameObject);
		}
	}
}
