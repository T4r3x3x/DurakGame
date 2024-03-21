using Autofac;

using AutoMapper;

using DurakClient.Factories.ViewModelFactories;
using DurakClient.Factories.ViewModelFactories.Implementations;
using DurakClient.MVVM.ViewModels;
using DurakClient.Services;
using DurakClient.Services.ChannelProviders;
using DurakClient.Services.ConnectionServices;
using DurakClient.Services.GameServices;
using DurakClient.Services.LobbyServices;
using DurakClient.Utilities;

using System;

namespace DurakClient.Setup.DI.ContainerRegisters
{
    internal static class ContainerDependenciesRegister
    {
        internal static ContainerBuilder RegisterDependecies(this ContainerBuilder builder)
        {
            builder.RegisterChannelProvider()
                .RegisterGrpcSevices()
                .RegisterAutoMapper()
                .RegisterClientServices()
                .RegisterViewModels()
                .RegisterViewModelsFactories();
            return builder;
        }

        internal static ContainerBuilder RegisterGrpcSevices(this ContainerBuilder builder)
        {
            builder.RegisterGrpcService<Connections.Services.LobbyService.LobbyServiceClient>();
            builder.RegisterGrpcService<Connections.Services.GameService.GameServiceClient>();
            builder.RegisterGrpcService<Connections.Services.ConnectionService.ConnectionServiceClient>();
            return builder;
        }

        internal static void RegisterGrpcService<T>(this ContainerBuilder builder)
        {
            builder.Register((IChannelProvider channelProvider) =>
                (T)Activator.CreateInstance(typeof(T), channelProvider.GetInvoker())!);
        }

        internal static ContainerBuilder RegisterAutoMapper(this ContainerBuilder builder)
        {
            builder.Register(_ => MappingProfilesRegister.GetMapper())
                .As<IMapper>()
                .InstancePerLifetimeScope();
            return builder;
        }

        internal static ContainerBuilder RegisterClientServices(this ContainerBuilder builder)
        {
            builder.RegisterType<LobbyService>()
                .As<ILobbyService>().SingleInstance();
            builder.RegisterType<GameService>()
                .As<IGameService>();
            builder.RegisterType<ConnectionService>()
                .As<IConnectionService>();
            builder.RegisterType<Resources>();
            return builder;
        }

        internal static ContainerBuilder RegisterChannelProvider(this ContainerBuilder builder)
        {
            builder.RegisterType<ChannelProvider>()
                .As<IChannelProvider>();
            return builder;
        }

        internal static ContainerBuilder RegisterViewModels(this ContainerBuilder builder)
        {
            builder.RegisterType<ConnectionViewModel>();
            builder.RegisterType<FilterViewModel>();
            builder.RegisterType<CreateLobbyViewModel>();
            builder.RegisterType<LobbiesViewModel>();
            return builder;
        }

        internal static ContainerBuilder RegisterViewModelsFactories(this ContainerBuilder builder)
        {
            builder.RegisterType<ConnectionViewModelFactory>()
                .As<IViewModelFactory<ConnectionViewModel>>();
            builder.RegisterType<FilterViewModelFactory>()
                .As<IViewModelFactory<FilterViewModel>>();
            builder.RegisterType<LobbiesViewModelFactory>()
                .As<IViewModelFactory<LobbiesViewModel>>();
            builder.RegisterType<CreateLobbyViewModelFactory>()
               .As<IViewModelFactory<CreateLobbyViewModel>>();
            return builder;
        }
    }
}