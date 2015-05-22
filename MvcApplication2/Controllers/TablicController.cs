using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IgrajKarte.Enums;
using IgrajKarte.Models;
using IgrajKarte.Helpers;
using System.Data.Entity.Validation;
using System.Diagnostics;

namespace IgrajKarte.Controllers
{
    [Authorize]
    public class TablicController : Controller
    {
        private EntityModel DataContext;
        private int CurrentUserId;
        private Tablic tablic;

        public TablicController()
        {
            DataContext = new EntityModel();
            tablic = new Tablic();
        }
    
        [HttpGet]
        public ActionResult Index(string gameId)
        {
            if (HttpContext.Session["CurrentUserId"] != null)
            {
                CurrentUserId = (int)HttpContext.Session["CurrentUserId"];
                ViewBag.CurrentUserId = CurrentUserId;
            }
            if (CurrentUserId == -1)
            {
                return RedirectToAction("LogOn", "Account");
            }
            int id = Int32.Parse(gameId);
            tablic = DataContext.Tablics.Where<Tablic>(t => t.Id == id).SingleOrDefault();

            return RedirectToAction("StartGame2", new { id = id });
        }

        [HttpGet]
        public ActionResult StartGame2(int id)
        {
            CurrentUserId = (int)HttpContext.Session["CurrentUserId"];
            ViewBag.CurrentUserId = CurrentUserId;
            tablic = DataContext.Tablics.Where<Tablic>(t => t.Id == id).SingleOrDefault();
            int deck = CardDeck.AsList(tablic.CardsInDeck).Count;
            TablicGame tablicGame = new TablicGame();
            if (tablic.Status == (int)GameStatus.Created)
            {
                try
                {
                    tablicGame.Id = tablic.Id;
                    tablicGame.NOPlayers = 2;
                    tablicGame.ActivePlayer = tablic.ActiveUser;
                    tablicGame.CardsInDeck = tablic.CardsInDeck;
                    tablicGame.CardsOnTable = CardDeck.GetRandomCards(ref tablicGame.CardsInDeck, 4);
                    tablicGame.SelectedCards = tablic.SelectedCards;
                    tablicGame.Status = (int)GameStatus.InProgress;

                    Player player1 = new Player();
                    player1.Id = DataContext.Players.Where(t => true).Count();
                    player1.UserId = tablic.User1;
                    player1.CardsInHand = CardDeck.GetRandomCards(ref tablicGame.CardsInDeck, 6);
                    player1.CardsOnHeap = "";
                    player1.Dots = 0;
                    player1.Score = 0;
                    player1.GameType = (int)GameType.Tablic2;
                    player1.GameId = tablic.Id;
                    if (player1.UserId == tablic.ActiveUser)
                        player1.PlayerStatus = (int)PlayerStatus.Playing;
                    else
                        player1.PlayerStatus = (int)PlayerStatus.Waiting;


                    Player player2 = new Player();
                    player1.Id = (DataContext.Players.Where(t => true).Count() + 1);
                    player2.UserId = tablic.User2;
                    player2.CardsInHand = CardDeck.GetRandomCards(ref tablicGame.CardsInDeck, 6);
                    player1.CardsOnHeap = "";
                    player2.Dots = 0;
                    player2.Score = 0;
                    player2.GameType = (int)GameType.Tablic2;
                    player2.GameId = tablic.Id;
                    if (player2.UserId == tablic.ActiveUser)
                        player2.PlayerStatus = (int)PlayerStatus.Playing;
                    else
                        player2.PlayerStatus = (int)PlayerStatus.Waiting;

                    tablicGame.Player1 = player1;
                    tablicGame.Player2 = player2;
                    tablic.Cards = tablicGame.CardsOnTable;
                    tablic.SelectedCards = tablicGame.SelectedCards;
                    tablic.CardsInDeck = tablicGame.CardsInDeck;
                    tablic.Status = tablicGame.Status;

                    DataContext.Players.Add(player1);
                    DataContext.Players.Add(player2);
                    DataContext.SaveChanges();

                    Session["ActivePlayer"] = tablicGame.ActivePlayer;
                }
                catch (DbEntityValidationException dbEx)
                {
                    string errorMessage = "";
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Console.WriteLine(validationError.PropertyName);
                            errorMessage += validationError.PropertyName + " | " + validationError.ErrorMessage + "\t\t";
                            Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }
                    return Json(new { status = false, result = "Bad Initialization: " + errorMessage });
                }
            }
            else if(tablic.Status == (int)GameStatus.InProgress)
            {
                tablicGame.Id = tablic.Id;
                tablicGame.NOPlayers = 2;
                tablicGame.ActivePlayer = tablic.ActiveUser;
                tablicGame.CardsInDeck = tablic.CardsInDeck;
                tablicGame.CardsOnTable = tablic.Cards;
                tablicGame.SelectedCards = tablic.SelectedCards;
                
                Player player1 = DataContext.Players.Where(p => p.UserId == tablic.User1 && p.GameId == tablic.Id).FirstOrDefault();
               
                Player player2 = DataContext.Players.Where(p => p.UserId == tablic.User2 && p.GameId == tablic.Id).FirstOrDefault();
                
                tablicGame.Player1 = player1;
                tablicGame.Player2 = player2;

                Session["ActivePlayer"] = tablicGame.ActivePlayer;
            }
            else if (tablic.Status == (int)GameStatus.Ended || tablic.Status == (int)GameStatus.Resigned)
            { 
            
            }
            deck = CardDeck.AsList(tablic.CardsInDeck).Count;
            return View(tablicGame);
        }

        [HttpPost]
        public ActionResult CardSelected(string cardName, string parentElement, int gameId)
        {
            DataContext = new EntityModel();
            Tablic tablicGame = DataContext.Tablics.Where(t => t.Id == gameId).FirstOrDefault<Tablic>();
            List<string> selectedCards = CardDeck.AsList(tablicGame.SelectedCards);
            
            if (parentElement == "tableCards")
            {
                if (selectedCards.Contains(cardName))
                {
                    selectedCards.Remove(cardName);
                    tablicGame.SelectedCards = CardDeck.AsString(selectedCards);
                    DataContext.SaveChanges();
                    return Json(new { selected = false, result = "" });
                }
                else
                {
                    selectedCards.Add(cardName);
                    tablicGame.SelectedCards = CardDeck.AsString(selectedCards);
                    DataContext.SaveChanges();
                    return Json(new { selected = true, result = "" });
                }
            }
            else if (parentElement == "myCards")
            {
                int cu = (int)Session["CurrentUserId"];
                Player currentPlayer = DataContext.Players.Where(p => p.UserId == cu && p.GameId == gameId).FirstOrDefault();
                if (selectedCards.Count == 0)
                {
                    tablicGame.Cards = CardDeck.AddCards(tablicGame.Cards, new string[]{cardName});
                    currentPlayer.CardsInHand = CardDeck.RemoveCards(currentPlayer.CardsInHand, new string[] { cardName });
                    if (tablicGame.ActiveUser == tablicGame.User1)
                        tablicGame.ActiveUser = tablicGame.User2;
                    else
                        tablicGame.ActiveUser = tablicGame.User1;

                    Session["ActivePlayer"] = tablicGame.ActiveUser;
                    DataContext.SaveChanges();
                    return Json(new { status = true, action = "throw", ActiveUser = tablicGame.ActiveUser, tableCards = tablicGame.Cards });
                }
                else
                {
                    // do something
                    List<int> selectedCardsNumbers = CardDeck.GetCardsNumbers(tablicGame.SelectedCards);
                    int cardNumber = CardDeck.GetCardNumber(cardName);
                    if (CalculateResult(selectedCardsNumbers, cardNumber))
                    {
                        string cards = tablicGame.SelectedCards;
                        tablicGame.Cards = CardDeck.RemoveCards(tablicGame.Cards, cards.Trim().Trim(',').Split(','));
                        currentPlayer.CardsInHand = CardDeck.RemoveCards(currentPlayer.CardsInHand, new string[] { cardName });

                        currentPlayer.CardsOnHeap = CardDeck.AddCards(currentPlayer.CardsOnHeap, selectedCards.ToArray<string>());
                        currentPlayer.CardsOnHeap = CardDeck.AddCards(currentPlayer.CardsOnHeap, new string[] {cardName});
                        int points = CalculatePoints(currentPlayer.CardsOnHeap);
                        tablicGame.SelectedCards = "";
                        if (tablicGame.ActiveUser == tablicGame.User1)
                            tablicGame.ActiveUser = tablicGame.User2;
                        else
                            tablicGame.ActiveUser = tablicGame.User1;

                        Session["ActivePlayer"] = tablicGame.ActiveUser;
                        Session["LastTake"] = currentPlayer.Id;

                        DataContext.SaveChanges();
                        return Json(new { status = true, action = "take", selectedCards = cards, ActiveUser = tablicGame.ActiveUser, tableCards = tablicGame.Cards });
                    }
                    else
                    {
                        return Json(new { status = false, action = "take" });
                    }
                }
            }
            return Json(new { status = true, result = "" });
        }

        [HttpPost]
        public ActionResult MoveListener(string cardName, string parentElement, int gameId)
        {
            int cu = (int)Session["CurrentUserId"];

            DataContext = new EntityModel();
            Tablic tablicGame = DataContext.Tablics.Where(t => t.Id == gameId).FirstOrDefault<Tablic>();
            if (tablicGame.ActiveUser == cu)
                return Json(new { status = true, ActiveUser = tablicGame.ActiveUser, tableCards = tablicGame.Cards });
            else
                return Json(new { status = false });
                    
        }


        [HttpPost]
        public ActionResult InitializeNewGameLevel(int gameId)
        {
            DataContext = new EntityModel();
            Tablic tablicGame = DataContext.Tablics.Where(t => t.Id == gameId).FirstOrDefault<Tablic>();
            Player currentPlayer = DataContext.Players.Where(p => p.UserId == CurrentUserId && p.GameId == gameId).FirstOrDefault();
            Player opponentPlayer = DataContext.Players.Where(p => p.UserId != CurrentUserId && p.GameId == gameId).FirstOrDefault();

            List<string> currentPlayerCards = CardDeck.AsList(currentPlayer.CardsInHand);
            List<string> opponentPlayerCards = CardDeck.AsList(opponentPlayer.CardsInHand);
            string gameDeckCards = tablicGame.CardsInDeck;

            if ((currentPlayerCards.Count == 6 && opponentPlayerCards.Count == 6) || (currentPlayerCards.Count == 0 && opponentPlayerCards.Count == 0))
            {
                if (CardDeck.AsList(gameDeckCards).Count == 0)
                {
                    if (currentPlayer.Id == (int)Session["LastTake"])
                    {
                        currentPlayer.CardsOnHeap = CardDeck.AddCards(currentPlayer.CardsOnHeap, tablicGame.Cards.Trim().Trim(',').Split(','));
                        int points = CalculatePoints(currentPlayer.CardsOnHeap);
                    }
                    else
                    {
                        opponentPlayer.CardsOnHeap = CardDeck.AddCards(opponentPlayer.CardsOnHeap, tablicGame.Cards.Trim().Trim(',').Split(','));
                        int points = CalculatePoints(opponentPlayer.CardsOnHeap);    
                    }

                    // remove all cards on table
                    return Json(new { status = true, GameEnd = true });
                }
                else
                {
                    if ((currentPlayerCards.Count == 0 && opponentPlayerCards.Count == 0))
                    {
                        currentPlayer.CardsInHand = CardDeck.GetRandomCards(ref gameDeckCards, 6);
                        opponentPlayer.CardsInHand = CardDeck.GetRandomCards(ref gameDeckCards, 6);
                        tablicGame.CardsInDeck = gameDeckCards;
                        DataContext.SaveChanges();
                    }
            
                    return Json(new { status = true, GameEnd = false, cPCards = currentPlayer.CardsInHand, oPCards = opponentPlayer.CardsInHand, number = CardDeck.AsList(gameDeckCards).Count });
                }
            }

            return Json(new { status = false });
        }
   


        private bool CalculateResult(List<int> tableCards, int cardFromHend)
        {
            int sum = 0;
            int acesNumber = 0;
            foreach (int card in tableCards)
            {
                if (card == 1)
                    acesNumber++;
                else
                    sum += card;
            }
            switch (acesNumber)
            {
                case 0:
                    {
                        if (sum % cardFromHend == 0)
                            return true;
                        else
                            return false;
                    }
                case 1:
                    {
                        if ((sum + 1) % cardFromHend == 0 || (sum + 11) % cardFromHend == 0)
                            return true;
                        else
                            return false;
                    }
                case 2:
                    {
                        if ((sum + 2) % cardFromHend == 0 || (sum + 12) % cardFromHend == 0 || (sum + 22) % cardFromHend == 0)
                            return true;
                        else
                            return false;
                    }
                case 3:
                    {
                        if ((sum + 3) % cardFromHend == 0 || (sum + 13) % cardFromHend == 0 || (sum + 23) % cardFromHend == 0 || (sum + 33) % cardFromHend == 0)
                            return true;
                        else
                            return false;
                    }
                case 4:
                    {
                        if ((sum + 4) == cardFromHend || (sum + 14) == cardFromHend || (sum + 24) == cardFromHend || (sum + 34) == cardFromHend || (sum + 44) == cardFromHend)
                            return true;
                        else
                            return false;
                    }
                default: 
                    return false;
            }
        }

        private int CalculatePoints(string cards)
        {
            int sum = 0;
            List<string> cardsStrings = CardDeck.AsList(cards);
            List<int> cardNumbers = CardDeck.GetCardsNumbers(cards);

            foreach (int num in cardNumbers)
            {
                if (num >= 10 || num == 1)
                    sum++;
            }

            if (cards.Contains("K10"))
                sum++;
            if (cards.Contains("T2"))
                sum++;

            return sum;
        }
    }
}
