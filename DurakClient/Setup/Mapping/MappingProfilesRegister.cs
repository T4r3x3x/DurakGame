using AutoMapper;

using Common.Utilities;

namespace DurakClient.Utilities
{
    public class MappingProfilesRegister
    {
        public static IMapper GetMapper()
        {
            var config = new MapperConfiguration(
                cfg =>
                  {
                      cfg.AddProfile<CommonMappingProfile>();
                      cfg.AddProfile<ClientMappingProfile>();
                  });
            return config.CreateMapper();
        }
    }
}