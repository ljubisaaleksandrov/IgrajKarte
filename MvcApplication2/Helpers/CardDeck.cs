using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IgrajKarte.Enums;

namespace IgrajKarte.Helpers
{
    public static class CardDeck
    {
        public static string InitializeDeck(GameType gameType)
        {
            string deck = "";
            switch (gameType)
            {
                case GameType.Tablic2:
                case GameType.Tablic4:
                {
                    foreach (string name in Enum.GetNames(typeof(Card)))
                    {
                        if(!name.Contains("15"))
                            deck += name + ",";
                    }
                    break;
                }
                case GameType.Magarac:
                {
                    foreach (string name in Enum.GetNames(typeof(Card)))
                    {
                        if ((name.Contains("1") && !name.Contains("10")) || name.Contains("T2"))
                            deck += name + ",";
                    }
                    break;
                }
                default:
                {
                    foreach (string name in Enum.GetNames(typeof(Card)))
                    {
                        deck += name + ",";
                    }
                    break;
                }
            }

            return deck;
        }

        public static string RemoveCards(string deck, string[] cards)
        {
            foreach (string card in cards)
            {
                int cardLocation = deck.IndexOf(card); // because of ',' character
                int deckl = deck.Length;
                if(cardLocation >= 0)
                    deck = deck.Remove(cardLocation, card.Length + 1);
            }

            return deck;
        }

        public static string AddCards(string deck, string[] cards)
        {
            foreach (string card in cards)
            {
                if (card != "")
                {
                    int cardLocation = deck.IndexOf(card);
                    if (cardLocation < 0)
                        deck += card + ",";
                }
            }

            return deck;
        }

        public static string GetRandomCards(ref string deck, int cardsNumber)
        {
            List<string> cardsList = AsList(deck);
            if (cardsList.Count < cardsNumber)
                return (string)null;
            else
            {
                string cards = "";
                Random random = new Random();
                for (int i = 0; i < cardsNumber; i++)
                {
                    int cardindex = random.Next(cardsList.Count - 1);
                    cards += cardsList[cardindex] + ",";
                    cardsList.RemoveAt(cardindex);
                }

                deck = AsString(cardsList);

                return cards;
            }
        }

        public static List<string> AsList(string cards)
        {
            List<string> cardsList = cards.Trim().Trim(',').Split(',').ToList<string>();
            if (cardsList.Contains(""))
                cardsList.Remove("");
            return cardsList;
        }

        public static string AsString(List<string> cardsList)
        {
            string cards = "";
            foreach (string s in cardsList)
                cards += s + ",";

            return cards;
        }

        public static int GetCardNumber(string card)
        {
            if (card.Length == 3)
                return Int32.Parse(card.Substring(1, 2));
            else
                return Int32.Parse(card.Substring(1, 1));
        }

        public static List<int> GetCardsNumbers(string cards)
        {
            List<int> cardsList = new List<int>();
            foreach (string card in AsList(cards))
            {
                if (card.Length == 3)
                    cardsList.Add(Int32.Parse(card.Substring(1, 2)));
                else
                    cardsList.Add(Int32.Parse(card.Substring(1, 1)));
            }

            return cardsList;
        }

    }
}