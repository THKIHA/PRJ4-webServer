﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Common;
using Common.Models;
using MVC.Identity;
using MVC.Others;
using MVC.ViewModels;

namespace MVC.Controllers
{
    [Authorize]
    public class LobbyController : BaseController
    {
        public LobbyController(IFactory factory, IUserContext userContext)
            : base(factory, userContext)
        {
 
        }

        #region Accept

        //GET: /<controller/Accept/<id>
        public ActionResult Accept(long id)
        {
            using (var myWork = GetUOF)
            {
                var lobby = myWork.Lobby.Get(id);

                if (lobby != null)
                {
                    var myUser = myWork.User.Get(GetUserName);
                    lobby.AcceptLobby(myUser);
                }

                myWork.Complete();
            }

            
            return Redirect("/Lobby/List");
        }

        #endregion

        #region Create

        // GET: /<controller>/Create
        public ActionResult Create()
        {
            return View(new CreateLobbyViewModel());
        }

        [HttpPost]
        // POST: /<controller>/Create
        public ActionResult Create(CreateLobbyViewModel viewModel)
        {
            using (var myWork = GetUOF)
            {
                // Perform data validation.
                if (!TryValidateModel(viewModel))
                {
                    // Errors, return and let the user handle it.
                    return View(viewModel);
                }

                // Create the domain lobby object.
                var lobby = new Lobby()
                {
                    Name = viewModel.Name,
                };

                // Save to the database.
                myWork.Lobby.Add(lobby);
                var aUser = myWork.User.Get(GetUserName);
                lobby.MemberList.Add(aUser);
                myWork.Complete();

                // Show the newly created lobby.
                return Redirect($"/Lobby/Show/{lobby.LobbyId}");
            }

           
        }

        #endregion

        #region Invite

        [HttpGet]
        public ActionResult Invite(long id)
        {
            using (var myWork = GetUOF)
            {
                var lobby = myWork.Lobby.Get(id);

                if (lobby == null)
                {
                    return HttpNotFound();
                }

                var currentUser = myWork.User.Get(GetUserName);

                if (!lobby.MemberList.Contains(currentUser))
                {
                    return HttpForbidden();
                }
            }

            var viewModel = new InviteToLobbyViewModel()
            {
                Id = id
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Invite(InviteToLobbyViewModel viewModel)
        {
            using (var myWork = GetUOF)
            {
                var user = myWork.User.Get(viewModel.Username);
                var lobby = myWork.Lobby.Get(viewModel.Id);


                var currentUser = myWork.User.Get(GetUserName);

                if (!lobby.MemberList.Contains(currentUser))
                {
                    return HttpForbidden();
                }

                // Does the user exits?
                if (user != null)
                {
                    // Is the user already a member of the lobby?
                    if (lobby.MemberList.Contains(user))
                    {
                        ModelState.AddModelError("Username", Resources.Lobby.ErrorUserAlreadyInLobby);

                        return View("Invite", viewModel);
                    }

                    lobby.InviteUserToLobby(user);
                    myWork.Complete();
                }
                else
                {
                    // User doesn't exist.
                    ModelState.AddModelError("Username", Resources.Lobby.ErrorInvitedUserDoesNotExist);

                    return View("Invite", viewModel);
                }

                return Redirect($"/Lobby/Show/{lobby.LobbyId}");
            }
        }

        #endregion

        #region List

        // GET: /<controller>/List
        public ActionResult List()
        {
            using (var myWork = GetUOF)
            {
                var aUser = myWork.User.Get(GetUserName);

                // Display the lobbies.
                var viewModel = new LobbiesViewModel()
                {
                    MemberOfLobbies = aUser.MemberOfLobbies,
                    InvitedToLobbies = aUser.InvitedToLobbies
                };

                return View(viewModel);
            }   
        }

        #endregion

        #region Leave

        // GET: /<controller>/Show/<id>
        public ActionResult Leave(long id)
        {
            using (var myWork = GetUOF)
            {
                // Get the lobby from the database.
                var lobby = myWork.Lobby.Get(id);

                if (lobby == null)
                {
                    // Error.
                    throw new Exception("No such lobby");
                }

                lobby.RemoveMemberFromLobby(myWork.User.Get(GetUserName));

                if (!lobby.MemberList.Any())
                {
                    //Slet tom lobby
                    myWork.Lobby.Remove(lobby);
                }

                myWork.Complete();
                return Redirect($"/Lobby/List");
            }
        }

        #endregion

        #region Remove

        // GET: /<controller>/Remove/<id>
        public ActionResult Remove(long id)
        {
            using (var myWork = GetUOF)
            {
                // Get the lobby from the database.
                var lobby = myWork.Lobby.Get(id);

                if (lobby == null)
                {
                    // Error.
                    throw new Exception("No such lobby");
                }

                lobby.RemoveLobby();

                myWork.Lobby.Remove(lobby);
                myWork.Complete();
                return Redirect("Lobby/List");
            }
        }

        #endregion

        #region Show

        // GET: /<controller>/Show/<id>
        public ActionResult Show(long id)
        {
            using (var myWork = GetUOF)
            {
                // Get the lobby from the database.
                var lobby = myWork.Lobby.GetEager(id);
                var user = myWork.User.Get(GetUserName);

                if (lobby == null)
                {
                    return HttpNotFound();
                }

                if (!lobby.MemberList.Contains(user))
                {
                    return HttpForbidden();
                }

                var myBets = new List<Bet>();
                foreach (var bet in lobby.Bets)
                {
                    var tempBet = myWork.Bet.GetEager(bet.BetId);
                    myBets.Add(tempBet);
                }

                // Sort in active and inactive.
                var activeBets = myBets.Where(b => !b.IsConcluded).ToList();
                var inactiveBets = myBets.Where(b => b.IsConcluded).ToList();

                // Create a viewmodel for the lobby.
                var viewModel = new LobbyViewModel()
                {
                    Id = lobby.LobbyId,
                    Name = lobby.Name,
                    ActiveBets = activeBets,
                    CompletedBets = inactiveBets,
                    Participants = lobby.MemberList,
                    InvitedUsers = lobby.InvitedList
                };

                return View(viewModel);
            }
        }

        #endregion
    }
}
