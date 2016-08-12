using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerCountrol : MonoBehaviour {
	Game game ;
	Canvas canvas;
	public GameObject buttonPrefab;
	int cardSelected;
	int colorSelected;
	bool hasColor;
	bool hasColorButton;
	List<Card> hands = new List<Card> ();
	List<GameObject> buttons = new List<GameObject>();
	// Use this for initialization
	void Start(){
		game = GameObject.Find ("GameManager").GetComponent<Game> ();
		canvas = GameObject.Find ("Canvas").GetComponent<Canvas>();;
		hasColor = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public Card Card(){
		if (buttons.Count == 0) {
			createButton ();
			hands.Sort((s1,s2)=> s1.CardID.CompareTo(s2.CardID));
		}
		//selectCard ();
		if (cardSelected == 110) {
			deleteButton ();
			Card tempCard = new global::Card ();
			tempCard.color = global::Card.CardColor.None;
			tempCard.function = global::Card.CardFunction.None;
			return tempCard;
		}
		else if (cardSelected != 109) {
			if(hands[cardSelected].color==global::Card.CardColor.Black & hasColor == false){
				hasColor = false;
				if (hasColorButton == false) {
					createColorButton ();
					hasColorButton = true;
				}

				Card newcard = new global::Card();
				newcard.function = global::Card.CardFunction.zero;
				newcard.color = global::Card.CardColor.Black;
				return newcard;
			}
			else{
				deleteButton ();
				Card tempCard = hands [cardSelected];
				hands.RemoveAt(cardSelected);
				return tempCard ;
			}
		}
		else {
			Card newcard = new global::Card();
			newcard.function = global::Card.CardFunction.zero;
			newcard.color = global::Card.CardColor.Black;
			//Debug.Log ("wait");
			return newcard;
		}
	}
	public void getCard(Card newcard){
		hands.Add (newcard);
	}
	public Card.CardColor chooseColor(){
		hasColor = false;
		return (colorSelected==0)?global::Card.CardColor.Blue:
			(colorSelected==1)?global::Card.CardColor.Yellow:
				(colorSelected==2)?global::Card.CardColor.Red:global::Card.CardColor.Green;
	}
	public void restart(){
		hands.Clear ();
	}

	public void createButton(){
		for (int i=0; i<hands.Count; i++) {
			GameObject newButton = Instantiate(buttonPrefab);
			newButton.transform.SetParent(canvas.transform);
			newButton.transform.localPosition = new Vector2(-172f+23*i,-131.5f);
			newButton.transform.localScale = new Vector3(1,1,1);
			int a = i;
			newButton.GetComponent<Button>().onClick.AddListener(delegate{ddebug(a);});
			buttons.Add(newButton);

		}

		GameObject drawButton = Instantiate(buttonPrefab);
		drawButton.transform.SetParent(canvas.transform);
		drawButton.transform.localPosition = new Vector2(-195f,-131.5f);
		drawButton.transform.localScale = new Vector3(1,1,1);
		drawButton.GetComponent<Image> ().color = Color.blue;
		drawButton.GetComponent<Button>().onClick.AddListener(delegate{ddebug(110);});
		buttons.Add(drawButton);
		cardSelected = 109;
	}
	public void createColorButton(){
		for (int i=0; i<4; i++) {
			GameObject newButton = Instantiate(buttonPrefab);
			newButton.transform.SetParent(canvas.transform);
			newButton.transform.localPosition = new Vector2(-172f+23*(i+(hands.Count)),-131.5f);
			newButton.transform.localScale = new Vector3(1,1,1);
			newButton.GetComponent<Image> ().color = (i==0)?Color.blue:(i==1)?Color.yellow:(i==2)?Color.red:Color.green;
			int a = i;
			newButton.GetComponent<Button>().onClick.AddListener(delegate{colorSelecte(a);});
			buttons.Add(newButton);
			
		}
	}
	public int selectCard(){
		return cardSelected;
	}
	public void deleteButton(){
		for (int i=buttons.Count-1; i>=0; i--) {
			GameObject newButton = buttons[i];
			Destroy(newButton);
		}
		buttons.Clear ();
	}
	public void ddebug(int bb){
		cardSelected = bb;

	}
	public void colorSelecte(int tar){
		colorSelected = tar;
		hasColor = true;
		hasColorButton = false;
	}
}
