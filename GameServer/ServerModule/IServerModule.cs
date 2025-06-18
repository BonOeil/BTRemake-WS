namespace GameServer.ServerModule
{
    public interface IServerModule
    {
        void PreBuild(WebApplicationBuilder builder);

        void PostBuild(WebApplication app);
    }
}
