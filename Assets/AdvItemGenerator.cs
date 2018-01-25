using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvItemGenerator : MonoBehaviour {
	//carPrefabを入れる
	public GameObject carPrefab;
	//coinPrefabを入れる
	public GameObject coinPrefab;
	//cornPrefabを入れる
	public GameObject trafficconePrefab;
	//Unityちゃん
	private GameObject unitychan;
	//ゴール地点（出現）
	private float goalPos =0;
	//Unity位置
	private float uniPos =0;
	//現在Unity位置
	private float nowuniPos =0;


	//アイテムを出すx方向の範囲
	private float posRange =3.4f;


	// Use this for initialization
	void Start () {
		//Unityちゃんのゲームオブジェクトを定義
		this.unitychan = GameObject.Find("unitychan");
		uniPos = unitychan.transform.position.z;
		nowuniPos = unitychan.transform.position.z;
		Debug.Log (uniPos);
		Debug.Log (nowuniPos);

		goalPos = uniPos + 50;
		//一定の距離ごとにアイテムを生成
		for (int i = (int)uniPos+30; i < goalPos+30; i+=15){
			//どのアイテムを出すのかをランダムに設定
			int num =Random.Range(0,10);
			Debug.Log ("アイテムナンバー" + num);
			if (num <= 1){
				//コーンをx軸方向に一直線に生成
				for (float j=-1;j<=1;j+=0.4f){
					GameObject cone= Instantiate(trafficconePrefab) as GameObject;
					cone.transform.position = new Vector3 (4*j,cone.transform.position.y, i);
				}
			}else{

				//レーンごとにアイテムを生成
				for (int j = -1; j<2; j++){
					//アイテムの種類を決める
					int item = Random.Range (1,11);
					//アイテムを置くZ座標のオフセットをランダムに設定
					int offsetZ = Random.Range(-5,6);
					//60%コイン配置：30％車配置：10%何もなし
					if(1 <=item && item <=6){
						//コインを生成
						GameObject coin = Instantiate (coinPrefab) as GameObject;
						coin.transform.position = new Vector3 (posRange * j,coin.transform.position.y, i + offsetZ);
					}else if(7<= item && item <= 9){
						//車を生成
						GameObject car =Instantiate (carPrefab) as GameObject;
						car.transform.position = new Vector3 (posRange*j,car.transform.position.y, i + offsetZ);
					}
				}
			}
		}
	}

	// Update is called once per frame
	void Update ()
	{
		//Unityちゃんの位置を検出
		nowuniPos = unitychan.transform.position.z;

		//UnityちゃんのZ位置が前回の位置より15動いていたらアイテムを出す。
		if (nowuniPos > uniPos + 15) {
			//出現位置を再設定
			goalPos = uniPos + 50;
			//どのアイテムを出すのかをランダムに設定
			int num = Random.Range (0, 10);
			Debug.Log ("アイテムナンバー" + num);
			if (num <= 1) {
				//コーンをx軸方向に一直線に生成
				for (float j = -1; j <= 1; j += 0.4f) {
					GameObject cone = Instantiate (trafficconePrefab) as GameObject;
					cone.transform.position = new Vector3 (4 * j, cone.transform.position.y, goalPos);
				}
			} else {

				//レーンごとにアイテムを生成
				for (int j = -1; j < 2; j++) {
					//アイテムの種類を決める
					int item = Random.Range (1, 11);
					//アイテムを置くZ座標のオフセットをランダムに設定
					int offsetZ = Random.Range (-5, 6);
					//60%コイン配置：30％車配置：10%何もなし
					if (1 <= item && item <= 6) {
						//コインを生成
						GameObject coin = Instantiate (coinPrefab) as GameObject;
						coin.transform.position = new Vector3 (posRange * j, coin.transform.position.y, goalPos + offsetZ);
					} else if (7 <= item && item <= 9) {
						//車を生成
						GameObject car = Instantiate (carPrefab) as GameObject;
						car.transform.position = new Vector3 (posRange * j, car.transform.position.y, goalPos + offsetZ);
					}
				}
			}
			uniPos = nowuniPos;

		}
	}
}
