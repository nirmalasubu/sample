using Microsoft.Extensions.Configuration;
using Nancy;
using Nancy.TinyIoc;
using Newtonsoft.Json;

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
            cntr.Register<ISerializer, OnDemandTools.Utilities.Serialization.CustomJsonSerializer>();

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
            cntr.Register<OnDemandTools.DAL.Modules.Queue.Command.IQueueCommand, OnDemandTools.DAL.Modules.Queue.Command.QueueCommand>();

            cntr.Register<OnDemandTools.Business.Modules.Package.IPackageService, OnDemandTools.Business.Modules.Package.PackageService>();
            cntr.Register<OnDemandTools.DAL.Modules.Package.Commands.IPackageCommand, OnDemandTools.DAL.Modules.Package.Commands.PackageCommand>();
            cntr.Register<OnDemandTools.DAL.Modules.Package.Queries.IPackageQuery, OnDemandTools.DAL.Modules.Package.Queries.PackageQuery>();

            cntr.Register<OnDemandTools.DAL.Modules.Airings.IGetModifiedAiringQuery, OnDemandTools.DAL.Modules.Airings.Queries.GetAiringQuery>();
            cntr.Register<OnDemandTools.DAL.Modules.Airings.IGetAiringQuery, OnDemandTools.DAL.Modules.Airings.Queries.GetAiringQuery>();

            cntr.Register<OnDemandTools.DAL.Modules.Airings.IGetModifiedAiringQuery, OnDemandTools.DAL.Modules.Airings.Queries.GetAiringQuery>();
            cntr.Register<OnDemandTools.DAL.Modules.Airings.IGetAiringQuery, OnDemandTools.DAL.Modules.Airings.Queries.GetAiringQuery>();

            cntr.Register<OnDemandTools.DAL.Modules.File.Command.IFileUpsertCommand, OnDemandTools.DAL.Modules.File.Command.FileUpsertCommand>();            
            cntr.Register< OnDemandTools.DAL.Modules.File.Queries.IFileQuery, OnDemandTools.DAL.Modules.File.Queries.FileQuery> ();
            cntr.Register<OnDemandTools.Business.Modules.File.IFileService, OnDemandTools.Business.Modules.File.FileService>();

            cntr.Register<OnDemandTools.DAL.Modules.Reporting.Command.IReportStatusCommand, OnDemandTools.DAL.Modules.Reporting.Command.ReportStatusCommand>();
            cntr.Register<OnDemandTools.Business.Modules.Reporting.IReportingService, OnDemandTools.Business.Modules.Reporting.DFReportingService>();
            cntr.Register<OnDemandTools.Business.Modules.Airing.IAiringService, OnDemandTools.Business.Modules.Airing.AiringService>();

            cntr.Register<OnDemandTools.Business.Modules.Handler.IHandlerHistoryService, OnDemandTools.Business.Modules.Handler.HandlerHistoryService>();
            cntr.Register<OnDemandTools.Business.Modules.Pathing.IPathingService, OnDemandTools.Business.Modules.Pathing.PathingService>();
            cntr.Register<OnDemandTools.DAL.Modules.Handler.Command.IHandlerHistoryCommand, OnDemandTools.DAL.Modules.Handler.Command.HandlerHistoryCommand>();
            cntr.Register<OnDemandTools.DAL.Modules.Pathing.Queries.IPathTranslationQueries, OnDemandTools.DAL.Modules.Pathing.Queries.PathTranslationQueries>();

            cntr.Register<OnDemandTools.DAL.Modules.Airings.IAiringSaveCommand, OnDemandTools.DAL.Modules.Airings.Commands.AiringSaveCommand>();
            cntr.Register<OnDemandTools.DAL.Modules.Airings.IAiringMessagePusher, OnDemandTools.DAL.Modules.Airings.Commands.AiringMessagePusher>();

            cntr.Register<OnDemandTools.DAL.Modules.Airings.ITaskUpdater, OnDemandTools.DAL.Modules.Airings.Commands.AiringUpdateTaskCommand>();

            // Special initialization for StatusLibrary class
            OnDemandTools.DAL.Modules.Reporting.Library.StatusLibrary.Init(cntr.Resolve<IConfiguration>());

        }
    }
}