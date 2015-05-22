using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using IgrajKarte.Helpers;

namespace IgrajKarte.Models
{
    public class TablicGame
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

        [Display(Name = "Player 3")]
        public Player Player3;

        [Display(Name = "Player 4")]
        public Player Player4;

        [Required]
        [Display(Name = "Nomber of players")]
        public int NOPlayers;

        [Required]
        [Display(Name = "Active Player")]
        public int ActivePlayer;

        [Required]
        [Display(Name = "Cards in Deck")]
        public string CardsInDeck;

        [Required]
        [Display(Name = "Cards on Table")]
        public string CardsOnTable;

        [Required]
        [Display(Name = "SelectedCards")]
        public string SelectedCards;

        [Required]
        [Display(Name = "Status")]
        public int Status;


    }
}