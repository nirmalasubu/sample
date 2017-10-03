using System;
using System.Linq;

namespace OnDemandTools.DAL.Modules.UserPermissions.Queries
{
    public interface IUserPermissionQuery
    {
        /// <summary>
        /// Gets all system and users
        /// </summary>
        /// <returns></returns>
        IQueryable<Model.UserPermission> Get();


        /// <summary>
        /// Gets all the portal modules, and its user permission
        /// </summary>
        /// <returns></returns>
        IQueryable<Model.PortalModule> GetAllPortalModules();

        /// <summary>
        /// Get the user permission object id
        /// </summary>
        /// <param name="objectId"></param>
        /// <returns></returns>
        Model.UserPermission GetById(string objectId);

        /// <summary>
        /// Gets the User permission by API key
        /// </summary>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        Model.UserPermission GetByApiKey(Guid apiKey);


        /// <summary>
        /// Gets the user permission by Email address
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        Model.UserPermission GetByUserName(string emailAddress);


        /// <summary>
        /// Gets the System permission by API key and updates the last access time
        /// </summary>
        /// <param name="apiKey">the Api key</param>
        /// <returns></returns>
        Model.UserPermission GetByApiKeyAndUpdateLastAccessedTime(Guid apiKey);

        /// <summary>
        /// Gets the user permission by Email address and updates Last login time.
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        Model.UserPermission GetByUserNameAndUpdateLastLoginTime(string emailAddress);

        /// <summary>
        /// Get the technical/Functional contacts by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IQueryable<Model.UserPermission> GetContactForByUserId(string id);
    }
}
