﻿using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Common.Models;
using NSubstitute;
using NUnit.Framework;

namespace Common.Tests.Models
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    class LobbyPropertyTests
    {
        private Lobby _uut;
        private IUtility _utility;

        [SetUp]
        public void Setup()
        {
            _utility = Substitute.For<IUtility>();
            _utility.DatabaseSecure(Arg.Any<string>()).Returns(callinfo => callinfo.ArgAt<string>(0));
            _uut = new Lobby(_utility);
        }

        [Test]
        public void OutcomeId_SetOutcomeId_OutcomeIdSet()
        {
            foreach (var id in UtilityCommen.ValidIds)
            {
                Assert.That(() => _uut.LobbyId = id, Throws.Nothing);
            }
        }

        [Test]
        public void OutcomeId_GetOutcomeId_OutComeIdReturned()
        {
            foreach (var id in UtilityCommen.ValidIds)
            {
                _uut.LobbyId = id;
                Assert.That(_uut.LobbyId, Is.EqualTo(id));
            }
        }

        [Test]
        public void Name_SetValidName_ValidNameSet()
        {
            foreach (var chars in UtilityCommen.ValidCharacters)
            {
                Assert.That(() => _uut.Name = chars, Throws.Nothing);
            }
        }

        [Test]
        public void Name_GetName_NameReturened()
        {
            foreach (var name in UtilityCommen.ValidCharacters)
            {
                _uut.Name = name;
                Assert.That(_uut.Name, Is.EqualTo(name));
            }
        }

        [Test]
        public void Name_SetInvalidName_ThrowsException()
        {
            foreach (var chars in UtilityCommen.InvalidCharacters)
            {
                _utility.DidNotReceive().DatabaseSecure(Arg.Is(chars));
                _uut.Name = chars;
                _utility.Received(1).DatabaseSecure(Arg.Is(chars));
            }
        }


        [Test]
        public void Bets_SetValidBets_BetsSet()
        {
            Assert.That(() => _uut.Bets = UtilityCommen.ValidBets, Throws.Nothing);
        }

        [Test]
        public void Bets_GetValidBets_BetsReturened()
        {
            _uut.Bets = UtilityCommen.ValidBets;

            Assert.That(_uut.Bets, Is.EquivalentTo(UtilityCommen.ValidBets));
        }

        [Test]
        public void MemberList_SetValidMembers_MembersSet()
        {
            Assert.That(() => _uut.MemberList = UtilityCommen.ValidUsers, Throws.Nothing);
        }

        [Test]
        public void MemberList_GetValidMembers_MembersReturned()
        {
            _uut.MemberList = UtilityCommen.ValidUsers;

            Assert.That(_uut.MemberList, Is.EquivalentTo(UtilityCommen.ValidUsers));
        }

        [Test]
        public void InvitedList_SetValidMembers_MembersSet()
        {
            Assert.That(() => _uut.InvitedList = UtilityCommen.ValidUsers, Throws.Nothing);
        }

        [Test]
        public void InvitedList_GetValidMembers_MembersReturned()
        {
            _uut.InvitedList = UtilityCommen.ValidUsers;

            Assert.That(_uut.InvitedList, Is.EquivalentTo(UtilityCommen.ValidUsers));
        }

        [Test]
        public void InviteUserToLobby_AddUserToInvitedList_UserAdded()
        {
            foreach (var user in UtilityCommen.ValidUsers)
            {
                _uut.InviteUserToLobby(user);
                Assert.That(_uut.InvitedList, Contains.Item(user));
            }
        }

        [Test]
        public void InviteUserToLobby_AddUserListToInvitedList_UsersAdded()
        {
            _uut.InviteUsersToLobby(UtilityCommen.ValidUsers.ToList());
            foreach (var user in UtilityCommen.ValidUsers)
            {
                Assert.That(_uut.InvitedList, Contains.Item(user));
            }
        }

        [Test]
        public void AcceptLobby_UserNotInInvitedList_UserNotAddedToMemberList()
        {
            foreach (var user in UtilityCommen.ValidUsers)
            {
                _uut.AcceptLobby(user);
                Assert.That(_uut.MemberList, Is.Empty);
            }
        }

        [Test]
        public void AcceptLobby_UserInInvitedList_UserRemovedFromInvitedList()
        {
            foreach (var user in UtilityCommen.ValidUsers)
            {
                _uut.InviteUserToLobby(user);
                _uut.AcceptLobby(user);
                Assert.That(_uut.InvitedList, Is.Empty);
            }
        }

        [Test]
        public void AcceptLobby_UserInInvitedList_UserAddedToMemberList()
        {
            foreach (var user in UtilityCommen.ValidUsers)
            {
                _uut.InviteUserToLobby(user);
                _uut.AcceptLobby(user);
                Assert.That(_uut.MemberList, Contains.Item(user));
            }
        }

    }
}
