using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IgrajKarte.Helpers
{
    public static class Utils
    {
        public static List<int> GetPlayersInIds(Room room)
        { 
            List<int> playersIn = new List<int>(0);
            if (room.PlayerIDIn.Trim().Trim(',') != "")
            {
                string[] pIds = room.PlayerIDIn.Trim().Trim(',').Split(',');

                foreach (string playerId in pIds)
                {
                    playersIn.Add(Int32.Parse(playerId));
                }
            }
            return playersIn;
        }

        public static List<int> GetPlayersOutIds(Room room)
        {
            List<int> playersOut = new List<int>(0);
            if (room.PlayerIDOut.Trim().Trim(',') != "")
            {
                string[] pIOut = room.PlayerIDOut.Trim().Trim(',').Split(',');
                foreach (string playerOut in pIOut)
                {
                    playersOut.Add(Int32.Parse(playerOut));
                }
            }

            return playersOut;
        }

        public static string SetPlayersIds(List<int> playerIds)
        {
            string iDs = "";

            foreach (var id in playerIds)
            {
                iDs += "," + id.ToString();
            }

            iDs = iDs.Trim(',');

            return iDs;
        }


    }
}