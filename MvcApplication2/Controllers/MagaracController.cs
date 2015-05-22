using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IgrajKarte;
using IgrajKarte.Enums;
using IgrajKarte.Helpers;
using IgrajKarte.Models;

namespace IgrajKarte.Controllers
{
    public class MagaracController : Controller
    {
        private EntityModel DataContext;
        private int CurrentUserId;
        private Magarac magarac;
        private List<int> playersIDs;

        public MagaracController()
        {
            DataContext = new EntityModel();
            magarac = new Magarac();
            playersIDs = new List<int>();
        }

                /// <summary>
        /// Checks whether the current user is able to continue game or not
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns>-1 - user is not autorised</returns>
        /// <returns>0 - user is not one of the game players, so he cannot view the other players games</returns>
        /// <returns>1 - user is able to continue this game</returns>
        private int CheckCurrentUser(int gameId)
        {
            if (HttpContext.Session["CurrentUserId"] != null)
            {
                CurrentUserId = (int)HttpContext.Session["CurrentUserId"];
                ViewBag.CurrentUserId = CurrentUserId;
            }
            else
            {
                return -1;
            }
            
            if (CurrentUserId == -1)
            {
                return -1;
            }



            DataContext = new EntityModel();
            IEnumerable<Magarac> magarac = DataContext.Magaracs.Where<Magarac>(t => (t.User1 == CurrentUserId || t.User2 == CurrentUserId ||
                                                                  t.User3 == CurrentUserId || t.User4 == CurrentUserId) && t.Id == gameId).AsEnumerable();
            if (magarac.Count() != 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        [HttpGet]
        public ActionResult Index(string gameId)
        {
            int id = Int32.Parse(gameId);
            int check = CheckCurrentUser(id);
            if(check == -1)
                return RedirectToAction("LogOn", "Account");
            else if (check == 0)
                return RedirectToAction("Index", "DashBoard");

            magarac = DataContext.Magaracs.Where<Magarac>(t => t.Id == id).SingleOrDefault();
            playersIDs.Add(magarac.User1);
            playersIDs.Add(magarac.User2);
            playersIDs.Add(magarac.User3);
            playersIDs.Add(magarac.User4);
     
            return RedirectToAction("StartGame", new { id = id });
        }


        [HttpGet]
        public ActionResult StartGame(int id)
        {
            int check = CheckCurrentUser(id);
            if(check == -1)
                return RedirectToAction("LogOn", "Account");
            else if (check == 0)
                return RedirectToAction("Index", "DashBoard");

            DataContext = new EntityModel();
            ViewBag.CurrentUserId = CurrentUserId;
            magarac = DataContext.Magaracs.Where<Magarac>(t => t.Id == id).ToList<Magarac>().First();
            MagaracGame magaracGame = new MagaracGame();
           
            if (magarac.Status == (int) GameStatus.Created)
            {
                try
                {
                    magaracGame.Id = magarac.Id;
                    magaracGame.ActivePlayer = magarac.ActiveUser;
                    magaracGame.CardsInDeck = CardDeck.RemoveCards(magarac.CardsInDeck, new string[] { "T2" });
                    magaracGame.Status = (int)GameStatus.InProgress;
                    magaracGame.t2Counter = magarac.T2Counter;

                    Player player1 = new Player();
                    player1.Id = DataContext.Players.Where(t => true).Count();
                    player1.UserId = magarac.User1;
                    player1.CardsInHand = CardDeck.GetRandomCards(ref magaracGame.CardsInDeck, 4);
                    player1.CardsOnHeap = "";
                    player1.Dots = 0;
                    player1.Score = 0;
                    player1.GameType = (int)GameType.Magarac;
                    player1.GameId = magarac.Id;
                    if (player1.UserId == magarac.ActiveUser)
                    {
                        player1.PlayerStatus = (int)PlayerStatus.Playing;
                        player1.CardsInHand += "T2,";
                    }
                    else
                        player1.PlayerStatus = (int)PlayerStatus.Waiting;


                    Player player2 = new Player();
                    player2.Id = (DataContext.Players.Where(t => true).Count() + 1);
                    player2.UserId = magarac.User2;
                    player2.CardsInHand = CardDeck.GetRandomCards(ref magaracGame.CardsInDeck, 4);
                    player2.CardsOnHeap = "";
                    player2.Dots = 0;
                    player2.Score = 0;
                    player2.GameType = (int)GameType.Magarac;
                    player2.GameId = magarac.Id;
                    if (player2.UserId == magarac.ActiveUser)
                    {
                        player2.PlayerStatus = (int)PlayerStatus.Playing;
                        player2.CardsInHand += "T2,";
                    }
                    else
                        player2.PlayerStatus = (int)PlayerStatus.Waiting;

                    Player player3 = new Player();
                    player3.Id = DataContext.Players.Where(t => true).Count() + 2;
                    player3.UserId = magarac.User3;
                    player3.CardsInHand = CardDeck.GetRandomCards(ref magaracGame.CardsInDeck, 4);
                    player3.CardsOnHeap = "";
                    player3.Dots = 0;
                    player3.Score = 0;
                    player3.GameType = (int)GameType.Magarac;
                    player3.GameId = magarac.Id;
                    if (player3.UserId == magarac.ActiveUser)
                    {
                        player3.PlayerStatus = (int)PlayerStatus.Playing;
                        player3.CardsInHand += "T2,";
                    }
                    else
                        player3.PlayerStatus = (int)PlayerStatus.Waiting;


                    Player player4 = new Player();
                    player4.Id = (DataContext.Players.Where(t => true).Count() + 3);
                    player4.UserId = magarac.User4;
                    player4.CardsInHand = CardDeck.GetRandomCards(ref magaracGame.CardsInDeck, 4);
                    player4.CardsOnHeap = "";
                    player4.Dots = 0;
                    player4.Score = 0;
                    player4.GameType = (int)GameType.Magarac;
                    player4.GameId = magarac.Id;
                    if (player4.UserId == magarac.ActiveUser)
                    {
                        player4.PlayerStatus = (int)PlayerStatus.Playing;
                        player4.CardsInHand += "T2,";
                    }
                    else
                        player4.PlayerStatus = (int)PlayerStatus.Waiting;

                    magaracGame.Player1 = player1;
                    magaracGame.Player2 = player2;
                    magaracGame.Player3 = player3;
                    magaracGame.Player4 = player4;
                    magarac.CardsInDeck = magaracGame.CardsInDeck;
                    magarac.Status = magaracGame.Status;

                    DataContext.Players.Add(player1);
                    DataContext.Players.Add(player2);
                    DataContext.SaveChanges();

                    Session["ActivePlayer"] = magaracGame.ActivePlayer;
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
            else if(magarac.Status == (int)GameStatus.InProgress)
            {
                magaracGame.Id = magarac.Id;
                magaracGame.ActivePlayer = magarac.ActiveUser;
                magaracGame.CardsInDeck = magarac.CardsInDeck;
                magaracGame.t2Counter = magarac.T2Counter;

                Player player1 = DataContext.Players.Where(p => p.UserId == magarac.User1 && p.GameId == magaracGame.Id).FirstOrDefault();
                Player player2 = DataContext.Players.Where(p => p.UserId == magarac.User2 && p.GameId == magaracGame.Id).FirstOrDefault();
                Player player3 = DataContext.Players.Where(p => p.UserId == magarac.User3 && p.GameId == magaracGame.Id).FirstOrDefault();
                Player player4 = DataContext.Players.Where(p => p.UserId == magarac.User4 && p.GameId == magaracGame.Id).FirstOrDefault();

                InitializePlayerIDsList(magarac);

                magaracGame.Player1 = player1;
                magaracGame.Player2 = player2;
                magaracGame.Player3 = player3;
                magaracGame.Player4 = player4;

                Session["ActivePlayer"] = magaracGame.ActivePlayer;
            }
            else if (magarac.Status == (int)GameStatus.Ended || magarac.Status == (int)GameStatus.Resigned)
            { 
            
            }
            return View(magaracGame);
        
        }

        [HttpPost]
        public ActionResult CardSelected(string cardName, int gameId)
        {
            DataContext = new EntityModel();
            Magarac magaracGame = DataContext.Magaracs.Where(t => t.Id == gameId).FirstOrDefault<Magarac>();
            InitializePlayerIDsList(magaracGame);
            try{
            int currentUserId = (int)Session["CurrentUserId"];
            int currentPlayerIndex = playersIDs.IndexOf(currentUserId);
            int nextPlayerIndex = (currentPlayerIndex + 1) % 3;
            int nextUserId = playersIDs[nextPlayerIndex];

            Player currentPlayer = DataContext.Players.Where(p => p.UserId == currentUserId && p.GameId == gameId).FirstOrDefault();
            Player nextPlayer = DataContext.Players.Where(p => p.UserId == nextUserId && p.GameId == gameId).FirstOrDefault();

            if (cardName == "T2")
            {
                if (magaracGame.T2Counter >= 3)
                {

                }
                else
                {
                    return Json(new { status = true, action = "take", ActiveUser = magaracGame.ActiveUser });                    
                }
            }
            else
            {
                currentPlayer.CardsInHand = CardDeck.RemoveCards(currentPlayer.CardsInHand.Trim(), new string[] { cardName });
                nextPlayer.CardsInHand = CardDeck.AddCards(nextPlayer.CardsInHand.Trim(), new string[] { cardName });
                magaracGame.ActiveUser = nextPlayer.Id;
                if (currentPlayer.CardsInHand.Contains("T2"))
                {
                    magaracGame.T2Counter++;
                }

                DataContext.SaveChanges();
                return Json(new { status = true, next = nextPlayer.Id, nextCards = nextPlayer.CardsInHand });
            }
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
            
            
             
            //return Json(new { status = true, action = "take", selectedCards = cards, ActiveUser = magaracGame.ActiveUser, tableCards = magaracGame.Cards });
            return Json(new { status = true, result = "" });
        }




        private void InitializePlayerIDsList(Magarac game)
        {
            playersIDs.Clear();
            playersIDs.Add(game.User1);
            playersIDs.Add(game.User2);
            playersIDs.Add(game.User3);
            playersIDs.Add(game.User4);
        }
    }
}