namespace Calabonga.BackgroundWorker.Api.Infrastructure.Entities
{
    /// <summary>
    /// Work type
    /// </summary>
    public enum WorkType
    {
        /// <summary>
        /// Required by naming conventions
        /// https://youtu.be/xMTPlajeS3M
        /// </summary>
        None,

        /// <summary>
        /// Checks files in the folder and start processing
        /// </summary>
        ProcessUploadsFolder,

        /// <summary>
        /// Clean up temporary folder
        /// </summary>
        CleanUploadsFolder
    }
}