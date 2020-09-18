using System.Threading.Tasks;

using Calabonga.UnitOfWork;

using Microsoft.Extensions.Logging;

namespace Calabonga.BackgroundWorker.Api.Web.Infrastructure.Working
{
    /// <summary>
    /// Worker uses IUnitOfWork for access database (Nimble Framework)
    /// https://youtu.be/WbSwp1Aa7hM
    /// https://youtu.be/XUFphtpKZtI
    /// https://youtu.be/aIYZ92CEJN8
    /// </summary>
    public class Worker : WorkerBase, IWorker
    {
        public Worker(IUnitOfWork unitOfWork, ILogger<Worker> logger)
            : base(unitOfWork, logger)
        {
        }

        /// <summary>
        /// Appends work for worker and save it to database
        /// </summary>
        /// <returns></returns>
        public Task AppendWorkPriceCalculationAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}