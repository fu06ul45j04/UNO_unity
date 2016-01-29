using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerModle: MonoBehaviour {



	List<Card> hands = new List<Card> ();
	public Game game;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//throw a card 
	//if no card return color and function equal to None
	public Card Card(){
		if (game.nowCard.function == global::Card.CardFunction.free) {
			Debug.Log("free now");
			int i=0;
			while(hands[i].function==global::Card.CardFunction.WildDrawFour)
				i++;
			Card Y = hands[i];
			hands.RemoveAt(i);
			Debug.Log(Y.function);
			return Y;
		}if (game.nowCard.function == global::Card.CardFunction.DrawTwo & game.nextDraw != 0 ) {
			if (hands.Exists (x=>x.function == game.nowCard.function)) {
				int Z = hands.FindIndex (x=>x.function == game.nowCard.function);
				Card Tk2 = hands[Z];
				hands.RemoveAt(Z);
				Debug.Log(Tk2.function);
				return Tk2; 
			} 
			else {
				Card T = new global::Card ();
				T.function = global::Card.CardFunction.None;
				T.color = global::Card.CardColor.None;
				return T; 
			}
		} 
		else {
			if(game.nowCard.function == global::Card.CardFunction.free & hands.Exists (x => x.color == game.nowCard.color)){
				int Z = hands.FindIndex (x => x.color == game.nowCard.color||x.function==global::Card.CardFunction.Wild);
				Card Tk = hands[Z];
				hands.RemoveAt(Z);
				return Tk;
			}
			else if (hands.Exists (x => x.color == game.nowCard.color||x.function == game.nowCard.function||x.function==global::Card.CardFunction.Wild)) {
				int Z = hands.FindIndex (x => x.color == game.nowCard.color||x.function == game.nowCard.function||x.function==global::Card.CardFunction.Wild);
				Card Tk = hands[Z];
				hands.RemoveAt(Z);
				Debug.Log(Tk.function);
				return Tk; 
			} 
			else if(hands.Exists (x => x.function == global::Card.CardFunction.WildDrawFour)){
				int Z = hands.FindIndex (x => x.function == global::Card.CardFunction.WildDrawFour);
				Card Tk = hands[Z];
				hands.RemoveAt(Z);
				Debug.Log(Tk.function);
				return Tk;
			}
			else {
				Card T = new global::Card ();
				T.function = global::Card.CardFunction.None;
				T.color = global::Card.CardColor.None;
				return T; 
			}
		}
	}

	//get a card to hands
	public void getCard(Card newcard){
		hands.Add (newcard);
	}

	//pick a color if you throw a wild card
	public Card.CardColor chooseColor(){
		return global::Card.CardColor.Blue;
	}

	public void restart(){
		hands.Clear ();
	}

}
