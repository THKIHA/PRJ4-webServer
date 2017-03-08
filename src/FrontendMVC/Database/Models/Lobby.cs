﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FrontendMVC.Database.Models
{
    public class Lobby
    {
        [Key]
        public long LobbyId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public List<Bet> Bets { get; set; }
        public List<User> Members { get; set; }
        public List<User> Invited { get; set; }
    }
}
