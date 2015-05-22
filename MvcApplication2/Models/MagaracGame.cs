using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using IgrajKarte.Helpers;

namespace IgrajKarte.Models
{
    public class MagaracGame
    {
        [Required]
        [Display(Name = "Id")]
        public int Id;

        [Required]
        [Display(Name = "Player 1")]
        public Player Player1;

        [Required]
        [Display(Name = "Player 2")]
        public Player Player2;

        [Required]
        [Display(Name = "Player 3")]
        public Player Player3;

        [Required]
        [Display(Name = "Player 4")]
        public Player Player4;

        [Required]
        [Display(Name = "Active Player")]
        public int ActivePlayer;

        [Required]
        [Display(Name = "Cards in Deck")]
        public string CardsInDeck;

        [Required]
        [Display(Name = "T2 Counter")]
        public int t2Counter;

        [Required]
        [Display(Name = "Status")]
        public int Status;
    }
}