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
		if (hands.Exists (x => x.color == game.nowCard.color||x.function == game.nowCard.function||x.function==global::Card.CardFunction.Wild)) {
			List<Card> Z = new List<Card>();
			if(_GetNextDraw()!=0)
				Z = hands.FindAll(x=>x.function == _GetNowCard().function);
			else
				Z = hands.FindAll (x => x.color == _GetNowCard().color||x.function == _GetNowCard().function||x.function==global::Card.CardFunction.Wild);
			int index = 0;
			int card_point = 0;
			for(int i=0;i<Z.Count;i++){
				if(Z[i].color == global::Card.CardColor.Black){
					if(card_point ==0){
						card_point = 1;
						index = i;
					}
				}
				else if(Z[i].color != _GetNowCard().color){
					global::Card.CardColor GC = _GetNowCard().color;

					if(Z[i].color==global::Card.CardColor.Red){
						if(GC == global::Card.CardColor.Yellow){
							if(color_count_red > color_count_yellow){
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
			if(Z.Count == 0){
				if(hands.Exists (x => x.function == global::Card.CardFunction.WildDrawFour)){
					int Z2 = hands.FindIndex (x => x.function == global::Card.CardFunction.WildDrawFour);
					Card Tk2 = hands[Z2];
					hands.RemoveAt(Z2);
					return Tk2;
				}
				return _NoCard();
			}
			Card Tk = Z[index];
			int zi = hands.FindIndex(x=>x.function ==Tk.function & x.color == Tk.color);
			hands.RemoveAt(zi);
			return Tk; 
		} 
		else if(hands.Exists (x => x.function == global::Card.CardFunction.WildDrawFour)){
			int Z = hands.FindIndex (x => x.function == global::Card.CardFunction.WildDrawFour);
			Card Tk = hands[Z];
			hands.RemoveAt(Z);
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
		return global::Card.CardColor.Blue;
	} //when you card wild
	public void restart(){
		hands.Clear ();
		color_count_red =0;
		color_count_yellow =0;
		color_count_green =0;
		color_count_blue =0;
		color_count_black =0;
	
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
}
