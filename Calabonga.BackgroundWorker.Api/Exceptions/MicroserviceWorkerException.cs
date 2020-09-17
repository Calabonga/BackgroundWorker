using System;

namespace Calabonga.BackgroundWorker.Api.Exceptions
{
    /// <summary>
    /// Represents Background Worker general exception
    /// </summary>
    [Serializable]
    public class MicroserviceWorkerException : Exception
    {
        public MicroserviceWorkerException() : base(AppData.Exceptions.BackgroundWorkerException)
        {

        }

        public MicroserviceWorkerException(string message) : base(message)
        {

        }

        public MicroserviceWorkerException(string message, Exception exception) : base(message, exception)
        {

        }

        public MicroserviceWorkerException(Exception exception) : base(AppData.Exceptions.BackgroundWorkerException, exception)
        {

        }
    }
}
