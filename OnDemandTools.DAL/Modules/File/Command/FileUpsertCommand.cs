using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using OnDemandTools.DAL.Database;

namespace OnDemandTools.DAL.Modules.File.Command
{
    public class FileUpsertCommand : IFileUpsertCommand
    {
        #region Private Properties
        private readonly MongoDatabase _database;
        #endregion

        #region Constructor

        ///<summary>
        /// Constructor
        ///</summary>
        public FileUpsertCommand(IODTDatastore connection)
        {
            _database = connection.GetDatabase();
        }

        #endregion

     
        #region Public Methods

        /// <summary>
        /// Persists the non video files. If data point unique=true, update existing
        /// entry; else, add provided file as a new entry
        /// </summary>
        /// <param name="files">The files.</param>
        /// <param name="user">The user.</param>
        public void PersistNonVideoFiles(List<Model.File> files, string userName)
        {
            foreach (Model.File file in files)
            {
                PersistNonVideoFile(file, userName);
            }
        }


        /// <summary>
        /// Persists the non video file. If data point unique=true, update existing
        /// entry; else, add provided file as a new entry
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="user">The user.</param>
        public void PersistNonVideoFile(Model.File file, string userName)
        {
            var collection = _database.GetCollection<Model.File>("File");            

            if (file.Unique)
            {
                var filter = Query.And(
                    Query.EQ("TitleId", file.TitleId),
                    Query.Matches("Match", new BsonRegularExpression("^" + file.Match + "$", "i")));

                collection.Remove(filter, RemoveFlags.None);
            }

            // Set audit information and save
            file.ModifiedDatetime = DateTime.UtcNow;
            file.ModifiedBy = userName;
            collection.Insert(file);
        }


        /// <summary>
        /// Persists the video files.
        /// </summary>
        /// <param name="files">The files.</param>
        /// <param name="user">The user.</param>
        public void PersistVideoFiles(List<Model.File> files, string userName)
        {
            foreach (var file in files)
            {
                PersistVideoFile(file, userName);
            }
        }


        /// <summary>
        /// Persist the provided file content. The key logic is as follows:
        /// 
        /// If [titleid!=empty && airingid==empty && mediaid==empty]
        ///     register by titleid. If [TitleId, Type, Domain, Path & Name] match, update; else, insert
        ///     
        /// If [titleid!=empty && airingid!=empty && mediaid==empty]
        ///     register by titleid (even if airingid is associated with
        ///     mediaid that currently has file information). If [TitleId, Type, Domain, Path & Name] match, 
        ///     update; else, insert            
        ///     
        /// If [titleid!=empty && airingid!=empty && mediaid!=empty]
        ///     register by titleid (even if airingid is associated with
        ///     mediaid that currently has file information). If [TitleId, Type, Domain, Path & Name] 
        ///     match, update; else, insert
        /// 
        /// If [titleid==empty && airingid!=empty && mediaid!=empty]
        ///     register by airingid. If [AiringId, Type, Domain, Path & Name] match, update; else, insert
        ///     
        /// If [titleid==empty && airingid==empty && mediaid!=empty]
        ///     register by mediaid. 'contents.name' is considered unique. If it
        ///     exist, update, else insert
        ///     
        /// </summary>
        /// <param name="file"></param>
        public void PersistVideoFile(Model.File file, string userName)
        {
            // If only title id is provided, then register file using titleid.
            // see logic description above
            if (file.TitleId.HasValue)
            {
                ///RegisterVideoFileByTitleId
                RegisterVideoFileByTitleId(file, userName);
            }
            else
            {
                if (!String.IsNullOrEmpty(file.AiringId))
                {
                    // RegisterVideoFileByAiring/AssitId
                    RegisterVideoFileByAiringId(file, userName);
                }
                else
                {
                    // RegisterVideoFileByMediaId
                    RegisterVideoFileByMediaId(file, userName);
                }

            }
        }


        /// <summary>
        /// Saves the specified files. Based on the value for
        /// video, call the appropriate save operation.
        /// </summary>
        /// <param name="files">The files.</param>
        /// <param name="user">The user.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Save(List<Model.File> files, string userName)
        {
            foreach (var file in files)
            {
                if (file.Video)
                {
                    PersistVideoFile(file, userName);
                }
                else
                {
                    PersistNonVideoFile(file, userName);
                }

            }
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// Registers the video file by title identifier.
        /// If [TitleId, Type, Domain, Path & Name] match existing record
        ///  update, else insert.
        ///   
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="user">The user.</param>
        void RegisterVideoFileByTitleId(Model.File file, string userName)
        {
            var collection = _database.GetCollection<Model.File>("File");
            var filter = Query.And(Query.EQ("TitleId", file.TitleId), Query.EQ("Video", true));
            if (collection.Count(filter) == 0)
                collection.Save(file);
            else
            {
                // This removes existing content if it exists and then inserts. 
                foreach (var content in file.Contents)
                {
                    var query = Query.And(
                                Query.EQ("TitleId", file.TitleId),
                                Query.EQ("Video", true),
                                Query.EQ("Contents.Name", content.Name));

                    // Remove, if it exist
                    var pull = Update<Model.File>.Pull(x => x.Contents, builder => builder.EQ(q => q.Name, content.Name));
                    collection.Update(query, pull);

                    // Insert
                    query = Query.And(Query.EQ("TitleId", file.TitleId), Query.EQ("Video", true));
                    collection.Update(query, Update<Model.File>.AddToSet(c => c.Contents, content)
                        .Set(c => c.ModifiedBy, file.ModifiedBy)
                        .Set(c => c.ModifiedDatetime, file.ModifiedDatetime));
                }
            }

            collection.Remove(filter, RemoveFlags.None);

            // Set audit information and save
            file.ModifiedDatetime = DateTime.UtcNow;
            file.ModifiedBy =userName;
            collection.Insert(file);
        }

        /// <summary>
        /// Registers the video file by airing identifier.
        /// If [AiringId] match existing record
        ///  update, else insert.
        ///   
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="user">The user.</param>
        void RegisterVideoFileByAiringId(Model.File file, string userName)
        {
            // Gather audit information
            file.ModifiedDatetime = DateTime.UtcNow;
            file.ModifiedBy = userName;

            // Check if file exist for given AiringId. If not, insert new file document, else;
            // if it exist, verify if the sub document matches. If so, update
            // existing sub document, else insert new sub document         
            MongoCollection<Model.File> fileCollection = _database.GetCollection<Model.File>("File");
            var query = Query.And(Query.EQ("AiringId", file.AiringId), Query.NotExists("MediaId"));
            Model.File dbFile = fileCollection.Find(query).FirstOrDefault();
            if (dbFile == null)
            {
                // Add new file document
                fileCollection.Save(file);
            }
            else
            {

                // This removes existing content if it exists and then inserts. 
                foreach (var content in file.Contents)
                {
                    query = Query.And(
                                Query.EQ("AiringId", file.AiringId),
                                Query.NotExists("MediaId"),
                                Query.EQ("Contents.Name", content.Name));

                    // Remove, if it exist
                    var pull = Update<Model.File>.Pull(x => x.Contents, builder => builder.EQ(q => q.Name, content.Name));
                    fileCollection.Update(query, pull);

                    // Insert
                    query = Query.And(Query.EQ("AiringId", file.AiringId), Query.NotExists("MediaId"));
                    fileCollection.Update(query, Update<Model.File>.AddToSet(c => c.Contents, content)
                        .Set(c => c.ModifiedBy, file.ModifiedBy)
                        .Set(c => c.ModifiedDatetime, file.ModifiedDatetime));
                }
            }
        }


        /// <summary>
        /// Registers the video file by media identifier. 'contents.name' is considered unique. If it
        ///     exist, update, else insert
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="user">The user.</param>
        void RegisterVideoFileByMediaId(Model.File file,string userName)
        {

            // Gather audit information
            file.ModifiedDatetime = DateTime.UtcNow;
            file.ModifiedBy = userName;

            // Check if file exist for given media id. If not, insert new file document, else;
            // if it exist, verify if the sub document matches. If so, update
            // existing sub document, else insert new sub document         
            MongoCollection<Model.File> fileCollection = _database.GetCollection<Model.File>("File");
            var query = Query.And(Query.EQ("MediaId", file.MediaId), Query.NotExists("AiringId"));
            Model.File dbFile = fileCollection.Find(query).FirstOrDefault();
            if (dbFile == null)
            {
                // Add new file document
                fileCollection.Save(file);
            }
            else
            {

                // This removes existing content if it exists and then inserts. 
                foreach (var content in file.Contents)
                {
                    query = Query.And(
                                Query.EQ("MediaId", file.MediaId),
                                Query.NotExists("AiringId"),
                                Query.EQ("Contents.Name", content.Name));

                    // Remove, if it exist
                    var pull = Update<Model.File>.Pull(x => x.Contents, builder => builder.EQ(q => q.Name, content.Name));
                    fileCollection.Update(query, pull);

                    // Insert
                    query = Query.And(Query.EQ("MediaId", file.MediaId), Query.NotExists("AiringId"));
                    fileCollection.Update(query, Update<Model.File>.AddToSet(c => c.Contents, content)
                        .Set(c => c.ModifiedBy, file.ModifiedBy)
                        .Set(c => c.ModifiedDatetime, file.ModifiedDatetime));
                }

            }
        }

      
        #endregion

    }
}
