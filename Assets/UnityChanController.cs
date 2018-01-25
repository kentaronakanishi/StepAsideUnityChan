using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UnityChanController : MonoBehaviour {
	//アニメーションするためのコンポーネントを入れる
	private Animator myAnimator;

	//Unityちゃんを移動させるコンポーネントを入れる（追加）
	private Rigidbody myRigidbody;
	//前進するための力（追加）
	private float forwardForce = 800.0f;
	//左右に移動するための力（追加）
	private float turnForce =500.0f;
	//左右の移動できる範囲（追加）
	private float movableRange =3.4F;
	//ジャンプするための力
	private float upForce =500.0f;
	//動きを減速させる係数
	private float coefficient =0.95f;

	//ゲーム終了のカウント
	private int life;
	//ゲーム終了の判定
	private bool isEnd =false;

	//ゲーム終了時に表示するテキスト
	private GameObject stateText;
	//スコアを表示するテキスト
	private GameObject scoreText;
	//スピードを表示するテキスト
	private GameObject speedText;
	//ライフを表示するテキスト
	private GameObject lifeText;

	//得点
	private int score =0;
	//速度
	private int speed =0;
	//前フレームの座標
	Vector3 latestPos;
	//左ボタン押下の判定
	private bool isLButtonDown = false;
	//右ボタン押下の判定
	private bool isRButtonDown = false;






	//前後に加減速するための力（独自）
	private float axelForce =5.0f;

	// Use this for initialization
	void Start () {

		//Animatorコンポーネントを取得
		this.myAnimator = GetComponent<Animator>();

		//走るアニメーションを開始
		this.myAnimator.SetFloat ("Speed", 1f);

		//Rigidbodyコンポーネントを取得
		this.myRigidbody = GetComponent<Rigidbody>();

		//シーン中のstateTextオブジェクトを取得
		this.stateText = GameObject.Find("GameResultText");

		//シーンの中のscoreTextオブジェクトを取得
		this.scoreText=GameObject.Find("ScoreText");

		//シーン中のspeedTextオブジェクトを取得
		this.speedText=GameObject.Find("SpeedText");

		//シーン中のlifeTextオブジェクトを取得
		this.lifeText=GameObject.Find("LifeText");
		life = 10;



	}
	void Update () {

		
		//ゲーム終了ならUnityちゃんの動きを減衰する
		if (this.isEnd) {
			this.forwardForce *= this.coefficient;
			this.turnForce *= this.coefficient;
			this.upForce *= this.coefficient;
			this.myAnimator.speed *= this.coefficient;
		}

		//Speedを算出
		speed =(Mathf.FloorToInt(myRigidbody.velocity.magnitude))*10;
		//speed = Mathf.FloorToInt(((myRigidbody.transform.position - latestPos) / Time.deltaTime).magnitude);
		//latestPos = myRigidbody.transform.position; 
		//Speedを表示
		this.speedText.GetComponent<Text>().text = speed+"km/h";

			//Unityちゃんに前方向の力を与える
			this.myRigidbody.AddForce (this.transform.forward * this.forwardForce);
			//Unityちゃんを矢印キーまたはボタンに応じて左右に移動させる
		if ((Input.GetKey (KeyCode.LeftArrow) || this.isLButtonDown) && -this.movableRange < this.transform.position.x) {
				//左に移動
				this.myRigidbody.AddForce (-this.turnForce, 0, 0);
		} else if ((Input.GetKey (KeyCode.RightArrow) || this.isRButtonDown) && this.transform.position.x < this.movableRange) {
				//右に移動
				this.myRigidbody.AddForce (this.turnForce, 0, 0);
			}
			//Jumpステートの場合はJumpにfalseをセットする
			if (this.myAnimator.GetCurrentAnimatorStateInfo (0).IsName ("Jump")) {
				this.myAnimator.SetBool ("Jump", false);
			}

			//ジャンプしていない時にスペースが押されたらジャンプする（追加）
			if (Input.GetKeyDown (KeyCode.Space) && this.transform.position.y < 0.5f) {
				//ジャンプアニメを再生
				this.myAnimator.SetBool ("Jump", true);
				//Unityちゃんに上方向の力を与える
				this.myRigidbody.AddForce (this.transform.up * this.upForce);
				Debug.Log ("ジャンプ中　強さ" + upForce);
			}
			

		//Unityちゃんを矢印キーで加減速させる（独自）
			if (Input.GetKey (KeyCode.UpArrow) && this.forwardForce <= 10000f) {
				//加速
				this.myRigidbody.AddForce (0, 0, this.axelForce);
				forwardForce = forwardForce + axelForce;
				Debug.Log ("加速中　現在" + forwardForce);
			} else if (Input.GetKey (KeyCode.DownArrow)) {
				//減速
				this.myRigidbody.AddForce (0, 0, -this.axelForce / 2);
				forwardForce = forwardForce - axelForce;
				Debug.Log ("減速中　現在" + forwardForce);
			}
		}

	//トリガーモードで他のオブジェクトと接触した場合の処理
	void OnTriggerEnter(Collider other){

		//障害物に衝突した場合
		if (other.gameObject.tag == "CarTag" || other.gameObject.tag == "TrafficConeTag") {
			this.life = life - 1;

			//lifeTextに残ライフを表示
			this.lifeText.GetComponent<Text> ().text = "Unity-chan  x " + life;

			//ライフがゼロになったらゲームオーバー
			if (life == 0) {
				this.isEnd = true;
				//stateTextにGAME OVERを表示
				this.stateText.GetComponent<Text> ().text = "GAME OVER";	
			}
		}

		//ゴール地点に到達した場合
		if (other.gameObject.tag == "GoalTag") {
			this.isEnd = true;
			//stateTextにGAME CLEARを表示
			this.stateText.GetComponent<Text>().text = "CLEAR!!";
		}

		//コインに衝突した場合
		if (other.gameObject.tag == "CoinTag") {

			//スコアを加算
			this.score+=10;

			//ScoreText獲得した点数を表示
			this.scoreText.GetComponent<Text>().text = "Score "+this.score+"pt";

			//接触したコインのオブジェクトを破棄
			Destroy (other.gameObject);

			//パーティクルを再生
			GetComponent<ParticleSystem>().Play();

			//接触したコインのオブジェクトを破壊
			Destroy(other.gameObject);

			//加速（独自）
			forwardForce = forwardForce +100;


		}
	}
	//ジャンプボタンを押したときの処理
			public void GetMyJumpButtonDown(){
		if (this.transform.position.y < 0.5f) {
			this.myAnimator.SetBool ("Jump", true);
			this.myRigidbody.AddForce (this.transform.up * this.upForce);
		}
	}
			//左ボタンを押し続けた時の処理
			public void GetMyLeftButtonDown(){
		this.isLButtonDown = true;
	}
			//左ボタンを離した時の処理
			public void GetMyLeftButtonUp(){
		this.isLButtonDown = false;
	}
			//右ボタンを押し続けた時の処理
			public void GetMyRightButtonDown(){
				this.isRButtonDown = true;
			}
			//右ボタンを離した時の処理
			public void GetMyRightButtonUp(){
				this.isRButtonDown = false;
			}
}