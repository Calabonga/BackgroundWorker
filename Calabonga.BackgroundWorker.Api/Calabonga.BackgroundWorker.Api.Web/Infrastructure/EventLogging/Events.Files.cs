using System;
using Microsoft.Extensions.Logging;

namespace Calabonga.BackgroundWorker.Api.Web.Infrastructure.EventLogging
{
    /// <summary>
    /// Microsoft ILogger system helper
    /// </summary>
    static partial class Events
    {
        #region NothingToMerge

        private static readonly Action<ILogger, Exception?> NothingToMergeExecute =
            LoggerMessage.Define(
                LogLevel.Information,
                EventIdHelper.NothingToMergeId,
                "wrongType of Worker detected!");

        public static void NothingToMerge(ILogger logger)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                WrongWorkTypeDetectedForWorkerExecute(logger, null!);
            }
        }

        #endregion
    }
}