﻿using AutoMapper;

using Connections.Services;

using Google.Protobuf.WellKnownTypes;

using Grpc.Core;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;

using Moq;

using NUnit.Framework.Internal;

using Server;
using Server.Entities;

using GameService = Server.Services.GameService;
using LobbyService = Server.Services.LobbyService;

namespace Tests.ServerTests
{
    internal class LobbyServiceTest
    {
        private LobbyService _lobbyService;
        private ConnectionResources _resources;

        private string _nickName = "TEST";

        private readonly Empty _empty = new Empty();

        private ServerCallContext _mockContext;

        [SetUp]
        public void SetUp()
        {
            var config = new MapperConfiguration(config => config.AddProfile<AppMappingProfile>());
            var mapper = new Mapper(config);
            _resources = new ConnectionResources();
            _lobbyService = new LobbyService(mapper, new Mock<GameService>(mapper, _resources).Object, _resources);
            _mockContext = new Mock<ServerCallContext>().Object;
        }

        #region CreateLobbyTests
        [Test]
        public void TRYING_CREATE_LOBBY_SHOUD_DO_IT()
        {
            #region Arrange            
            Guid guid = Guid.NewGuid();
            string nickName = "TEST";

            LobbySetting lobbySetting = new LobbySetting() { DeckType = DeckType.Common, PlayersCount = 2, PlayersStartCount = 2 };
            var lobbyCreateRequest = new LobbyCreateRequest()
            {
                CreatorId = guid.ToString(),
                Name = nickName,
                Password = string.Empty,
                Settings = lobbySetting
            };

            _resources.Users.Add(guid, new() { Guid = guid, NickName = nickName });
            #endregion

            #region Act
            var stream = new Mock<IServerStreamWriter<LobbyState>>().Object;
            _lobbyService.CreateLobby(lobbyCreateRequest, stream, _mockContext);
            #endregion

            #region Assert
            var lobby = _resources.Lobbies.First().Value;
            Assert.IsTrue(lobby is not null);
            Assert.IsTrue(lobby.Name == nickName);
            #endregion
        }
        #endregion

        #region DeleteLobbyTests
        [Test]
        public void TRYING_DELETE_LOBBY_SHOUD_DO_IT()
        {
            #region Arrange            
            var user = Helpers.AddNewUser(_nickName, _resources);
            var lobby = Helpers.AddNewLobby(user, _resources);

            ActionRequest request = new ActionRequest() { LobbyId = lobby.Guid.ToString(), SenderId = user.Guid.ToString() };
            #endregion

            #region Act
            _lobbyService.DeleteLobby(request, _mockContext);
            #endregion

            #region Assert
            Assert.IsTrue(_resources.Lobbies.Count == 0);
            #endregion
        }

        [Test]
        public void TRYING_DELETE_SENDING_WRONG_ID_SHOULD_THROW_EXCEPTION()
        {
            #region Arrange            
            var user = Helpers.AddNewUser(_nickName, _resources);
            var lobby = Helpers.AddNewLobby(user, _resources);
            var wrongId = Guid.NewGuid();

            ActionRequest request = new ActionRequest() { LobbyId = wrongId.ToString(), SenderId = user.Guid.ToString() };
            #endregion

            #region Act           
            try
            {
                _lobbyService.DeleteLobby(request, _mockContext);
            }
            #endregion

            #region Assert
            catch (Exception ex)
            {
                Assert.IsTrue(ex is RpcException);
            }
            #endregion
        }
        #endregion

        #region JoinLobbyTests
        [TestCase("")]
        [TestCase(null)]
        [TestCase("Adsafawr1432132")]
        public void TRYING_JOIN_LOBBY_SENDING_CORRECT_PASSWORD_SHOULD_JOIN(string? password)
        {
            #region Arrange            
            var user = Helpers.AddNewUser(_nickName, _resources);
            var lobby = Helpers.AddNewLobby(user, _resources, password: password);

            ActionRequest actionRequest = new() { LobbyId = lobby.Guid.ToString(), SenderId = user.Guid.ToString() };
            JoinRequest request = new() { ActionRequest = actionRequest, Password = password };
            var stream = new Mock<IServerStreamWriter<LobbyState>>().Object;
            #endregion

            #region Act      
            _lobbyService.JoinLobby(request, stream, _mockContext);
            #endregion

            #region Assert            
            Assert.IsTrue(lobby.Players.Count == 1);
            #endregion
        }

        [TestCase("\n")]
        [TestCase(null)]
        [TestCase("Adsafawr1432132")]
        public void TRYING_JOIN_LOBBY_SENDING_INCORRECT_PASSWORD_SHOULD_THROW_EXCEPTION(string? password)
        {
            #region Arrange            
            var user = Helpers.AddNewUser(_nickName, _resources);
            var lobby = Helpers.AddNewLobby(user, _resources, password: password);

            ActionRequest actionRequest = new() { LobbyId = lobby.Guid.ToString(), SenderId = user.Guid.ToString() };
            JoinRequest request = new() { ActionRequest = actionRequest, Password = string.Empty };
            var stream = new Mock<IServerStreamWriter<LobbyState>>().Object;
            #endregion

            #region Act      
            try
            {
                _lobbyService.JoinLobby(request, stream, _mockContext);
            }
            #endregion

            #region Assert 
            catch (RpcException ex)
            {
                Assert.IsTrue(ex.StatusCode == StatusCode.PermissionDenied);
            }
            #endregion
        }
        #endregion

        #region LeaveLobbyTests
        [Test]
        public void TRYING_LEAVE_LOBBY_SHOULD_LEAVE()
        {
            #region Arrange            
            var user = Helpers.AddNewUser(_nickName, _resources);
            var lobby = Helpers.AddNewLobby(user, _resources);

            ActionRequest actionRequest = new() { LobbyId = lobby.Guid.ToString(), SenderId = user.Guid.ToString() };
            #endregion

            #region Act      
            _lobbyService.LeaveLobby(actionRequest, _mockContext);
            #endregion

            #region Assert            
            Assert.IsTrue(lobby.Players.Count == 0);
            #endregion
        }

        [Test]
        public void TRYING_LEAVE_LOBBY_SENDING_WROND_USER_ID_SHOULD_THROW_EXCEPTION()
        {
            #region Arrange            
            var user = Helpers.AddNewUser(_nickName, _resources);
            var lobby = Helpers.AddNewLobby(user, _resources);
            var wrongGuid = Guid.NewGuid();
            ActionRequest actionRequest = new() { LobbyId = lobby.Guid.ToString(), SenderId = wrongGuid.ToString() };
            #endregion

            #region Act  
            try
            {
                _lobbyService.LeaveLobby(actionRequest, _mockContext);
            }
            #endregion

            #region Assert 
            catch (RpcException ex)
            {
                Assert.IsTrue(ex.StatusCode == StatusCode.NotFound);
            }
            #endregion
        }

        [Test]
        public void TRYING_LEAVE_LOBBY_SENDING_WROND_LOBBY_ID_SHOULD_THROW_EXCEPTION()
        {
            #region Arrange            
            var user = Helpers.AddNewUser(_nickName, _resources);
            var lobby = Helpers.AddNewLobby(user, _resources);
            var wrongGuid = Guid.NewGuid();
            ActionRequest actionRequest = new() { LobbyId = wrongGuid.ToString(), SenderId = user.Guid.ToString() };
            #endregion

            #region Act  
            try
            {
                _lobbyService.LeaveLobby(actionRequest, _mockContext);
            }
            #endregion

            #region Assert 
            catch (RpcException ex)
            {
                Assert.IsTrue(ex.StatusCode == StatusCode.NotFound);
            }
            #endregion
        }
        #endregion

        #region KickPlayerTests
        [Test]
        public void NOT_OWNER_TRYING_KICK_PLAYER_SHOULD_THROW_EXCEPTION()
        {
            #region Arrange            
            var lobbyOwner = Helpers.AddNewUser(_nickName, _resources);
            var kicker = Helpers.AddNewUser(_nickName, _resources);
            var kickingUser = Helpers.AddNewUser(_nickName, _resources);

            var lobby = Helpers.AddNewLobby(lobbyOwner, _resources);
            lobby.Players.Add(kickingUser);

            ActionRequest actionRequest = new() { LobbyId = lobby.Guid.ToString(), SenderId = kicker.Guid.ToString() };
            KickPlayerRequest kickPlayerRequest = new() { KickingPlayerId = kickingUser.Guid.ToString(), ActionRequest = actionRequest };
            #endregion

            #region Act   
            try
            {
                _lobbyService.KickPlayer(kickPlayerRequest, _mockContext);
            }
            #endregion

            #region Assert            
            catch (RpcException ex)
            {
                Assert.IsTrue(lobby.Players.Count == 1);
                Assert.IsTrue(ex.StatusCode == StatusCode.PermissionDenied);
            }
            #endregion
        }

        [Test]
        public void OWNER_TRYING_KICK_PLAYER_SENDING_WRONG_ID_SHOULD_THROW_EXCEPTION()
        {
            #region Arrange            
            var lobbyOwner = Helpers.AddNewUser(_nickName, _resources);
            var kickingUser = Helpers.AddNewUser(_nickName, _resources);

            var lobby = Helpers.AddNewLobby(lobbyOwner, _resources);
            lobby.Players.Add(kickingUser);

            var wrongGuid = Guid.NewGuid();

            ActionRequest actionRequest = new() { LobbyId = lobby.Guid.ToString(), SenderId = lobbyOwner.Guid.ToString() };
            KickPlayerRequest kickPlayerRequest = new() { KickingPlayerId = wrongGuid.ToString(), ActionRequest = actionRequest };
            #endregion

            #region Act   
            try
            {
                _lobbyService.KickPlayer(kickPlayerRequest, _mockContext);
            }
            #endregion

            #region Assert            
            catch (RpcException ex)
            {
                Assert.IsTrue(ex.StatusCode == StatusCode.NotFound);
            }
            #endregion
        }

        [Test]
        public void TRYING_KICK_PLAYER_SHOULD_DO_IT()
        {
            #region Arrange            
            var user = Helpers.AddNewUser(_nickName, _resources);
            var kickingUser = Helpers.AddNewUser(_nickName, _resources);

            var lobby = Helpers.AddNewLobby(user, _resources);
            lobby.Players.Add(kickingUser);

            ActionRequest actionRequest = new() { LobbyId = lobby.Guid.ToString(), SenderId = user.Guid.ToString() };
            KickPlayerRequest kickPlayerRequest = new() { KickingPlayerId = kickingUser.Guid.ToString(), ActionRequest = actionRequest };
            #endregion

            #region Act      
            _lobbyService.KickPlayer(kickPlayerRequest, _mockContext);
            #endregion

            #region Assert

            Assert.IsTrue(lobby.Players.Count == 0);
            #endregion
        }
        #endregion

        #region GetAllLobiesTests
        [TestCase(1)]
        [TestCase(4)]
        [TestCase(0)]
        public void TRYING_GET_ALL_LOBIES_SHOULD_RETURN_ALL_CREATED_LOBBIES(int lobbyCount)
        {
            #region Arrange            
            var user = Helpers.AddNewUser(_nickName, _resources);
            for (int i = 0; i < lobbyCount; i++)
                Helpers.AddNewLobby(user, _resources, i.ToString());

            #endregion

            #region Act 
            var lobbies = _lobbyService.GetAllLobies(_empty, _mockContext).Result;
            var uniqueLobbyCount = lobbies.LobbyList_.Distinct().Count();
            #endregion

            #region Assert
            Assert.IsTrue(uniqueLobbyCount == lobbyCount);
            #endregion
        }
        #endregion 
    }
}