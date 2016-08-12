using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AI_HeuristicV1 : MonoBehaviour {
	Game game ;
	List<Card> hands = new List<Card> ();
	int color_count_red =0;
	int color_count_yellow =0;
	int color_count_green =0;
	int color_count_blue =0;
	int color_count_black =0;
	//-Functions-
	void Start(){
		game = GameObject.Find ("GameManager").GetComponent<Game> ();

	}
	public Card Card(){
		if (hands.Count == 1 & color_count_black == 1)
			return _NoCard ();
		if (hands.Exists (x => x.color == game.nowCard.color||x.function == game.nowCard.function||x.function==global::Card.CardFunction.Wild)) {
			List<Card> Z = new List<Card>();
			if(_GetNextDraw()!=0)
				Z = hands.FindAll(x=>x.function == _GetNowCard().function);
			else
				Z = hands.FindAll (x => x.color == _GetNowCard().color||x.function == _GetNowCard().function||x.color == global::Card.CardColor.Black);
			int index = 0;
			int card_point = 0;
			for(int i=0;i<Z.Count;i++){
				if(Z[i].color == global::Card.CardColor.Black & _GetNextDraw()==0){
					if(hands.Count - color_count_black ==1 ){
						card_point = 3;
						index = i;
					} 
					else if(card_point ==0){
						card_point = 1;
						index = i;
					}
				}
				else if(Z[i].color != _GetNowCard().color & card_point != 3){
					global::Card.CardColor GC = _GetNowCard().color;

					if(Z[i].color==global::Card.CardColor.Red){
						if(GC == global::Card.CardColor.Yellow){
							if(color_count_red > color_count_yellow){
								card_point = 2;
								index = i;
								break;
							}
							else if(card_point<2){
								card_point = 2;
								index =i;
							}
						}
						else if(GC == global::Card.CardColor.Blue){
							if(color_count_red > color_count_blue){
								card_point = 2;
								index = i;
								break;
							}
							else if(card_point<2){
								card_point = 2;
								index =i;
							}
						}
						else if(GC == global::Card.CardColor.Green){
							if(color_count_red > color_count_green){
								card_point = 2;
								index = i;
								break;
							}
							else if(card_point<2){
								card_point = 2;
								index =i;
							}
						}
					}
					else if(Z[i].color==global::Card.CardColor.Yellow){
						if(GC == global::Card.CardColor.Red){
							if(color_count_yellow > color_count_red){
								card_point = 2;
								index = i;
								break;
							}
							else if(card_point<2){
								card_point = 2;
								index =i;
							}
						}
						else if(GC == global::Card.CardColor.Blue){
							if(color_count_yellow > color_count_blue){
								card_point = 2;
								index = i;
								break;
							}
							else if(card_point<2){
								card_point = 2;
								index =i;
							}
						}
						else if(GC == global::Card.CardColor.Green){
							if(color_count_yellow > color_count_green){
								card_point = 2;
								index = i;
								break;
							}
							else if(card_point<2){
								card_point = 2;
								index =i;
							}
						}
					}
					else if(Z[i].color==global::Card.CardColor.Green){
						if(GC == global::Card.CardColor.Yellow){
							if(color_count_green > color_count_yellow){
								card_point = 2;
								index = i;
								break;
							}
							else if(card_point<2){
								card_point = 2;
								index =i;
							}
						}
						else if(GC == global::Card.CardColor.Blue){
							if(color_count_green > color_count_blue){
								card_point = 2;
								index = i;
								break;
							}
							else if(card_point<2){
								card_point = 2;
								index =i;
							}
						}
						else if(GC == global::Card.CardColor.Red){
							if(color_count_green > color_count_red){
								card_point = 2;
								index = i;
								break;
							}
							else if(card_point<2){
								card_point = 2;
								index =i;
							}
						}
					}
					else if(Z[i].color==global::Card.CardColor.Blue){
						if(GC == global::Card.CardColor.Yellow){
							if(color_count_blue > color_count_yellow){
								card_point = 2;
								index = i;
								break;
							}
							else if(card_point<2){
								card_point = 2;
								index =i;
							}
						}
						else if(GC == global::Card.CardColor.Red){
							if(color_count_blue > color_count_red){
								card_point = 2;
								index = i;
								break;
							}
							else if(card_point<2){
								card_point = 2;
								index =i;
							}
						}
						else if(GC == global::Card.CardColor.Green){
							if(color_count_blue > color_count_green){
								card_point = 2;
								index = i;
								break;
							}
							else if(card_point<2){
								card_point = 2;
								index =i;
							}
						}
					}
				}
				else if(card_point<2){
					card_point = 2;
					index =i;
				}
			}
			if(Z.Count == 0 ){
				if( _GetNextDraw()==0 & hands.Exists (x => x.function == global::Card.CardFunction.WildDrawFour & hands.Count != 1)){
					Debug.Log("p");
					int Z2 = hands.FindIndex (x => x.function == global::Card.CardFunction.WildDrawFour);
					Card Tk2 = hands[Z2];
					hands.RemoveAt(Z2);
					decreaseColorCount(Tk2.color);
					return Tk2;
				}
				return _NoCard();
			}
			Card Tk = Z[index];
			int zi = hands.FindIndex(x=>x.function ==Tk.function & x.color == Tk.color);
			hands.RemoveAt(zi);
			decreaseColorCount(Tk.color);
			return Tk; 
		} 
		else if(_GetNextDraw()==0 & hands.Exists (x => x.function == global::Card.CardFunction.WildDrawFour)){
			int Z = hands.FindIndex (x => x.function == global::Card.CardFunction.WildDrawFour);
			Card Tk = hands[Z];
			hands.RemoveAt(Z);
			decreaseColorCount(Tk.color);
			return Tk;
		}
		else
			return _NoCard ();
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
	public Card.CardColor chooseColor(){
		int k = 0;
		int k1, k2;
		int c1, c2;
		if (color_count_red < color_count_blue) {
			k1 = 1;
			c1 = color_count_blue;
		}
		else{
			k1 = 0;
			c1 = color_count_red;
		}
		if (color_count_green < color_count_yellow){
			k2 = 3;
			c2 = color_count_yellow;
		}
		else{
			k2 = 2;
			c2 = color_count_green;
		}
		if( c1 < c2 ){
			k = k2;
		}
		else
			k = k1;
		return (k==0)?global::Card.CardColor.Red:
				(k==1)?global::Card.CardColor.Blue:
				(k==2)?global::Card.CardColor.Green:
					global::Card.CardColor.Yellow;
	} //when you card wild
	public void restart(){
		hands.Clear ();
		color_count_red =0;
		color_count_yellow =0;
		color_count_green =0;
		color_count_blue =0;
		color_count_black =0;
	
	} // when game restart

	public void decreaseColorCount(Card.CardColor color){
		if (color == global::Card.CardColor.Red)
			color_count_red--;
		else if (color == global::Card.CardColor.Yellow)
			color_count_yellow--;
		else if (color == global::Card.CardColor.Green)
			color_count_green--;
		else if (color == global::Card.CardColor.Blue)
			color_count_blue--;
		else if (color == global::Card.CardColor.Black)
			color_count_black--;
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
