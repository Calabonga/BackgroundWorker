namespace Calabonga.BackgroundWorker.Api.Entities
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
        /// price updated
        /// </summary>
        PriceCalculation,

        /// <summary>
        /// XLSX-file generation
        /// </summary>
        PriceGeneration,

        /// <summary>
        /// Send file to customer
        /// </summary>
        PriceSending
    }
}