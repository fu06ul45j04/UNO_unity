using UnityEngine;
using System.Collections;

public class AI_origin : MonoBehaviour {
	Game game = GameObject.Find("GameManager").GetComponent<Game>();

	//-Functions-
	public Card Card(){return _NoCard ();} //pick a Card to Card
	public void getCard(Card newcard){} //recive a Card
	public Card.CardColor chooseColor(){return global::Card.CardColor.Blue;} //when you card wild
	public void restart(){} // when game restart
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
