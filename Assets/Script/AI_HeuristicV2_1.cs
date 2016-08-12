using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AI_HeuristicV2_1 : MonoBehaviour {
	Game game;
	List<Card> hands = new List<Card>();
	int color_count_red =0;
	int color_count_yellow =0;
	int color_count_green =0;
	int color_count_blue =0;
	int color_count_black =0;
	//Card nowcard;
	Game.Player nowPlayer;
	Card[] lastcard = new Card[4];
	//-Functions-
	void Start(){
		game = GameObject.Find ("GameManager").GetComponent<Game> ();
		nowPlayer = _GetNowPlayer();
	}
	void Update(){
		if ( nowPlayer != _GetNowPlayer () ) {
			//Debug.Log("record");
			recordCard();
			nowPlayer = _GetNowPlayer();
		}
	}
	public void recordCard(){
		if (nowPlayer == Game.Player.player1) {
			lastcard[0] = _GetNowCard();
		}
		else if (nowPlayer == Game.Player.player2) {
			lastcard[1] = _GetNowCard();
		}
		else if (nowPlayer == Game.Player.player3) {
			lastcard[2] = _GetNowCard();
		}
		else if (nowPlayer == Game.Player.player4) {
			lastcard[3] = _GetNowCard();
		}
	}
	public Card Card(){
		Card maxPriorityCard = _NoCard() ;
		int maxPriority = 0;
		int[] cardPriority = new int[hands.Count];
		for (int i=0; i<hands.Count; i++) {
			if(_GetHandCount(_GetNextPlayer())<=2){
				if ((lastcard[_GetNextPlayerInt()].color == hands[i].color) & (hands[i].function != global::Card.CardFunction.DrawTwo)
					& (hands[i].function != global::Card.CardFunction.WildDrawFour)
					& (hands[i].function != global::Card.CardFunction.Skip))
				{
					cardPriority[i] -= 3;
				}
				else if((_GetNowCard().color == hands[i].color) & (hands[i].function == global::Card.CardFunction.DrawTwo)){
					cardPriority[i] += 2;                                                                          
				}
				else if((_GetNowCard().color == hands[i].color) & _GetNextDraw() == 0 & ((hands[i].function == global::Card.CardFunction.DrawTwo)
					| (hands[i].function == global::Card.CardFunction.WildDrawFour)
					| (hands[i].function == global::Card.CardFunction.Skip)
					| (hands[i].function == global::Card.CardFunction.Reverse))){
					cardPriority[i] += 1;                                                                          
				}
			}
			if (hands[i].color == _GetNowCard().color & _GetNextDraw() == 0) {
				cardPriority[i] += 4;
			}
			else if (hands[i].function == _GetNowCard().function) {
				cardPriority[i] += 3;
				if (hands[i].color == maxCardColor())
					cardPriority[i] += 2;
			}
			else if (hands[i].color == global::Card.CardColor.Black & _GetNextDraw() == 0) {
				if (hands.Count - color_count_black == 1)
					cardPriority [i] += 6;
				else if (hands.Count == 1)
					cardPriority [i] = 0;
				else 
					cardPriority[i] += 2;
			}
			if (cardPriority[i] > maxPriority){
				maxPriorityCard = hands[i];
				maxPriority = cardPriority[i];
			}
		}
		if (maxPriority == 0)
			return _NoCard ();
		else {
			int index = hands.FindIndex(x=>x.function ==maxPriorityCard.function & x.color == maxPriorityCard.color);
			hands.RemoveAt(index);
			reduceColorCount(maxPriorityCard);
			//Debug.Log(maxPriorityCard.color);
			//Debug.Log(maxPriorityCard.function);
			return maxPriorityCard;
		}
	} //pick a Card to Card
	public void getCard(Card newcard){
		hands.Add (newcard);
		if (newcard.color == global::Card.CardColor.Red)
			color_count_red++;
		else if (newcard.color == global::Card.CardColor.Yellow)
			color_count_yellow++;
		else if (newcard.color == global::Card.CardColor.Green)
			color_count_green++;
		else if (newcard.color == global::Card.CardColor.Blue)
			color_count_blue++;
		else if (newcard.color == global::Card.CardColor.Black)
			color_count_black++;
	} //recive a Card
	public Card.CardColor chooseColor(){return maxCardColor();} //when you card wild
	public void restart(){
		hands.Clear ();
		color_count_red = 0;
		color_count_yellow = 0;
		color_count_green = 0;
		color_count_blue = 0;
		color_count_black = 0;
	} // when game restart
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
	Game.TurnOrder _GetTurnOrder(){ return game.turnOrder;}
	int _GetHandCount(Game.Player tar){ return game.getHandCount (tar);}
	int _GetNextPlayerInt(){ return game.nextPlayerInt ();}
	Game.Player _GetNextPlayer(){ return game.nextPlayer ();}
	Card.CardColor maxCardColor(){
		if (color_count_red > color_count_yellow) {
			if (color_count_red > color_count_blue) {
				if (color_count_red > color_count_green) {
					return global::Card.CardColor.Red;
				} else
					return global::Card.CardColor.Green;
			} else {
				if (color_count_blue > color_count_green) {
					return global::Card.CardColor.Blue;
				} else
					return global::Card.CardColor.Green;
			}
		} else {
			if (color_count_yellow > color_count_blue) {
				if (color_count_yellow > color_count_green) {
					return global::Card.CardColor.Yellow;
				} else
					return global::Card.CardColor.Green;
			} else {
				if (color_count_blue > color_count_green) {
					return global::Card.CardColor.Blue;
				} else
					return global::Card.CardColor.Green;
			}
		}
	}
	void reduceColorCount(Card tar){
		if (tar.color == global::Card.CardColor.Red)
			color_count_red--;
		else if(tar.color == global::Card.CardColor.Yellow)
			color_count_yellow--;
		else if(tar.color == global::Card.CardColor.Blue)
			color_count_blue--;
		else if(tar.color == global::Card.CardColor.Green)
			color_count_green--;
		else
			color_count_black--;
	}
}
