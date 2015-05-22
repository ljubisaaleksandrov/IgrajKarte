using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IgrajKarte.Enums
{
    public enum GameStatus : int
    {
        Created = 1,
        InProgress = 2,
        Resigned = 3,
        Ended = 4
    }
}