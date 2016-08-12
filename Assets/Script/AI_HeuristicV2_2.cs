using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class AI_HeuristicV2_2 : MonoBehaviour {
	Game game;
	List<Card> hands = new List<Card>();
	int color_count_red =0;
	int color_count_yellow =0;
	int color_count_green =0;
	int color_count_blue =0;
	int color_count_black =0;
	int dis_color_count_red =0;
	int dis_color_count_yellow =0;
	int dis_color_count_green =0;
	int dis_color_count_blue =0;
	int dis_color_count_black =0;
	int discordCount = 0;
	int deckCount = 80;
	int[,] dis_function_count = new int[4,17];
	int[,] player_no_color = new int[4, 4];
	//Card nowcard;
	Game.Player nowPlayer;
	Card[] lastcard = new Card[4];
	//-Functions-
	void Start(){
		game = GameObject.Find ("GameManager").GetComponent<Game> ();
		nowPlayer = _GetNowPlayer();
		deckCount = 79;
	}
	void Update(){
		if ( nowPlayer != _GetNowPlayer () ) {
			//Debug.Log("record");
			recordCard();
			nowPlayer = _GetNowPlayer();
		}
		if ( deckCount != game.getDeckCount ()) {
			if(deckCount == game.getDeckCount()+1){
				noColor();
			}
			else{
				haveColor();
			}
			deckCount = game.getDeckCount ();
		}
	}
	public void recordCard(){
		if (game.deckcount() == discordCount)
			return;
		else if (game.deckcount() == 0)
			clearRecordDiscord ();
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
		recordDiscord (_GetNowCard ());
	}
	public void noColor(){
		player_no_color [_GetNowPlayer () - Game.Player.player1, _GetNowCard ().color - global::Card.CardColor.Red] = 1;
		//Debug.Log ((_GetNowPlayer () - Game.Player.player1)+" "+( _GetNowCard ().color - global::Card.CardColor.Red));
		/*for (int i=0; i<4; i++)
			Debug.Log (player_no_color [i, 0] + "," + player_no_color [i, 1] + "," + player_no_color [i, 2] + "," + player_no_color [i, 3]);
	*/}
	public void haveColor(){
		for (int i=0; i<4; i++)
			player_no_color [game.prevPlayer() - Game.Player.player1, i] = 0;
		/*for (int i=0; i<4; i++)
			Debug.Log (player_no_color [i, 0] + "," + player_no_color [i, 1] + "," + player_no_color [i, 2] + "," + player_no_color [i, 3]);
*/
	}
	public Card Card(){
		Card maxPriorityCard = _NoCard() ;
		float maxPriority = 0;
		float[] cardPriority = new float[hands.Count];
		for (int i=0; i<hands.Count; i++) {
			
			//Debug.Log(hands.Count);
			//Debug.Log(hands[i].color.ToString() +  hands[i].function + " f");
			if(!_isLegal(hands[i])){
				cardPriority[i] = 0;
				//Debug.Log(hands[i].color.ToString() +  hands[i].function + " notlegal");
				continue;
			}
			cardPriority[i] = search(hands, hands[i]);
			if(_GetHandCount(_GetNextPlayer())<=2){
				if ((lastcard[_GetNextPlayerInt()].color == hands[i].color) & (hands[i].function != global::Card.CardFunction.DrawTwo)
				    & (hands[i].function != global::Card.CardFunction.WildDrawFour)
				    & (hands[i].function != global::Card.CardFunction.Skip))
				{
					cardPriority[i] -= 10;
				}
				else if((_GetNowCard().color == hands[i].color) & (hands[i].function == global::Card.CardFunction.DrawTwo)){
					cardPriority[i] += 4;                                                                          
				}
				else if((_GetNowCard().color == hands[i].color) & _GetNextDraw() == 0 & ((hands[i].function == global::Card.CardFunction.DrawTwo)
				                                                                         | (hands[i].function == global::Card.CardFunction.WildDrawFour)
				                                                                         | (hands[i].function == global::Card.CardFunction.Skip)
				                                                                         | (hands[i].function == global::Card.CardFunction.Reverse))){
					cardPriority[i] += 2;                                                                          
				}
			}
			if (cardPriority[i] > maxPriority){
				maxPriorityCard = hands[i];
				maxPriority = cardPriority[i];
			}
		}
		for (int i=0; i<hands.Count; i++) {
			Debug.Log(hands[i].color.ToString() +  hands[i].function + " : pri = " + cardPriority[i]);
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
	float search(List<Card> sHands, Card sCard){
		float total = 0;
		int index = sHands.FindIndex(x=>x.function ==sCard.function & x.color == sCard.color);
		Card temp = sHands [index];
		sHands.RemoveAt(index);
		reduceColorCount(sCard);
		Card sNowcard = new global::Card ();
		float nocolorcount;
		for (int i=0; i<4; i++) {
			nocolorcount =0;
			for(int j=0;j<4;j++){
				if(j==_GetNowPlayer()-Game.Player.player1)
					continue;
				if(player_no_color[j,i] == 1)
					nocolorcount++;
			}
			for(int j=0;j<15;j++){
				sNowcard.color = global::Card.CardColor.Red + i ;
				sNowcard.function = global::Card.CardFunction.zero + j ;
				int x = (j==0)?2:4;
				float ca = cal (sHands, sNowcard);
				if(x-dis_function_count[i,j]!=0)
				total += ca*((float)(x-dis_function_count[i,j])/(108-(float)game.deckcount()-1))*(float)((3-nocolorcount)/3);
				//Debug.Log(ca+","+((float)(x-dis_function_count[i,j])/(108-(float)game.deckcount()-1))+","+(float)((3-nocolorcount)/3)+","+total);
			}
		}
		/*
		sNowcard.color = global::Card.CardColor.Red;
		total += cal (sHands, sNowcard)*((25-(float)dis_color_count_red)/(108-(float)game.deckcount()-1));

		//Debug.Log(((25-(float)dis_color_count_red)/(108-(float)game.deckcount()-1)));
		Debug.Log("total =" + total);
		sNowcard.color = global::Card.CardColor.Blue;
		for (int i=0; i<15; i++) {
			sNowcard.function = global::Card.CardFunction.zero+i;

		}
		total += cal (sHands, sNowcard)*((25-(float)dis_color_count_blue)/(108-(float)game.deckcount()-1));
		Debug.Log("total =" + total);
		sNowcard.color = global::Card.CardColor.Green;
		total += cal (sHands, sNowcard)*((25-(float)dis_color_count_green)/(108-(float)game.deckcount()-1));
		Debug.Log("total =" + total);
		sNowcard.color = global::Card.CardColor.Yellow;
		total += cal (sHands, sNowcard)*((25-(float)dis_color_count_yellow)/(108-(float)game.deckcount()-1));
		Debug.Log("total =" + total);*/

		sHands.Insert (index, temp);
		increaseColorCount (sCard);
		return total;
	}
	int cal(List<Card> cHands,Card cNowcard){
		int[] priority = new int[cHands.Count];
		int maxPriority =0;
		for (int i=0; i<cHands.Count; i++) {
		
			if (cHands[i].color == cNowcard.color /*& _GetNextDraw() == 0*/) {
				priority[i] += 40;
			}
			else if (hands[i].function == _GetNowCard().function) {
				priority[i] += 10;
				if (hands[i].color == maxCardColor())
					priority[i] += 10;
			}
			else if (cHands[i].color == global::Card.CardColor.Black /*& _GetNextDraw() == 0*/) {
				if (cHands.Count - color_count_black == 1)
					priority [i] += 60;
				else if (cHands.Count == 1)
					priority [i] = 0;
				else 
					priority[i] += 50;
			}
			if(cHands[i].function == global::Card.CardFunction.DrawTwo)
				priority[i] += 10;
			if (priority[i] > maxPriority){
				maxPriority = priority[i];
			}
		}/*
		for (int i=0; i<hands.Count; i++) {
			Debug.Log(hands[i].color.ToString() +  hands[i].function + " : pri = " + cardPriority[i]);
		}*/
		for (int i=0; i<cHands.Count; i++) {
			for(int j=i; j<cHands.Count;j++){
				if(cHands[i].function == cHands[j].function && cHands[i].color!=global::Card.CardColor.Black)
					if(maxPriority>5)
						maxPriority-=1;
			}
		}
		if (cHands.Count == 0)
			return 100;
		else
			return maxPriority;

	
	}
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
	void recordDiscord(Card newcard){
		if (newcard.color == global::Card.CardColor.Red)
			dis_color_count_red++;
		else if (newcard.color == global::Card.CardColor.Yellow)
			dis_color_count_yellow++;
		else if (newcard.color == global::Card.CardColor.Green)
			dis_color_count_green++;
		else if (newcard.color == global::Card.CardColor.Blue)
			dis_color_count_blue++;
		else if (newcard.color == global::Card.CardColor.Black)
			dis_color_count_black++;
		dis_function_count [newcard.color - global::Card.CardColor.Red,newcard.function - global::Card.CardFunction.zero]++;
		/*for(int i=0; i<4; i++)
			Debug.Log (dis_function_count[i,0]+","+dis_function_count[i,1]+","+dis_function_count[i,2]+","+dis_function_count[i,3]+","+dis_function_count[i,4]+","+dis_function_count[i,5]+","+dis_function_count[i,6]+","+dis_function_count[i,7]+","+dis_function_count[i,8]+","+dis_function_count[i,9]+","+dis_function_count[i,10]+","+dis_function_count[i,11]+","+dis_function_count[i,12]+","+dis_function_count[i,13]+","+dis_function_count[i,14]);
		*///Debug.Log (dis_color_count_red + " " + dis_color_count_yellow + " " + dis_color_count_green + " " + dis_color_count_blue + " " + dis_color_count_black);
	}
	void clearRecordDiscord(){
		dis_color_count_red = 0;
		dis_color_count_yellow = 0;
		dis_color_count_green = 0;
		dis_color_count_blue = 0;
		dis_color_count_black = 0;
		for(int i=0; i<4; i++)
			for (int j=0; j<17; j++)
				dis_function_count [i,j] = 0;
		
	}
	public Card.CardColor chooseColor(){return maxCardColor();} //when you card wild
	public void restart(){
		hands.Clear ();
		color_count_red = 0;
		color_count_yellow = 0;
		color_count_green = 0;
		color_count_blue = 0;
		color_count_black = 0;
		dis_color_count_red = 0;
		dis_color_count_yellow = 0;
		dis_color_count_green = 0;
		dis_color_count_blue = 0;
		dis_color_count_black = 0;
		for(int i=0; i<4; i++)
			for (int j=0; j<17; j++)
				dis_function_count [i,j] = 0;
		deckCount = 79;
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
	bool _isLegal(Card card){
		//Debug.Log(card.color.ToString() +  card.function + " g");
		if ((card.color != _GetNowCard ().color) & (card.function != _GetNowCard ().function)) {
			
			//Debug.Log(card.color.ToString() +  card.function + " h");

			if (card.color != global::Card.CardColor.Black)
				return false;
			else if(_GetNextDraw() != 0)
				return false;
			else
				return true;
		} else if (_GetNextDraw() != 0 & card.function != global::Card.CardFunction.DrawTwo) {
			return false;
		}
		else
			return true;
	}
	void increaseColorCount(Card tar){
		if (tar.color == global::Card.CardColor.Red)
			color_count_red++;
		else if(tar.color == global::Card.CardColor.Yellow)
			color_count_yellow++;
		else if(tar.color == global::Card.CardColor.Blue)
			color_count_blue++;
		else if(tar.color == global::Card.CardColor.Green)
			color_count_green++;
		else
			color_count_black++;
	}
}