using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IgrajKarte.Helpers;
using IgrajKarte.Enums;
using System.Data.Entity.Validation;
using System.Diagnostics;

namespace IgrajKarte.Controllers
{
    [Authorize]
    public class DashBoardController : Controller
    {
        //
        // GET: /DashBoard/

        public EntityModel DataContext;
        public int CurrentUserId;

        public DashBoardController()
        {
            DataContext = new EntityModel();
        }

        public ActionResult Index()
        {
            List<Room> rooms = DataContext.Rooms.ToList<Room>();
            ViewBag.Rooms = rooms;
            string[] s = rooms[0].PlayerIDIn.Trim().Split(',');
            return View();
        }

        [HttpPost]
        public ActionResult CheckAvailablePlayersForTablic2(int gameCode)
        { 
            Room room = DataContext.Rooms.Where<Room>(r => r.GameId == gameCode).FirstOrDefault();
            if (HttpContext.Session["CurrentUserId"] != null)
            {
                CurrentUserId = (int)HttpContext.Session["CurrentUserId"];
            }
            if (CurrentUserId == -1)
            {
                return RedirectToAction("LogOn", "Account");
            }
            
            if(room.PlayerIDIn.Contains(CurrentUserId.ToString()))
            {
                DataContext = new EntityModel();
                Tablic tablic = DataContext.Tablics.Where<Tablic>(t => (t.User1 == CurrentUserId || t.User2 == CurrentUserId) && t.Status == (int)GameStatus.Created).LastOrDefault();
                return Json(new { status = true, result = "Lets play", gameId = tablic.Id });

            }

            List<int> PlayersOut = Utils.GetPlayersOutIds(room);
            if (PlayersOut.Count > 0)
            {
                // (PlayersOut.Contains(CurrentUserId) && PlayersOut.Count > 1)
                if (!PlayersOut.Contains(CurrentUserId))
                    return StartTablic2(gameCode, room);
                else
                    return Json(new { status = false, result = "There are no available players at the moment" });
            }
            else
            {
                if(!PlayersOut.Contains(CurrentUserId))
                {
                    room.PlayerIDOut = CurrentUserId.ToString() + ",";
                    DataContext.SaveChanges();
                    List<string> playersIn = room.PlayerIDIn.Trim().Trim(',').Split(',').ToList();
                }
             
                return Json(new { status = false, result = "There are no available players at the moment" });
            }
        }

        [HttpPost]
        public ActionResult StartTablic2(int gameCode, Room room)
        {
            try
            {
                room.PlayerIDIn = room.PlayerIDIn.Trim();
                List<int> PlayersOut = Utils.GetPlayersOutIds(room);
                int firstPlayerOnWait = PlayersOut[0];
                string remainingPlayers = "";
                for (int i = 1; i < PlayersOut.Count(); i++)
                    remainingPlayers += PlayersOut[i] + ",";

                room.PlayerIDIn += CurrentUserId.ToString() + "," + firstPlayerOnWait.ToString() + ",";
                room.PlayerIDOut = remainingPlayers;
                DataContext.SaveChanges();

                Tablic newGame = new Tablic();
                DataContext = new EntityModel();
                newGame.Id = DataContext.Tablics.Count();
                newGame.CardsInDeck = CardDeck.InitializeDeck(GameType.Tablic2);
                newGame.Status = (int)GameStatus.Created;
                newGame.Cards = "";
                newGame.SelectedCards = "";
                newGame.NOPlayers = 2;
                newGame.User1 = CurrentUserId;
                newGame.User2 = firstPlayerOnWait;
                newGame.User3 = -1;
                newGame.User4 = -1;
                newGame.ActiveUser = CurrentUserId;
                DataContext.Tablics.Add(newGame);
                DataContext.SaveChanges();

                return Json(new { status = true, result = "Lets play", gameId = newGame.Id});
            }
            catch (DbEntityValidationException dbEx)
            {
                string errorMessage = "";
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Console.WriteLine(validationError.PropertyName);
                        errorMessage += validationError.PropertyName + " | ";
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
                return Json(new { status = false, result = "Bad Initialization: " + errorMessage});
            }
        }



        [HttpPost]
        public ActionResult CheckAvailablePlayersForMagarac(int gameCode)
        {
            DataContext = new EntityModel();
            Room room = DataContext.Rooms.Where<Room>(r => r.GameId == gameCode).FirstOrDefault();
            if (HttpContext.Session["CurrentUserId"] != null)
            {
                CurrentUserId = (int)HttpContext.Session["CurrentUserId"];
            }
            if (CurrentUserId == -1)
            {
                return RedirectToAction("LogOn", "Account");
            }

            if (room.PlayerIDIn.Contains(CurrentUserId.ToString()))
            {
                DataContext = new EntityModel();
                IEnumerable<Magarac> magarac = DataContext.Magaracs.Where<Magarac>(t => (t.User1 == CurrentUserId || t.User2 == CurrentUserId ||
                                                                      t.User3 == CurrentUserId || t.User4 == CurrentUserId) && t.Status == (int)GameStatus.Created).AsEnumerable();
                if (magarac.Count() != 0)
                {
                    return Json(new {status = true, result = "Lets play", gameId = magarac.LastOrDefault().Id});
                }
            }

            List<int> playersOut = Utils.GetPlayersOutIds(room);
            List<int> playersIn = Utils.GetPlayersInIds(room);
            if ((playersOut.Count > 2 && !playersOut.Contains(CurrentUserId)) || playersOut.Count > 3)
            {
                if (!playersOut.Contains(CurrentUserId))
                {
                    return StartMagarac(gameCode, room.GameId);
                }
                else
                {
                    return Json(new { status = false, result = "There are no available players at the moment" });
                }
            }
            else
            {
                if (!playersOut.Contains(CurrentUserId) && !playersIn.Contains(CurrentUserId))
                {
                    playersOut.Add(CurrentUserId);
                    room.PlayerIDOut = Utils.SetPlayersIds(playersOut);
                    DataContext.SaveChanges();
                }

                return Json(new { status = false, result = "There are no available players at the moment" });
            }
        }

        [HttpPost]
        public ActionResult StartMagarac(int gameCode, int roomId)
        {
            try
            {
                DataContext = new EntityModel();
                Room room = DataContext.Rooms.FirstOrDefault(r => r.GameId == roomId);
                List<int> playersOut = Utils.GetPlayersOutIds(room);
                List<int> playersIn = Utils.GetPlayersInIds(room);
                
                List<int> players = playersOut.Take(3).ToList();
                foreach (var id in players)
                {
                    playersOut.Remove(id);
                    playersIn.Add(id);
                }
                
                players.Add(CurrentUserId);
                playersIn.Add(CurrentUserId);
                players.Sort();

                room.PlayerIDIn = Utils.SetPlayersIds(playersIn);
                room.PlayerIDOut = Utils.SetPlayersIds(playersOut);
                DataContext.SaveChanges();

                Magarac newGame = new Magarac();
                DataContext = new EntityModel();
                newGame.Id = DataContext.Magaracs.Count();
                newGame.CardsInDeck = CardDeck.InitializeDeck(GameType.Magarac);
                newGame.Status = (int)GameStatus.Created;
                newGame.User1 = players[0];
                newGame.User2 = players[1];
                newGame.User3 = players[2];
                newGame.User4 = players[3];
                newGame.ActiveUser = players[0];
                newGame.T2Counter = 0;
                DataContext.Magaracs.Add(newGame);
                DataContext.SaveChanges();

                return Json(new { status = true, result = "Lets play", gameId = newGame.Id });
            }
            catch (DbEntityValidationException dbEx)
            {
                string errorMessage = "";
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Console.WriteLine(validationError.PropertyName);
                        errorMessage += validationError.PropertyName + " | ";
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
                return Json(new { status = false, result = "Bad Initialization: " + errorMessage });
            }
        }
  
    
    }
}
