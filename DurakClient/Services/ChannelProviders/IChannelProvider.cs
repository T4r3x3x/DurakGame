using Grpc.Core;

namespace DurakClient.Services.ChannelProviders
{
    internal interface IChannelProvider
    {
        CallInvoker GetInvoker();
    }
}