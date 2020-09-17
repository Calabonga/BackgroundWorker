using System.Linq;
using Calabonga.BackgroundWorker.Api.Infrastructure.Entities;
using Calabonga.BackgroundWorker.Api.Infrastructure.Helpers;

namespace Calabonga.BackgroundWorker.Api.Extensions
{
    /// <summary>
    /// Work extensions
    /// </summary>
    public static class WorkExtensions
    {
        /// <summary>
        /// Returns true when work has children
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool HasChildren(this Work source)
        {
            return source.Children != null && source.Children.Any();
        }

        /// <summary>
        /// Return userName from parameters
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string GetUserName(this Work source)
        {
            return source.GetParamByName<string>(ParamsProperty.ParameterUserName);
        }
    }
}