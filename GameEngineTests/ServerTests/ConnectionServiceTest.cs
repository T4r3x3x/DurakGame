using Connections.Services;

using Grpc.Core;

using Moq;

using Server.Entities;

using ConnectionService = Server.Services.ConnectionService;

namespace Tests.ServerTests
{
    internal class ConnectionServiceTest
    {
        private ConnectionService _connectionService;
        private ConnectionResources _resources = new();
        private string _nickName = "TEST";

        [SetUp]
        public void SetUp()
        {
            _connectionService = new(_resources);
        }

        [Test]
        public void USER_TRYING_CONNECT_SHOULD_CONNECT()
        {
            #region Arrange            
            var loginRequest = new LoginRequest() { NickName = _nickName };
            var mockContext = new Mock<ServerCallContext>();
            #endregion

            #region Act
            var id = _connectionService.Connect(loginRequest, mockContext.Object).Result;
            #endregion

            #region Assert
            var user = _resources.GetUser(id.Id);
            Assert.IsTrue(user.NickName == _nickName);
            #endregion
        }

        [Test]
        public void USER_TRYING_DISCONNECT_SHOULD_DO_IT()
        {
            #region Arrange
            var user = Helpers.AddNewUser(_nickName, _resources);
            var playerId = new PlayerId() { Id = user.ToString() };
            var mockContext = new Mock<ServerCallContext>();
            #endregion

            #region Act
            _connectionService.Disconnect(playerId, mockContext.Object);
            #endregion

            #region Assert
            var searchResult = _resources.Users.TryGetValue(user.Guid, out var temp);
            Assert.IsTrue(searchResult == false);
            #endregion
        }

        [Test]
        public void USER_TRYING_DISCONNECT_SENDING_WRONG_ID_SHOULD_THROW_EXCEPTION()
        {
            #region Arrange            
            var wrongGuid = Guid.NewGuid();
            var playerId = new PlayerId() { Id = wrongGuid.ToString() };
            var mockContext = new Mock<ServerCallContext>();
            #endregion

            #region Act
            try
            {
                _connectionService.Disconnect(playerId, mockContext.Object);
            }
            #endregion

            #region Assert   
            catch (Exception ex)
            {
                Assert.IsTrue(ex is RpcException);
            }
            #endregion
        }
    }
}