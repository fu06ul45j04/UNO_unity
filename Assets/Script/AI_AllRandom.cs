﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AI_AllRandom : MonoBehaviour {
	Game game ;
	void Start(){
		game = GameObject.Find ("GameManager").GetComponent<Game> ();
	}

	List<Card> hands = new List<Card> ();
	//-Functions-
	public Card Card(){
		if (_GetNowCard().function == global::Card.CardFunction.DrawTwo & _GetNextDraw()!= 0 ) {
			if (hands.Exists (x=>x.function == _GetNowCard().function)) {
				List<Card> Z = hands.FindAll (x=>x.function == _GetNowCard().function);
				Card Tk2 = Z[Random.Range(0,Z.Count-1)];
				int zi = hands.FindIndex(x=>x.function ==Tk2.function & x.color == Tk2.color);
				hands.RemoveAt(zi);
				return Tk2; 
			} 
			else {
				return _NoCard(); 
			}
		} 
		else {
			List<Card> Z = new List<Card>();
			if (hands.Exists (x => x.color == game.nowCard.color||x.function == game.nowCard.function)) {
				Z = hands.FindAll (x => x.color == game.nowCard.color||x.function == game.nowCard.function);
			} 
			else if(hands.Exists (x => x.function == global::Card.CardFunction.WildDrawFour ||x.function==global::Card.CardFunction.Wild ) & hands.Count != 1){
				List<Card> Z2 = hands.FindAll (x => x.function == global::Card.CardFunction.WildDrawFour ||x.function==global::Card.CardFunction.Wild );
				foreach(Card c in Z2)
					Z.Add(c);
			}
			if(Z.Count ==0)
				return _NoCard();

			Card Tk = Z[Random.Range(0,Z.Count-1)];
			int zi = hands.FindIndex(x=>x.function ==Tk.function & x.color == Tk.color);
			hands.RemoveAt(zi);
			return Tk; 
		}
	} //pick a Card to Card
	public void getCard(Card newcard){
		hands.Add (newcard);
	} //recive a Card
	public Card.CardColor chooseColor(){
		float seed = Random.value;
		return (seed<0.25)?global::Card.CardColor.Blue :
				(seed<0.5)?global::Card.CardColor.Yellow :
				(seed<0.75)?global::Card.CardColor.Red : 
				global::Card.CardColor.Green;
	}
	public void restart(){
		hands.Clear ();
	}
	
	//-API-
	Card _GetNowCard(){return game.nowCard;} //card puted in the top
	Game.Player _GetNowPlayer(){return game.nowPlayer;} //now player
	int _GetNextDraw(){return game.nextDraw;} //count of next draw
	Card _NoCard(){		//no card to card
		Card T = new global::Card ();
		T.function = global::Card.CardFunction.None;
		T.color = global::Card.CardColor.None;
		return T;
	}
}
