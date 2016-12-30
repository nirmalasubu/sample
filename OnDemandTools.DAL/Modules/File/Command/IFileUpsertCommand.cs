using System.Collections.Generic;

namespace OnDemandTools.DAL.Modules.File.Command
{
    public interface IFileUpsertCommand
    {
        /// <summary>
        /// Saves the specified files.
        /// </summary>
        /// <param name="files">The files.</param>
        /// <param name="userName">The user.</param>
        void Save(List<Model.File> files, string userName);

        /// <summary>
        /// Persists the non video files.
        /// </summary>
        /// <param name="files">The files.</param>
        /// <param name="userName">The user.</param>
        void PersistNonVideoFiles(List<Model.File> files, string userName);

        /// <summary>
        /// Persists the video file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="userName">The user.</param>
        void PersistVideoFile(Model.File file, string userName);

        /// <summary>
        /// Persists the video files.
        /// </summary>
        /// <param name="files">The files.</param>
        /// <param name="userName">The user.</param>
        void PersistVideoFiles(List<Model.File> files, string userName);

    }
}
