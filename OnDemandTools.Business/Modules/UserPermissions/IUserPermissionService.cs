﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnDemandTools.Business.Modules.UserPermissions
{
    public interface IUserPermissionService
    {
        /// <summary>
        /// Gets all Userpermission
        /// </summary>
        /// <returns></returns>
        List<Model.UserPermission> GetAll(UserType userType);

        /// <summary>
        /// Gets all potal Modules
        /// </summary>
        /// <returns></returns>
        List<Model.PortalModule> GetAllPortalModules();

        /// <summary>
        /// Saves User Permission to database
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        Model.UserPermission Save(Model.UserPermission userPermission);

        /// <summary>
        /// Get User Permission by Object Id
        /// </summary>
        /// <param name="id">object id</param>
        /// <returns></returns>
        Model.UserPermission GetById(string id);

        /// <summary>
        /// Get the user permissions based on Email address
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        Model.UserPermission GetByUserName(string emailAddress);

        IList<Model.UserPermission> GetContactForByUserId(string id);
    }
}
