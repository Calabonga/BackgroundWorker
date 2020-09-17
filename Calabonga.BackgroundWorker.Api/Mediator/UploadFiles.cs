using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Calabonga.BackgroundWorker.Api.Infrastructure.EventLogging;
using Calabonga.BackgroundWorker.Api.Infrastructure.Services;
using Calabonga.BackgroundWorker.Api.Infrastructure.Working;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Calabonga.BackgroundWorker.Api.Mediator
{
    /// <summary>
    /// Request: Upload files
    /// </summary>
    public class MergeFilesRequest : IRequest<Unit> { }

    /// <summary>
    /// Handler: Upload files
    /// </summary>
    public class MergeFilesRequestHandler : IRequestHandler<MergeFilesRequest, Unit>
    {
        private readonly IWorker _worker;
        private readonly ILogger<MergeFailedEventHandler> _logger;
        private readonly IHostEnvironment _environment;
        private readonly IFileService _fileService;

        public MergeFilesRequestHandler(
            IWorker worker,
            ILogger<MergeFailedEventHandler> logger,
            IHostEnvironment environment,
            IFileService fileService)
        {
            _worker = worker;
            _logger = logger;
            _environment = environment;
            _fileService = fileService;
        }


        public async Task<Unit> Handle(MergeFilesRequest request, CancellationToken cancellationToken)
        {
            var folderPath = Path.Combine(_environment.ContentRootPath, "Uploads");

            var files = new DirectoryInfo(folderPath).GetFiles("*.txt");

            if (!files.Any())
            {
                Events.NothingToMerge(_logger);
                return Unit.Value;
            }

            if (files.Any(x => x.Length == 0))
            {
                Events.NothingToMerge(_logger);
                return Unit.Value;
            }

            var result = await _fileService.MergeAsync(files);
            if (!result.Ok)
            {
                await _worker.AppendWorkCleanUploadsAsync();
            }

            return Unit.Value;
        }
    }

    /// <summary>
    /// Merging result
    /// </summary>
    public class UploadResult
    {
        public UploadResult(IEnumerable<string> names)
        {
            Names = names;
            StringBuilder = new StringBuilder();
        }

        public StringBuilder StringBuilder { get; }

        public IEnumerable<string> Names { get; }
    }
}