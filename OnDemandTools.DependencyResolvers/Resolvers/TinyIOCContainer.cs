using Nancy.TinyIoc;


namespace OnDemandTools.Utilities.Resolvers
{
    public class TinyIOCResolver : IDependencyResolver
    {
        private TinyIoCContainer cntr;


        public TinyIOCResolver(TinyIoCContainer underlyingContainer)
        {
            this.cntr = underlyingContainer;
        }
    

        public void Dispose()
        {
            cntr.Dispose();
        }

      

        public void RegisterImplmentation()
        {
            cntr.Register<OnDemandTools.Business.Modules.AiringId.IAiringIdCreator, OnDemandTools.Business.Modules.AiringId.AiringIdCreator>();
            cntr.Register<OnDemandTools.DAL.Modules.AiringId.IAiringIdSaveCommand, OnDemandTools.DAL.Modules.AiringId.Commands.AiringIdSaveCommand>();
            cntr.Register<OnDemandTools.DAL.Database.IODTDatastore, OnDemandTools.DAL.Database.ODTDatastore>();


            cntr.Register<OnDemandTools.Business.Modules.User.IUserHelper, OnDemandTools.Business.Modules.User.UserHelper>();
            cntr.Register<OnDemandTools.DAL.Modules.User.Queries.IApiGetUserQuery, OnDemandTools.DAL.Modules.User.Queries.UsersQuery>();
            cntr.Register<OnDemandTools.DAL.Modules.User.Queries.IGetUsersQuery, OnDemandTools.DAL.Modules.User.Queries.UsersQuery>();
        }


    }
}