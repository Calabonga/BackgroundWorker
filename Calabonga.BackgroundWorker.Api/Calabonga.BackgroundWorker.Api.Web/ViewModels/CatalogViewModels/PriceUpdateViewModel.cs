namespace Calabonga.BackgroundWorker.Api.Web.ViewModels.CatalogViewModels
{
    /// <summary>
    /// PRice update view model
    /// </summary>
    public class PricesUpdateViewModel
    {
        /// <summary>
        /// Internal catalog rate
        /// </summary>
        public double InternalRate { get; set; }

        /// <summary>
        /// Central Back Russian Federation rate
        /// </summary>
        public double CurrencyRate { get; set; }
    }
}
