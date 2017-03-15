﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MVC.Database.Models
{
    public class Lobby
    {
        [Key]
        public long LobbyId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<Bet> Bets { get; set; }
        public ICollection<UserLobbyMember> Members { get; set; }
        //[Column("InvitedLobbies")]
        //public ICollection<User> Invited { get; set; }
    }
}
