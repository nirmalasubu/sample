using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using DLModel = OnDemandTools.DAL.Modules.Pathing.Model;


namespace OnDemandTools.DAL.Modules.Pathing.Command
{
    public interface IPathTranslationCommand
    {

        /// <summary>
        /// Save the given path translation model. If it already exist,
        /// update it; else, create a new one.
        /// </summary>
        /// <param name="model">Path translation model</param>
        /// <returns>Newly added or updated path translation model</returns>
        DLModel.PathTranslation Save(DLModel.PathTranslation pathTranslation);


        /// <summary>
        /// Delete path translation that matches the given object id
        /// </summary>
        /// <param name="id">Path translation object id</param>    
        void Delete(string id);
    }
}