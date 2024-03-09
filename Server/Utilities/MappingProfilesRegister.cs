using AutoMapper;

using Common.Utilities;

namespace Server.Utilities
{
    public class MappingProfilesRegister
    {
        public static IMapper GetMapper()
        {
            var config = new MapperConfiguration(
                cfg =>
                  {
                      cfg.AddProfile<CommonMappingProfile>();
                      cfg.AddProfile<ServerMappingProfile>();
                  });
            return config.CreateMapper();
        }
    }
}
