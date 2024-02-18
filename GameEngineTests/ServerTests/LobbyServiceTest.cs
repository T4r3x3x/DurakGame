using AutoMapper;

using Connections.Services;

using Grpc.Core;

using Moq;

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


    }
}
