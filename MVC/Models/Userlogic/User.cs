﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace MVC.Models.Userlogic
{
    public class User
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public decimal Balance { get; set; }
        public string Hash { get; set; }
        public string Salt { get; set; }
        public List<Lobby> MemberOfLobbies { get; set; }
        public List<Lobby> InvitedToLobbies { get; set; }
        public List<Bet> Bets { get; set; }
        public List<Outcome> Outcomes { get; set; }



        public void Persist()
        {
            
        }

        public void Delete()
        {
            
        }

        public void Assignment(MVC.Database.Models.User db)
        {
            this.FirstName = db.FirstName;
            this.LastName = db.LastName;
            this.Username = db.Username;
            this.Email = db.Email;
            this.Balance = db.Balance;


        }
    }
}