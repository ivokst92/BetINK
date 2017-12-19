namespace BetINK.Tests.Mocks
{
    using BetINK.Web.Infrastructure.Mapping;

    public static class Mapper
    {
        public static void Initialize()
        => AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });
    }
}
