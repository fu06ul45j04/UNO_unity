using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Game : MonoBehaviour {
	public enum GameState
	{
		initial,
		playing,
		end
	};
	public enum TurnOrder
	{
		clockwise,
		counterclockwise
	};
	public enum Player
	{
		player1,
		player2,
		player3,
		player4
	};
	struct cardItemAndCards{
		public GameObject cardItem;
		public Card cardItemCards;
	}
	List<GameObject> cardSortByID = new List<GameObject>();
	List<cardItemAndCards> player1Item = new List<cardItemAndCards> ();
	List<cardItemAndCards> player2Item = new List<cardItemAndCards> ();
	List<cardItemAndCards> player3Item = new List<cardItemAndCards> ();
	List<cardItemAndCards> player4Item = new List<cardItemAndCards> ();
	public GameState gameState = GameState.initial;
	public TurnOrder turnOrder = TurnOrder.clockwise;
	List<Card> deck = new List<Card> ();
	List<Card> discard = new List<Card> ();
	//
	public GameObject player1GO;
	public GameObject player2GO;
	public GameObject player3GO;
	public GameObject player4GO;
	public Player nowPlayer = Player.player1;
	public Player firstPlayer = Player.player1;
	public Card nowCard ;
	public GameObject nowCarditem; 
	public int nextDraw = 0;
	public Sprite[] UNOCard = new Sprite[54];
	public Text DrawCount;
	public Text WinnerText;
	public GameObject DataUI;
	public InputField autoRunInput;
	public Toggle fastToggle;
	public int[] PlayerWinCount = new int[4];
	//List<Card> player1Hand = new List<Card> ();
	bool waitingForPlayer = false;
	bool drawflag = false;
	bool fast_testflag = false;
	bool coroutineflag = false;
	int autoRunCount = new int();
	public GameObject cardUI;
	public GameObject canvas; 
	public GameObject Arrow;
	public Image nowColorImage;
	bool randomOrderFlag = false;
	public Toggle randomOrderToggle;
	public Player[] playerOrder;
	public Text randomOrderText;

	bool test_image_flag = true;

	// Use this for initialization
	//----
	private AI_HeuristicV2_3 player1 ;
	private AI_AllRandom player2;
	private AI_AllRandom player3;
	private AI_AllRandom player4;
	// AI_HeuristicV1 AI_HeuristicV2 AI_HeuristicV3 AI_AllRandom PlayerCountrol
	// AI_HeuristicV2_1 AI_HeuristicV2_2 AI_HeuristicV2_3
	void Start () {
		SetCardItem ();
		player1 = player1GO.GetComponent("AI_HeuristicV2_3") as AI_HeuristicV2_3 ;
		player2 = player2GO.GetComponent("AI_AllRandom") as AI_AllRandom;
		player3 = player3GO.GetComponent("AI_AllRandom") as AI_AllRandom;
		player4 = player4GO.GetComponent("AI_AllRandom") as AI_AllRandom;
		player1.enabled = true;
		player2.enabled = true;
		player3.enabled = true;
		player4.enabled = true;
		playerOrder = new Player[4]{Player.player1,Player.player2,Player.player3,Player.player4};
	}
	
	// Update is called once per frame
	void Update () {
		if (gameState == GameState.initial) {
			SetCardToDeck ();
			shuffle ();
			deal ();
			test_setData();
			gameStart ();
		} 
		else if (gameState == GameState.playing) {
			waitToPlayerCard ();
		} 
		else if (gameState == GameState.end) {
			autoRun();
		}
	}
	void SetCardItem(){
		
		for(int i=0;i<25;i++){
			Card newCard = new Card();
			newCard.color = Card.CardColor.Red;

			newCard.function = Card.CardFunction.zero+((i+1)/2);/*
			if( i==0 ){
				newCard.function = Card.CardFunction.zero;
			}
			else if( i>=1 && i<=2 ){
				newCard.function = Card.CardFunction.one;
			}
			else if( i>=3 && i<=4 ){
				newCard.function = Card.CardFunction.two;
			}
			else if( i>=5 && i<=6 ){
				newCard.function = Card.CardFunction.three;
			}
			else if( i>=7 && i<=8 ){
				newCard.function = Card.CardFunction.four;
			}
			else if( i>=9 && i<=10 ){
				newCard.function = Card.CardFunction.five;
			}
			else if( i>=11 && i<=12 ){
				newCard.function = Card.CardFunction.six;
			}
			else if( i>=13 && i<=14 ){
				newCard.function = Card.CardFunction.seven;
			}
			else if( i>=15 && i<=16 ){
				newCard.function = Card.CardFunction.eight;
			}
			else if( i>=17 && i<=18 ){
				newCard.function = Card.CardFunction.nine;
			}
			else if( i>=19 && i<=20){
				newCard.function = Card.CardFunction.Skip;
			}
			else if( i>=21 && i<=22){
				newCard.function = Card.CardFunction.Reverse;
			}
			else if( i>=23 && i<=24){
				newCard.function = Card.CardFunction.DrawTwo;
			}*/
			newCard.CardID = i;
			GameObject newCardItem = Instantiate(cardUI) as GameObject;
			newCardItem.transform.SetParent(canvas.transform,false);
			Image surface = newCardItem.GetComponent<Image>();
			getImage(surface,newCard.color,newCard.function);
			SetItemPosition(newCardItem);
			cardSortByID.Add(newCardItem);
		}
		for(int i=25;i<50;i++){
			Card newCard = new Card();
			newCard.color = Card.CardColor.Blue;

			newCard.function = Card.CardFunction.zero+((i-25+1)/2);
			newCard.CardID = i;
			GameObject newCardItem = Instantiate(cardUI) as GameObject;
			newCardItem.transform.SetParent(canvas.transform,false);
			Image surface = newCardItem.GetComponent<Image>();
			getImage(surface,newCard.color,newCard.function);
			SetItemPosition(newCardItem);
			cardSortByID.Add(newCardItem);
		}
		for(int i=50;i<75;i++){
			Card newCard = new Card();
			newCard.color = Card.CardColor.Green;
			newCard.function = Card.CardFunction.zero+((i-50+1)/2);

			newCard.CardID = i;
			GameObject newCardItem = Instantiate(cardUI) as GameObject;
			newCardItem.transform.SetParent(canvas.transform,false);
			Image surface = newCardItem.GetComponent<Image>();
			getImage(surface,newCard.color,newCard.function);
			SetItemPosition(newCardItem);
			cardSortByID.Add(newCardItem);
		}
		for(int i=75;i<100;i++){
			Card newCard = new Card();
			newCard.color = Card.CardColor.Yellow;
			newCard.function = Card.CardFunction.zero+((i-75+1)/2);

			newCard.CardID = i;
			GameObject newCardItem = Instantiate(cardUI) as GameObject;
			newCardItem.transform.SetParent(canvas.transform,false);
			Image surface = newCardItem.GetComponent<Image>();
			getImage(surface,newCard.color,newCard.function);
			SetItemPosition(newCardItem);
			cardSortByID.Add(newCardItem);
		}
		for (int i=100; i<104; i++) {
			Card newCard = new Card();
			newCard.function = Card.CardFunction.Wild;
			newCard.color = Card.CardColor.Black;
			newCard.CardID = i;
			GameObject newCardItem = Instantiate(cardUI) as GameObject;
			newCardItem.transform.SetParent(canvas.transform,false);
			Image surface = newCardItem.GetComponent<Image>();
			getImage(surface,newCard.color,newCard.function);
			SetItemPosition(newCardItem);
			cardSortByID.Add(newCardItem);
		}
		for (int i=104; i<108; i++) {
			Card newCard = new Card();
			newCard.function = Card.CardFunction.WildDrawFour;
			newCard.color = Card.CardColor.Black;
			newCard.CardID = i;
			GameObject newCardItem = Instantiate(cardUI) as GameObject;
			newCardItem.transform.SetParent(canvas.transform,false);
			Image surface = newCardItem.GetComponent<Image>();
			getImage(surface,newCard.color,newCard.function);
			SetItemPosition(newCardItem);
			cardSortByID.Add(newCardItem);
		}

	}
	void SetItemPosition(GameObject tar){
		Image tari = tar.GetComponent<Image> ();
		//Debug.Log (tari.sprite.name);
		tari.rectTransform.anchoredPosition = new Vector2(-315,131.5f);
		//Debug.Log (tari.rectTransform.anchoredPosition);
	}
	void SetCardToDeck(){

		for(int i=0;i<25;i++){
			Card newCard = new Card();
			newCard.color = Card.CardColor.Red;
			newCard.function = Card.CardFunction.zero+((i+1)/2);

			newCard.CardID = i;
			deck.Add(newCard);
		}
		for(int i=25;i<50;i++){
			Card newCard = new Card();
			newCard.color = Card.CardColor.Blue;
			newCard.function = Card.CardFunction.zero+((i-25+1)/2);
			newCard.CardID = i;
			deck.Add(newCard);
		}
		for(int i=50;i<75;i++){
			Card newCard = new Card();
			newCard.color = Card.CardColor.Green;
			newCard.function = Card.CardFunction.zero+((i-50+1)/2);
			newCard.CardID = i;
			deck.Add(newCard);
		}
		for(int i=75;i<100;i++){
			Card newCard = new Card();
			newCard.color = Card.CardColor.Yellow;
			newCard.function = Card.CardFunction.zero+((i-75+1)/2);
			newCard.CardID = i;
			deck.Add(newCard);
		}
		for (int i=100; i<104; i++) {
			Card newCard = new Card();
			newCard.function = Card.CardFunction.Wild;
			newCard.color = Card.CardColor.Black;
			newCard.CardID = i;
			deck.Add(newCard);
		}
		for (int i=104; i<108; i++) {
			Card newCard = new Card();
			newCard.function = Card.CardFunction.WildDrawFour;
			newCard.color = Card.CardColor.Black;
			newCard.CardID = i;
			deck.Add(newCard);
		}
	}

	void shuffle(){
		Card temp;
		for (int i = 0; i<deck.Count; i++) {
			int a = Random.Range(i,deck.Count);
			temp = deck[a];
			deck[a] = deck[i];
			deck[i] = temp;
		}
	}

	void deal(){
		for (int i=0; i<7; i++) {
			for(int j=0;j<4;j++){

				// deal to player hand
				
				// record in game

				if(nowPlayer == Player.player1)
					drawCards(Player.player1,1);
				else if(nowPlayer == Player.player2)
					drawCards(Player.player2,1);
				else if(nowPlayer == Player.player3)
					drawCards(Player.player3,1);
				else if(nowPlayer == Player.player4)
					drawCards(Player.player4,1);

				turnOneRound();
			}
		}
	}

	void turnOneRound(){
		if (turnOrder == TurnOrder.clockwise) {
			if (nowPlayer == playerOrder [0]) {
				nowPlayer = playerOrder [1];
			}
			else if (nowPlayer == playerOrder [1]) {
				nowPlayer = playerOrder [2];
			}
			else if (nowPlayer == playerOrder [2]) {
				nowPlayer = playerOrder [3];
			}
			else if (nowPlayer == playerOrder [3]) {
				nowPlayer = playerOrder [0];
			}
		} 
		else if (turnOrder == TurnOrder.counterclockwise) {
			if (nowPlayer == playerOrder [0]) {
				nowPlayer = playerOrder [3];
			}
			else if (nowPlayer == playerOrder [3]) {
				nowPlayer = playerOrder [2];
			}
			else if (nowPlayer == playerOrder [2]) {
				nowPlayer = playerOrder [1];
			}
			else if (nowPlayer == playerOrder [1]) {
				nowPlayer = playerOrder [0];
			}
		} 
		if (gameState == GameState.playing) {
			setArrow();
		}
		/*
		if (turnOrder == TurnOrder.clockwise) {
			if(nowPlayer == Player.player4)
				nowPlayer = nowPlayer-3;
			else
				nowPlayer = nowPlayer+1;
		} 
		else if(turnOrder == TurnOrder.counterclockwise){
			if(nowPlayer == Player.player1)
				nowPlayer = nowPlayer+3;
			else
				nowPlayer = nowPlayer-1;
		}
		if (gameState == GameState.playing) {
			setArrow();
		}*/
	}
	void setArrow(){
		if (gameState != GameState.playing)
			Arrow.transform.position = new Vector2 (-10.07f, 4.16f);
		else if (nowPlayer == Player.player1) {
			Arrow.transform.position = new Vector2 (-5.12f, -2.36f);
			Arrow.transform.rotation = Quaternion.Euler(0,0,-90);
		}
		else if (nowPlayer == Player.player2) {
			Arrow.transform.position = new Vector2 (1.82f, -2.59f);
			Arrow.transform.rotation = Quaternion.Euler(0,0,0);
		}
		else if (nowPlayer == Player.player3) {
			Arrow.transform.position = new Vector2 (1.65f, 2.02f);
			Arrow.transform.rotation = Quaternion.Euler(0,0,90);
		}
		else if (nowPlayer == Player.player4) {
			Arrow.transform.position = new Vector2 (-5.55f, 2.7f);
			Arrow.transform.rotation = Quaternion.Euler(0,0,180);
		}
	}
	void drawCards(Player player, int count){
		for (int i=0; i<count; i++) {
			if(deck.Count==0){
				addCardToDeck();
			}
			Card topCard = deck[0];
			deck.RemoveAt(0);
			if(player == Player.player1){
				//player1Hand.Add(topCard);
				player1.getCard(topCard);

				cardItemAndCards newCardItem = new cardItemAndCards();
				newCardItem.cardItem = cardSortByID[topCard.CardID];
				newCardItem.cardItemCards = topCard;
				player1Item.Add(newCardItem);
				changePosition(player1Item,Player.player1);
			}
			else if(player == Player.player2){
				player2.getCard(topCard);
				cardItemAndCards newCardItem = new cardItemAndCards();
				newCardItem.cardItem = cardSortByID[topCard.CardID];
				newCardItem.cardItem.transform.Rotate(0,0,90);
				newCardItem.cardItemCards = topCard;
				player2Item.Add(newCardItem);
				changePosition(player2Item,Player.player2);
			}
			else if(player == Player.player3){
				player3.getCard(topCard);
				
				cardItemAndCards newCardItem = new cardItemAndCards();
				newCardItem.cardItem = cardSortByID[topCard.CardID];
				newCardItem.cardItemCards = topCard;
				player3Item.Add(newCardItem);
				changePosition(player3Item,Player.player3);
			}
			else if(player == Player.player4){
				player4.getCard(topCard);
				cardItemAndCards newCardItem = new cardItemAndCards();
				newCardItem.cardItem = cardSortByID[topCard.CardID];
				newCardItem.cardItem.transform.Rotate(0,0,270);
				newCardItem.cardItemCards = topCard;
				player4Item.Add(newCardItem);
				changePosition(player4Item,Player.player4);
			}
		}
	}

	void getImage(Image tar, Card.CardColor color, Card.CardFunction func){
		if (color == Card.CardColor.Red) {
			if (func == Card.CardFunction.zero) {
				tar.sprite = UNOCard [0];
			} else if (func == Card.CardFunction.one) {
				tar.sprite = UNOCard [1];
			} else if (func == Card.CardFunction.two) {
				tar.sprite = UNOCard [2];
			} else if (func == Card.CardFunction.three) {
				tar.sprite = UNOCard [3];
			} else if (func == Card.CardFunction.four) {
				tar.sprite = UNOCard [4];
			} else if (func == Card.CardFunction.five) {
				tar.sprite = UNOCard [5];
			} else if (func == Card.CardFunction.six) {
				tar.sprite = UNOCard [6];
			} else if (func == Card.CardFunction.seven) {
				tar.sprite = UNOCard [7];
			} else if (func == Card.CardFunction.eight) {
				tar.sprite = UNOCard [8];
			} else if (func == Card.CardFunction.nine) {
				tar.sprite = UNOCard [9];
			} else if (func == Card.CardFunction.Skip) {
				tar.sprite = UNOCard [10];
			} else if (func == Card.CardFunction.Reverse) {
				tar.sprite = UNOCard [11];
			} else if (func == Card.CardFunction.DrawTwo) {
				tar.sprite = UNOCard [12];
			}
		} else if (color == Card.CardColor.Yellow) {
			if (func == Card.CardFunction.zero) {
				tar.sprite = UNOCard [13];
			} else if (func == Card.CardFunction.one) {
				tar.sprite = UNOCard [14];
			} else if (func == Card.CardFunction.two) {
				tar.sprite = UNOCard [15];
			} else if (func == Card.CardFunction.three) {
				tar.sprite = UNOCard [16];
			} else if (func == Card.CardFunction.four) {
				tar.sprite = UNOCard [17];
			} else if (func == Card.CardFunction.five) {
				tar.sprite = UNOCard [18];
			} else if (func == Card.CardFunction.six) {
				tar.sprite = UNOCard [19];
			} else if (func == Card.CardFunction.seven) {
				tar.sprite = UNOCard [20];
			} else if (func == Card.CardFunction.eight) {
				tar.sprite = UNOCard [21];
			} else if (func == Card.CardFunction.nine) {
				tar.sprite = UNOCard [22];
			} else if (func == Card.CardFunction.Skip) {
				tar.sprite = UNOCard [23];
			} else if (func == Card.CardFunction.Reverse) {
				tar.sprite = UNOCard [24];
			} else if (func == Card.CardFunction.DrawTwo) {
				tar.sprite = UNOCard [25];
			}
		} else if (color == Card.CardColor.Green) {
			if (func == Card.CardFunction.zero) {
				tar.sprite = UNOCard [26];
			} else if (func == Card.CardFunction.one) {
				tar.sprite = UNOCard [27];
			} else if (func == Card.CardFunction.two) {
				tar.sprite = UNOCard [28];
			} else if (func == Card.CardFunction.three) {
				tar.sprite = UNOCard [29];
			} else if (func == Card.CardFunction.four) {
				tar.sprite = UNOCard [30];
			} else if (func == Card.CardFunction.five) {
				tar.sprite = UNOCard [31];
			} else if (func == Card.CardFunction.six) {
				tar.sprite = UNOCard [32];
			} else if (func == Card.CardFunction.seven) {
				tar.sprite = UNOCard [33];
			} else if (func == Card.CardFunction.eight) {
				tar.sprite = UNOCard [34];
			} else if (func == Card.CardFunction.nine) {
				tar.sprite = UNOCard [35];
			} else if (func == Card.CardFunction.Skip) {
				tar.sprite = UNOCard [36];
			} else if (func == Card.CardFunction.Reverse) {
				tar.sprite = UNOCard [37];
			} else if (func == Card.CardFunction.DrawTwo) {
				tar.sprite = UNOCard [38];
			}
		} else if (color == Card.CardColor.Blue) {
			if (func == Card.CardFunction.zero) {
				tar.sprite = UNOCard [39];
			} else if (func == Card.CardFunction.one) {
				tar.sprite = UNOCard [40];
			} else if (func == Card.CardFunction.two) {
				tar.sprite = UNOCard [41];
			} else if (func == Card.CardFunction.three) {
				tar.sprite = UNOCard [42];
			} else if (func == Card.CardFunction.four) {
				tar.sprite = UNOCard [43];
			} else if (func == Card.CardFunction.five) {
				tar.sprite = UNOCard [44];
			} else if (func == Card.CardFunction.six) {
				tar.sprite = UNOCard [45];
			} else if (func == Card.CardFunction.seven) {
				tar.sprite = UNOCard [46];
			} else if (func == Card.CardFunction.eight) {
				tar.sprite = UNOCard [47];
			} else if (func == Card.CardFunction.nine) {
				tar.sprite = UNOCard [48];
			} else if (func == Card.CardFunction.Skip) {
				tar.sprite = UNOCard [49];
			} else if (func == Card.CardFunction.Reverse) {
				tar.sprite = UNOCard [50];
			} else if (func == Card.CardFunction.DrawTwo) {
				tar.sprite = UNOCard [51];
			}
		} else if (color == Card.CardColor.Black) {
			if(func == Card.CardFunction.Wild)
				tar.sprite = UNOCard[52];
			else if(func == Card.CardFunction.WildDrawFour)
				tar.sprite = UNOCard[53];
		}

	}

	void changePosition(List<cardItemAndCards> item,Player itemPlayer){
		item.Sort((s1,s2)=>s1.cardItemCards.CardID.CompareTo(s2.cardItemCards.CardID));
		if(itemPlayer == Player.player1){
			for(int i=0;i<item.Count;i++){
				Image surface = item[i].cardItem.GetComponent<Image>();
				surface.rectTransform.anchoredPosition = new Vector2(-161+23*i,-131.5f);
			}
		}
		else if(itemPlayer == Player.player2){
			for(int i=0;i<item.Count;i++){
				Image surface = item[i].cardItem.GetComponent<Image>();
				surface.rectTransform.anchoredPosition = new Vector2(118,-78+23*i);
			}
		}
		else if(itemPlayer == Player.player3){
			for(int i=0;i<item.Count;i++){
				Image surface = item[i].cardItem.GetComponent<Image>();
				surface.rectTransform.anchoredPosition = new Vector2(65-23*i,131.5f);
			}
		}
		else if(itemPlayer == Player.player4){
			for(int i=0;i<item.Count;i++){
				Image surface = item[i].cardItem.GetComponent<Image>();
				surface.rectTransform.anchoredPosition = new Vector2(-258,78-23*i);
			}
		}
	}

	void gameStart(){

		while(deck [0].color == Card.CardColor.Black) {
			shuffle();
		}
		nowCard = deck [0];
		deck.RemoveAt (0);
		discard.Add (nowCard);
		nowCarditem = cardSortByID [nowCard.CardID];
		nowCarditem.GetComponent<Image> ().rectTransform.anchoredPosition = new Vector2 (-68, -2.5f);
		waitingForPlayer = true;
		gameState = GameState.playing;
		setArrow ();
	} 

	void waitToPlayerCard(){
		if (waitingForPlayer == true & fast_testflag==false | coroutineflag == true) {
			;
		} 
		else if (nextDraw != 0) {
			if(nowCard.function == Card.CardFunction.DrawTwo){
				waitingForPlayer = true;
				Card assume;
				if(nowPlayer == Player.player1){
					assume = player1.Card();
					//Debug.Log(assume.function);
					if(assume.color==Card.CardColor.None && assume.function == Card.CardFunction.None){
						drawCards(Player.player1,nextDraw);
						nextDraw = 0;
						showDrawCount();
						if(nowCard.function == Card.CardFunction.WildDrawFour)
						nowCard.function = Card.CardFunction.free;
						//setnowCardItem();
						turnOneRound ();
					}
					else if(player1Item.Exists(x=>x.cardItemCards.color == assume.color & x.cardItemCards.function == assume.function)==true & (assume.function == Card.CardFunction.DrawTwo )){
						nowCard = assume;
						//setnowCardItem();
						CardOperate(assume);
						removeCardFromHand(assume,Player.player1);
						//player1Hand.Remove(assume);
						if(player1Item.Count == 0){
							PlayerWinCount[3]++;
							gameEnd(Player.player1);
						}
					}
					else {
						Debug.Log("player1 use wrong card" + assume.color + assume.function );
					}
				}
				else if(nowPlayer == Player.player2){
					assume = player2.Card();
					//Debug.Log(assume.function);
					if(assume.color==Card.CardColor.None && assume.function == Card.CardFunction.None){
						drawCards(Player.player2,nextDraw);
						nextDraw = 0;
						showDrawCount();
						if(nowCard.function == Card.CardFunction.WildDrawFour)
						nowCard.function = Card.CardFunction.free;
						//setnowCardItem();
						turnOneRound ();
					}
					else if(player2Item.Exists(x=>x.cardItemCards.color == assume.color & x.cardItemCards.function == assume.function)==true & (assume.function == Card.CardFunction.DrawTwo )){
						nowCard = assume;
						//setnowCardItem();
						CardOperate(assume);
						removeCardFromHand(assume,Player.player2);
						//player2Hand.Remove(assume);
						if(player2Item.Count == 0){
							PlayerWinCount[3]++;
							gameEnd(Player.player2);
						}
					}
					else {
						Debug.Log("player2 use wrong card" + assume.color + assume.function );
					}
				}
				else if(nowPlayer == Player.player3){
					assume = player3.Card();
					//Debug.Log(assume.function);
					if(assume.color==Card.CardColor.None && assume.function == Card.CardFunction.None){
						drawCards(Player.player3,nextDraw);
						nextDraw = 0;
						showDrawCount();
						if(nowCard.function == Card.CardFunction.WildDrawFour)
						nowCard.function = Card.CardFunction.free;
						//setnowCardItem();
						turnOneRound ();
					}
					else if(player3Item.Exists(x=>x.cardItemCards.color == assume.color & x.cardItemCards.function == assume.function)==true & (assume.function == Card.CardFunction.DrawTwo )){
						nowCard = assume;
						//setnowCardItem();
						CardOperate(assume);
						removeCardFromHand(assume,Player.player3);
						//player3Hand.Remove(assume);
						if(player3Item.Count == 0){
							PlayerWinCount[3]++;
							gameEnd(Player.player3);
						}
					}
					else {
						Debug.Log("player3 use wrong card" + assume.color + assume.function );
					}
				}
				else if(nowPlayer == Player.player4){
					assume = player4.Card();
					//Debug.Log(assume.function);
					if(assume.color==Card.CardColor.None && assume.function == Card.CardFunction.None){
						drawCards(Player.player4,nextDraw);
						nextDraw = 0;
						showDrawCount();
						if(nowCard.function == Card.CardFunction.WildDrawFour)
						nowCard.function = Card.CardFunction.free;
						//setnowCardItem();
						turnOneRound ();
					}
					else if(player4Item.Exists(x=>x.cardItemCards.color == assume.color & x.cardItemCards.function == assume.function)==true & (assume.function == Card.CardFunction.DrawTwo )){
						nowCard = assume;
						//setnowCardItem();
						CardOperate(assume);
						removeCardFromHand(assume,Player.player4);
						//player4Hand.Remove(assume);
						if(player4Item.Count == 0){
							PlayerWinCount[3]++;
							gameEnd(Player.player4);
						}
					}
					else {
						Debug.Log("player4 use wrong card" + assume.color + assume.function );
					}
				}
			}
			else{
				waitingForPlayer = true;
				if(nowPlayer == Player.player1){
					drawCards(Player.player1,nextDraw);
					nextDraw = 0;
					showDrawCount();
					nowCard.function = Card.CardFunction.free;
					//setnowCardItem();
					turnOneRound ();
				}
				else if(nowPlayer == Player.player2){
					drawCards(Player.player2,nextDraw);
					nextDraw = 0;
					showDrawCount();
					nowCard.function = Card.CardFunction.free;
					//setnowCardItem();
					turnOneRound ();
				}
				else if(nowPlayer == Player.player3){
					drawCards(Player.player3,nextDraw);
					nextDraw = 0;
					showDrawCount();
					nowCard.function = Card.CardFunction.free;
					//setnowCardItem();
					turnOneRound ();
				}
				else if(nowPlayer == Player.player4){
					drawCards(Player.player4,nextDraw);
					nextDraw = 0;
					showDrawCount();
					nowCard.function = Card.CardFunction.free;
					//setnowCardItem();
					turnOneRound ();
				}
			}
		}
		else if(nextDraw == 0){
			waitingForPlayer = true;
			Card assume;
			if(nowPlayer == Player.player1){
				assume = player1.Card();
				//Debug.Log(assume.function);
				if(assume.color==Card.CardColor.None && assume.function == Card.CardFunction.None){
					if(drawflag == false){
						drawCards(Player.player1,1);
						drawflag = true;
					}
					else if(drawflag == true){
						drawflag = false;
						turnOneRound();
					}
				}
				else if(player1Item.Exists(x=>x.cardItemCards.color == assume.color & x.cardItemCards.function == assume.function)==true & nowCard.function == Card.CardFunction.free){
					nowCard = assume;
					//setnowCardItem();
					CardOperate(assume);
					removeCardFromHand(assume,Player.player1);
					if(player1Item.Count == 0){
						PlayerWinCount[0]++;
						gameEnd(Player.player1);
					}
				}
				else if(player1Item.Exists(x=>x.cardItemCards.color == assume.color & x.cardItemCards.function == assume.function)==true & (assume.function == nowCard.function | assume.color == nowCard.color | nowCard.color == Card.CardColor.Black)& assume.color != Card.CardColor.Black ){
					nowCard = assume;
					//setnowCardItem();
					CardOperate(assume);
					removeCardFromHand(assume,Player.player1);
					drawflag = false;
					if(player1Item.Count == 0){
						PlayerWinCount[0]++;
						gameEnd(Player.player1);
					}
				}
				else if (player1Item.Exists(x=>x.cardItemCards.color == assume.color & x.cardItemCards.function == assume.function)==true & assume.color == Card.CardColor.Black ){
					if(player1Item.Count==1){
						Debug.Log("wronging: Black card can't play as last card");
					}
					else{
						nowCard = assume;
						CardOperate(assume);
						//Debug.Log(player1Item.Exists(x=>x.cardItemCards.color == assume.color & x.cardItemCards.function == assume.function));
						removeCardFromHand(assume,Player.player1);
						drawflag = false;
					}

				}
				else {
					//waitingForPlayer = false;
					Debug.Log("player1 use wrong card" + assume.color + assume.function );
				}
				//waitingForPlayer = false;
			}
			else if(nowPlayer == Player.player2){
				assume = player2.Card();
				if(assume.color==Card.CardColor.None && assume.function == Card.CardFunction.None){
					if(drawflag == false){
						drawCards(Player.player2,1);
						drawflag = true;
					}
					else if(drawflag == true){
						drawflag = false;
						turnOneRound();
					}
				}
				else if(player2Item.Exists(x=>x.cardItemCards.color == assume.color & x.cardItemCards.function == assume.function)==true & nowCard.function == Card.CardFunction.free){
					nowCard = assume;
					//setnowCardItem();
					CardOperate(assume);
					removeCardFromHand(assume,Player.player2);
					if(player2Item.Count == 0){
						PlayerWinCount[1]++;
						gameEnd(Player.player2);
					}
				}
				else if(player2Item.Exists(x=>x.cardItemCards.color == assume.color & x.cardItemCards.function == assume.function)==true & (assume.function == nowCard.function | assume.color == nowCard.color  | nowCard.color == Card.CardColor.Black)& assume.color != Card.CardColor.Black){
					nowCard = assume;
					//setnowCardItem();
					CardOperate(assume);
					removeCardFromHand(assume,Player.player2);
					drawflag = false;
					if(player2Item.Count == 0){
						PlayerWinCount[1]++;
						gameEnd(Player.player2);
					}
				}
				
				else if (player2Item.Exists(x=>x.cardItemCards.color == assume.color & x.cardItemCards.function == assume.function)==true & assume.color == Card.CardColor.Black ){
					if(player2Item.Count==1){
						Debug.Log("wronging: Black card can't play as last card");
					}
					else{
						nowCard = assume;
						CardOperate(assume);
						removeCardFromHand(assume,Player.player2);
						drawflag = false;
					}
					
				}
				else {
					Debug.Log("player2 use wrong card" + assume.color + assume.function );
				}
				//waitingForPlayer = false;
			}
			else if(nowPlayer == Player.player3){
				assume = player3.Card();
				//Debug.Log(assume.function);
				if(assume.color==Card.CardColor.None && assume.function == Card.CardFunction.None){
					if(drawflag == false){
						drawCards(Player.player3,1);
						drawflag = true;
					}
					else if(drawflag == true){
						drawflag = false;
						turnOneRound();
					}
				}
				else if(player3Item.Exists(x=>x.cardItemCards.color == assume.color & x.cardItemCards.function == assume.function)==true & nowCard.function == Card.CardFunction.free){
					nowCard = assume;
					//setnowCardItem();
					CardOperate(assume);
					removeCardFromHand(assume,Player.player3);
					if(player3Item.Count == 0){
						PlayerWinCount[2]++;
						gameEnd(Player.player3);
					}
				}
				else if(player3Item.Exists(x=>x.cardItemCards.color == assume.color & x.cardItemCards.function == assume.function)==true & (assume.function == nowCard.function | assume.color == nowCard.color  | nowCard.color == Card.CardColor.Black)& assume.color != Card.CardColor.Black){
					nowCard = assume;
					//setnowCardItem();
					CardOperate(assume);
					removeCardFromHand(assume,Player.player3);
					drawflag = false;
					if(player3Item.Count == 0){
						PlayerWinCount[2]++;
						gameEnd(Player.player3);
					}
				}
				else if (player3Item.Exists(x=>x.cardItemCards.color == assume.color & x.cardItemCards.function == assume.function)==true & assume.color == Card.CardColor.Black ){
					if(player3Item.Count==1){
						Debug.Log("wronging: Black card can't play as last card");
					}
					else{
						nowCard = assume;
						CardOperate(assume);
						removeCardFromHand(assume,Player.player3);
						drawflag = false;
					}
					
				}
				else {
					Debug.Log("player3 use wrong card" + assume.color + assume.function );
				}
				//waitingForPlayer = false;
			}
			else if(nowPlayer == Player.player4){
				assume = player4.Card();
				//Debug.Log(assume.function);
				if(assume.color==Card.CardColor.None && assume.function == Card.CardFunction.None){
					if(drawflag == false){
						drawCards(Player.player4,1);
						drawflag = true;
					}
					else if(drawflag == true){
						drawflag = false;
						turnOneRound();
					}
				}
				else if(player4Item.Exists(x=>x.cardItemCards.color == assume.color & x.cardItemCards.function == assume.function)==true & nowCard.function == Card.CardFunction.free){
					nowCard = assume;
					//setnowCardItem();
					CardOperate(assume);
					removeCardFromHand(assume,Player.player4);
					if(player4Item.Count == 0){
						PlayerWinCount[3]++;
						gameEnd(Player.player4);
					}
				}
				else if(player4Item.Exists(x=>x.cardItemCards.color == assume.color & x.cardItemCards.function == assume.function)==true & (assume.function == nowCard.function | assume.color == nowCard.color  | nowCard.color == Card.CardColor.Black)& assume.color != Card.CardColor.Black){
					nowCard = assume;
					//setnowCardItem();
					CardOperate(assume);
					removeCardFromHand(assume,Player.player4);
					drawflag = false;
					if(player4Item.Count == 0){
						PlayerWinCount[3]++;
						gameEnd(Player.player4);
					}
				}
				else if (player4Item.Exists(x=>x.cardItemCards.color == assume.color & x.cardItemCards.function == assume.function)==true & assume.color == Card.CardColor.Black ){
					if(player4Item.Count==1){
						Debug.Log("wronging: Black card can't play as last card");
					}
					else{
						nowCard = assume;
						CardOperate(assume);
						removeCardFromHand(assume,Player.player4);
						drawflag = false;
					}
					
				}
				else {
					Debug.Log("player4 use wrong card" + assume.color + assume.function );
				}
			}
		}
	}

	void CardOperate(Card card){
		if (card.function == Card.CardFunction.Skip) {
			turnOneRound ();
			turnOneRound ();
		} 
		else if (card.function == Card.CardFunction.Reverse) {
			if (turnOrder == TurnOrder.clockwise)
				turnOrder = TurnOrder.counterclockwise;
			else if (turnOrder == TurnOrder.counterclockwise)
				turnOrder = TurnOrder.clockwise;
			turnOneRound ();
		} 
		else if (card.function == Card.CardFunction.DrawTwo) {
			nextDraw += 2;
			showDrawCount();
			turnOneRound();
		} 
		else if (card.function == Card.CardFunction.Wild) {
			changeColor();
			turnOneRound();
		} 
		else if (card.function == Card.CardFunction.WildDrawFour) {
			changeColor();
			nextDraw += 4;
			showDrawCount();
			turnOneRound();
		} 
		else {
			turnOneRound();
		}
	}

	void removeCardFromHand(Card card,Player player){
		if(player == Player.player1){
			int rmCardN = player1Item.FindIndex(x=>x.cardItemCards.color == card.color&&x.cardItemCards.function == card.function);
			cardItemAndCards rmCard = player1Item[rmCardN];
			player1Item.RemoveAt(rmCardN);
			StartCoroutine(moveToNow(rmCard.cardItem));
			changePosition(player1Item,Player.player1);
			//Debug.Log(player1Item.Exists(x=>x.Equals(rmCard)));
		}
		else if(player == Player.player2){
			int rmCardN = player2Item.FindIndex(x=>x.cardItemCards.color == card.color&&x.cardItemCards.function == card.function);
			cardItemAndCards rmCard = player2Item[rmCardN];
			player2Item.RemoveAt(rmCardN);
			rmCard.cardItem.transform.Rotate(0,0,-90);
			StartCoroutine(moveToNow(rmCard.cardItem));
			changePosition(player2Item,Player.player2);
		}
		else if(player == Player.player3){
			int rmCardN = player3Item.FindIndex(x=>x.cardItemCards.color == card.color&&x.cardItemCards.function == card.function);
			cardItemAndCards rmCard = player3Item[rmCardN];
			player3Item.RemoveAt(rmCardN);
			StartCoroutine(moveToNow(rmCard.cardItem));
			changePosition(player3Item,Player.player3);
		}
		else if(player == Player.player4){
			int rmCardN = player4Item.FindIndex(x=>x.cardItemCards.color == card.color&&x.cardItemCards.function == card.function);
			cardItemAndCards rmCard = player4Item[rmCardN];
			player4Item.RemoveAt(rmCardN);
			rmCard.cardItem.transform.Rotate(0,0,-270);
			StartCoroutine(moveToNow(rmCard.cardItem));
			changePosition(player4Item,Player.player4);
		}
		discard.Add(card);
	}

	IEnumerator moveToNow(GameObject tar){
		Vector2 end = new Vector2 (-68, -2.5f);
		float movetime = (fast_testflag) ? 30f : 3f;
		coroutineflag = true;
		while (true) {
			Vector2 start = tar.GetComponent<Image>().rectTransform.anchoredPosition;
			if(start==end)
				break;
			tar.GetComponent<Image>().rectTransform.anchoredPosition = Vector2.MoveTowards(start,end,movetime);
			yield return 0;
		
		}
		setNowCardItem ();
		coroutineflag = false;
	}

	void resetCardFromHand(Card card,Player player){
		if(player == Player.player1){
			int rmCardN = player1Item.FindIndex(x=>x.cardItemCards.color == card.color&&x.cardItemCards.function == card.function);
			cardItemAndCards rmCard = player1Item[rmCardN];
			player1Item.RemoveAt(rmCardN);
			SetItemPosition(rmCard.cardItem);
			//Debug.Log(player1Item.Exists(x=>x.Equals(rmCard)));
		}
		else if(player == Player.player2){
			int rmCardN = player2Item.FindIndex(x=>x.cardItemCards.color == card.color&&x.cardItemCards.function == card.function);
			cardItemAndCards rmCard = player2Item[rmCardN];
			player2Item.RemoveAt(rmCardN);
			rmCard.cardItem.transform.Rotate(0,0,-90);
			SetItemPosition(rmCard.cardItem);
		}
		else if(player == Player.player3){
			int rmCardN = player3Item.FindIndex(x=>x.cardItemCards.color == card.color&&x.cardItemCards.function == card.function);
			cardItemAndCards rmCard = player3Item[rmCardN];
			player3Item.RemoveAt(rmCardN);
			SetItemPosition(rmCard.cardItem);
		}
		else if(player == Player.player4){
			int rmCardN = player4Item.FindIndex(x=>x.cardItemCards.color == card.color&&x.cardItemCards.function == card.function);
			cardItemAndCards rmCard = player4Item[rmCardN];
			player4Item.RemoveAt(rmCardN);
			rmCard.cardItem.transform.Rotate(0,0,-270);
			SetItemPosition(rmCard.cardItem);
		}
		discard.Add(card);
	}

	void addCardToDeck(){
		List<Card> temp = deck;
		deck = discard;
		discard = temp;
		shuffle ();
	}

	void setNowCardItem(){
		//if (nowCarditem != null) {
			SetItemPosition (nowCarditem);

		nowCarditem = cardSortByID[nowCard.CardID];

		nowColorImage.color = (nowCard.color == Card.CardColor.Blue) ? Color.blue :
			(nowCard.color == Card.CardColor.Red) ? Color.red :
			(nowCard.color == Card.CardColor.Yellow) ? Color.yellow :
			(nowCard.color == Card.CardColor.Green) ? Color.green :
			Color.white;
		/*
		Image surface = nowCarditem.GetComponent<Image>();
		surface.color = (nowCard.color==Card.CardColor.Red)?Color.red :
			(nowCard.color==Card.CardColor.Blue)?Color.blue :
				(nowCard.color==Card.CardColor.Green)?Color.green :
				(nowCard.color==Card.CardColor.Yellow)?Color.yellow :
				Color.gray;
		Text word = nowCarditem.GetComponentInChildren<Text>();
		word.text = nowCard.function.ToString();*/
	}

	void changeColor(){
		if(nowPlayer == Player.player1){
			Card.CardColor newColor = player1.chooseColor();
			if(newColor == Card.CardColor.Blue||newColor == Card.CardColor.Red || newColor == Card.CardColor.Yellow||newColor == Card.CardColor.Green){
				nowCard.color = newColor;
				//setnowCardItem();
			}
			else{
				Debug.Log("no color");
			}
		}
		else if(nowPlayer == Player.player2){
			Card.CardColor newColor = player2.chooseColor();
			if(newColor == Card.CardColor.Blue||newColor == Card.CardColor.Red || newColor == Card.CardColor.Yellow||newColor == Card.CardColor.Green){
				nowCard.color = newColor;
				//setnowCardItem();
			}
			else{
				Debug.Log("no color");
			}
		}
		else if(nowPlayer == Player.player3){
			Card.CardColor newColor = player3.chooseColor();
			if(newColor == Card.CardColor.Blue||newColor == Card.CardColor.Red || newColor == Card.CardColor.Yellow||newColor == Card.CardColor.Green){
				nowCard.color = newColor;
				//setnowCardItem();
			}
			else{
				Debug.Log("no color");
			}
		}
		else if(nowPlayer == Player.player4){
			Card.CardColor newColor = player4.chooseColor();
			if(newColor == Card.CardColor.Blue||newColor == Card.CardColor.Red || newColor == Card.CardColor.Yellow||newColor == Card.CardColor.Green){
				nowCard.color = newColor;
				//setnowCardItem();
			}
			else{
				Debug.Log("no color");
			}
		}
	}

	void showDrawCount(){
		if (nextDraw == 0) {
			DrawCount.text = "";
		} 
		else {
			DrawCount.text = ("+"+nextDraw);
		}
	}
	void gameEnd(Player winner){
		test_setData ();
		WinnerText.text = winner.ToString()+" Win!";
		//if(winner == Player.player1)
		gameState = GameState.end;
	}

	void setNextFirstPlayer(){
		if (!randomOrderFlag) {
			if (firstPlayer == Player.player1)
				firstPlayer = Player.player2;
			else if (firstPlayer == Player.player2)
				firstPlayer = Player.player3;
			else if (firstPlayer == Player.player3)
				firstPlayer = Player.player4;
			else if (firstPlayer == Player.player4)
				firstPlayer = Player.player1;
		} 
		else {
			int[] order = new int[4]{ 0, 1, 2, 3 };
			for (int i = 0; i<3; i++) {
				int a = Random.Range(i,4);
				int temp = order[a];
				order[a] = order[i];
				order[i] = temp;
			}
			for (int i = 0; i<4; i++) {
				playerOrder [i] = Player.player1 + order [i];
			}
			firstPlayer = playerOrder [0];
			randomOrderText.text = "Order: " + (order [0] + 1) + "->" + (order [1] + 1) + "->" + (order [2] + 1) + "->" + (order [3] + 1);
		}
	}

	void autoRun(){
		if (autoRunCount != 0 && coroutineflag != true) {
			autoRunCount--;
			autoRunInput.text = autoRunCount.ToString ();
			auto_clearall ();
		} 

	}

	public void auto_clearall(){
		nowColorImage.color = Color.white;
		nowCard.color = Card.CardColor.None;
		nowCard.function = Card.CardFunction.free;
		foreach(GameObject cs in cardSortByID)
			SetItemPosition (cs);
		while (player1Item.Count!=0) {
			resetCardFromHand(player1Item[0].cardItemCards,Player.player1);
		}
		while (player2Item.Count!=0) {
			resetCardFromHand(player2Item[0].cardItemCards,Player.player2);
		}
		while (player3Item.Count!=0) {
			resetCardFromHand(player3Item[0].cardItemCards,Player.player3);
		}
		while (player4Item.Count!=0) {
			resetCardFromHand(player4Item[0].cardItemCards,Player.player4);
		}
		setArrow ();
		player1.restart ();
		player2.restart ();
		player3.restart ();
		player4.restart ();
		setNextFirstPlayer ();
		deck.Clear ();
		discard.Clear ();
		nowPlayer = firstPlayer;
		nextDraw = 0;
		showDrawCount ();
		WinnerText.text = "";
		waitingForPlayer = false;
		drawflag = false;
		//fast_testflag = false;
		gameState = GameState.initial;
		/*for (int i=0; i<4; i++)
			PlayerWinCount [i] = 0;
		test_setData ();*/
	}

	public void clearall(){
		nowColorImage.color = Color.white;
		nowCard.color = Card.CardColor.None;
		nowCard.function = Card.CardFunction.free;
		SetItemPosition (nowCarditem);
		while (player1Item.Count!=0) {
			resetCardFromHand(player1Item[0].cardItemCards,Player.player1);
		}
		while (player2Item.Count!=0) {
			resetCardFromHand(player2Item[0].cardItemCards,Player.player2);
		}
		while (player3Item.Count!=0) {
			resetCardFromHand(player3Item[0].cardItemCards,Player.player3);
		}
		while (player4Item.Count!=0) {
			resetCardFromHand(player4Item[0].cardItemCards,Player.player4);
		}
		setArrow ();
		player1.restart ();
		player2.restart ();
		player3.restart ();
		player4.restart ();
		firstPlayer = Player.player1;
		deck.Clear ();
		discard.Clear ();
		nowPlayer = Player.player1;
		nextDraw = 0;
		showDrawCount ();
		WinnerText.text = "";
		waitingForPlayer = false;
		drawflag = false;
		fast_testflag = false;
		fastToggle.isOn = false;
		gameState = GameState.initial;
		for (int i=0; i<4; i++)
			PlayerWinCount [i] = 0;
		test_setData ();
	}

	public void test_next(){
		waitingForPlayer = false;
	}
	public void test_fast(){
		if (fastToggle.isOn == true)
			fast_testflag = true;
		else
			fast_testflag = false;
	}
	public void test_randomOrder(){
		randomOrderFlag = randomOrderToggle.isOn;

	}
	public void test_setData(){
		Text[] Pwin = DataUI.GetComponentsInChildren<Text>();
		Pwin [0].text = "Player1:"+PlayerWinCount[0];
		Pwin [1].text = "Player2:"+PlayerWinCount[1];
		Pwin [2].text = "Player3:"+PlayerWinCount[2];
		Pwin [3].text = "Player4:"+PlayerWinCount[3];
	}

	public void test_autoRun(){
		int n = System.Convert.ToInt32 (autoRunInput.text);
		autoRunCount = n;
	}
	public int getHandCount(Player tar){
		if (tar == Player.player1) {
			return player1Item.Count;
		}
		else if (tar == Player.player2) {
			return player2Item.Count;
		}
		else if (tar == Player.player3) {
			return player3Item.Count;
		}
		else {
			return player4Item.Count;
		}
	}
	public int nextPlayerInt(){/*
		if (turnOrder == TurnOrder.clockwise) {
			if (nowPlayer == Player.player1)
				return 3;
			else if (nowPlayer == Player.player2)
				return 0;
			else if (nowPlayer == Player.player3)
				return 1;
			else 
				return 2;
		} 
		else {
			if (nowPlayer == Player.player4)
				return 0;
			else if (nowPlayer == Player.player3)
				return 3;
			else if (nowPlayer == Player.player2)
				return 2;
			else 
				return 1;
		}*/
		if (turnOrder == TurnOrder.clockwise) {
			if (nowPlayer == playerOrder [0]) {
				return playerOrder [1] - Player.player1;
			}
			else if (nowPlayer == playerOrder [1]) {
				return playerOrder [2] - Player.player1;
			}
			else if (nowPlayer == playerOrder [2]) {
				return playerOrder [3] - Player.player1;
			}
			else {
				return playerOrder [0] - Player.player1;
			}
		} 
		else {
			if (nowPlayer == playerOrder [0]) {
				return playerOrder [3] - Player.player1;
			}
			else if (nowPlayer == playerOrder [3]) {
				return playerOrder [2] - Player.player1;
			}
			else if (nowPlayer == playerOrder [2]) {
				return playerOrder [1] - Player.player1;
			}
			else {
				return playerOrder [0] - Player.player1;
			}
		} 

	}
	public Player nextPlayer(){
		/*
		if (turnOrder == TurnOrder.clockwise) {
			if (nowPlayer == Player.player1)
				return Player.player4;
			else 
				return nowPlayer-1;
		} 
		else {
			if (nowPlayer == Player.player4)
				return Player.player1;
			else 
				return nowPlayer+1;
		}*/
		if (turnOrder == TurnOrder.clockwise) {
			if (nowPlayer == playerOrder [0]) {
				return playerOrder [1];
			}
			else if (nowPlayer == playerOrder [1]) {
				return playerOrder [2];
			}
			else if (nowPlayer == playerOrder [2]) {
				return playerOrder [3];
			}
			else{
				return playerOrder [0];
			}
		} 
		else {
			if (nowPlayer == playerOrder [0]) {
				return playerOrder [3];
			}
			else if (nowPlayer == playerOrder [3]) {
				return playerOrder [2];
			}
			else if (nowPlayer == playerOrder [2]) {
				return playerOrder [1];
			}
			else {
				return playerOrder [0];
			}
		} 
	}

	public int prevPlayerInt(){/*
		if (turnOrder == TurnOrder.clockwise) {
			if (nowPlayer == Player.player1)
				return 1;
			else if (nowPlayer == Player.player2)
				return 2;
			else if (nowPlayer == Player.player3)
				return 3;
			else 
				return 0;
		} 
		else {
			if (nowPlayer == Player.player4)
				return 2;
			else if (nowPlayer == Player.player3)
				return 1;
			else if (nowPlayer == Player.player2)
				return 0;
			else 
				return 3;
		}*/
		if (turnOrder == TurnOrder.counterclockwise) {
			if (nowPlayer == playerOrder [0]) {
				return playerOrder [1] - Player.player1;
			}
			else if (nowPlayer == playerOrder [1]) {
				return playerOrder [2] - Player.player1;
			}
			else if (nowPlayer == playerOrder [2]) {
				return playerOrder [3] - Player.player1;
			}
			else {
				return playerOrder [0] - Player.player1;
			}
		} 
		else {
			if (nowPlayer == playerOrder [0]) {
				return playerOrder [3] - Player.player1;
			}
			else if (nowPlayer == playerOrder [3]) {
				return playerOrder [2] - Player.player1;
			}
			else if (nowPlayer == playerOrder [2]) {
				return playerOrder [1] - Player.player1;
			}
			else {
				return playerOrder [0] - Player.player1;
			}
		} 
	}
	public Player prevPlayer(){/*
		if (turnOrder == TurnOrder.clockwise) {
			if (nowPlayer == Player.player4)
				return Player.player1;
			else 
				return nowPlayer+1;
		} 
		else {
			if (nowPlayer == Player.player1)
				return Player.player4;
			else 
				return nowPlayer-1;
		}*/
		if (turnOrder == TurnOrder.counterclockwise) {
			if (nowPlayer == playerOrder [0]) {
				return playerOrder [1];
			}
			else if (nowPlayer == playerOrder [1]) {
				return playerOrder [2];
			}
			else if (nowPlayer == playerOrder [2]) {
				return playerOrder [3];
			}
			else {
				return playerOrder [0];
			}
		} 
		else{
			if (nowPlayer == playerOrder [0]) {
				return playerOrder [3];
			}
			else if (nowPlayer == playerOrder [3]) {
				return playerOrder [2];
			}
			else if (nowPlayer == playerOrder [2]) {
				return playerOrder [1];
			}
			else {
				return playerOrder [0];
			}
		} 
	}

	public int skipPlayerInt(){

		if (nowPlayer == Player.player1)
			return 2;
		else if (nowPlayer == Player.player2)
			return 3;
		else if (nowPlayer == Player.player3)
			return 0;
		else 
			return 1;
	}
	public Player skipPlayer(){

		if (nowPlayer == Player.player1)
			return Player.player3;
		else if (nowPlayer == Player.player2)
			return Player.player4;
		else if (nowPlayer == Player.player3)
			return Player.player1;
		else 
			return Player.player2;
	}
	public int deckcount(){
		return discard.Count;
	}
	public int getDeckCount(){
		return deck.Count;
	}
}
