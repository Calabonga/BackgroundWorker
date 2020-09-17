using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Calabonga.BackgroundWorker.Api.Mediator;
using Calabonga.OperationResultsCore;

namespace Calabonga.BackgroundWorker.Api.Infrastructure.Services
{
    public interface IFileService
    {
        /// <summary>
        /// Merges all file to one big string and return result
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        Task<OperationResult<UploadResult>> MergeAsync(FileInfo[] files);
    }

    public class FileService : IFileService
    {
        public async Task<OperationResult<UploadResult>> MergeAsync(FileInfo[] files)
        {
            var result = new UploadResult(files.Select(x => x.Name).ToList());
            var operation = OperationResult.CreateResult(result);

            foreach (var file in files.Where(file => Path.GetExtension(file.Extension) == ".txt"))
            {
                using var reader = new StreamReader(file.OpenRead());
                string? line;
                while (!string.IsNullOrEmpty(line = await reader.ReadLineAsync()))
                {
                    operation.Result.StringBuilder.AppendLine(line);
                }
            }

            return operation;
        }
    }
}