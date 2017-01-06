using System.Collections.Generic;
using BLModel = OnDemandTools.Business.Modules.File.Model;
using OnDemandTools.Common.Configuration;

namespace OnDemandTools.Business.Modules.File
{
    public interface IFileService
    {
        /// <summary>
        /// Saves the specified files. Update if it already exist
        /// </summary>
        /// <param name="files">The files.</param>
        void Save(List<BLModel.File> files);

        /// <summary>
        /// Gets files by title identifier.
        /// </summary>
        /// <param name="titleId">The title identifier.</param>
        /// <returns></returns>
        List<BLModel.File> GetByTitleId(int titleId);

        /// <summary>
        /// Gets files by airing identifier.
        /// </summary>
        /// <param name="airingId">The airing identifier.</param>
        /// <returns></returns>
        List<BLModel.File> GetByAiringId(string airingId);

        /// <summary>
        /// Gets files by title identifiers, content identifiers, airing identifier
        /// and media identifier
        /// </summary>
        /// <param name="contentIds">The content ids.</param>
        /// <param name="titleIds">The title ids.</param>
        /// <param name="airingId">The airing identifier.</param>
        /// <param name="mediaId">The media identifier.</param>
        /// <returns></returns>
        IList<BLModel.File> GetBy(List<string> contentIds, List<int> titleIds, string airingId, string mediaId);

        /// <summary>
        /// Persists the non video files.
        /// </summary>
        /// <param name="files">The files.</param>
        void PersistNonVideoFiles(List<BLModel.File> files);

        /// <summary>
        /// Persists the video file.
        /// </summary>
        /// <param name="file">The file.</param>
        void PersistVideoFile(BLModel.File file);

        /// <summary>
        /// Persists the video files.
        /// </summary>
        /// <param name="files">The files.</param>
        void PersistVideoFiles(List<BLModel.File> files);

    }
}
