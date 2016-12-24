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
            // As best practice include namespace along with class name. Also be careful not to register same instance multiple times    
            cntr.Register<OnDemandTools.Business.Modules.AiringId.IAiringIdCreator, OnDemandTools.Business.Modules.AiringId.AiringIdCreator>();
            cntr.Register<OnDemandTools.DAL.Modules.AiringId.IAiringIdSaveCommand, OnDemandTools.DAL.Modules.AiringId.Commands.AiringIdSaveCommand>();
            cntr.Register<OnDemandTools.DAL.Database.IODTDatastore, OnDemandTools.DAL.Database.ODTDatastore>();

            cntr.Register<OnDemandTools.Business.Modules.User.IUserHelper, OnDemandTools.Business.Modules.User.UserHelper>();
            cntr.Register<OnDemandTools.DAL.Modules.User.Queries.IApiGetUserQuery, OnDemandTools.DAL.Modules.User.Queries.UsersQuery>();
            cntr.Register<OnDemandTools.DAL.Modules.User.Queries.IGetUsersQuery, OnDemandTools.DAL.Modules.User.Queries.UsersQuery>();

            cntr.Register<OnDemandTools.Business.Modules.AiringId.IIdDistributor, OnDemandTools.Business.Modules.AiringId.IdDistributor>();
            cntr.Register<OnDemandTools.DAL.Modules.AiringId.IGetLastAiringIdQuery, OnDemandTools.DAL.Modules.AiringId.Queries.GetLastAiringIdQuery>();

            cntr.Register<OnDemandTools.Business.Modules.Destination.IDestinationService, OnDemandTools.Business.Modules.Destination.DestinationService>();
            cntr.Register<OnDemandTools.DAL.Modules.Destination.Queries.IDestinationQuery, OnDemandTools.DAL.Modules.Destination.Queries.DestinationQuery>();

            cntr.Register<OnDemandTools.Business.Modules.Product.IProductService, OnDemandTools.Business.Modules.Product.ProductService>();
            cntr.Register<OnDemandTools.DAL.Modules.Product.Queries.IProductQuery, OnDemandTools.DAL.Modules.Product.Queries.ProductQuery>();

            cntr.Register<OnDemandTools.Business.Modules.Queue.IQueueService, OnDemandTools.Business.Modules.Queue.QueueService>();
            cntr.Register<OnDemandTools.DAL.Modules.Queue.Queries.IQueueQuery, OnDemandTools.DAL.Modules.Queue.Queries.QueueQuery>();
        }


    }
}