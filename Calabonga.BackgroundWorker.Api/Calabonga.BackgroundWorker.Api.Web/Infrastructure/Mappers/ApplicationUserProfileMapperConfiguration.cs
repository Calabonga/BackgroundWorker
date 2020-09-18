using Calabonga.BackgroundWorker.Api.Data;
using Calabonga.BackgroundWorker.Api.Web.Infrastructure.Mappers.Base;
using Calabonga.BackgroundWorker.Api.Web.ViewModels.AccountViewModels;

namespace Calabonga.BackgroundWorker.Api.Web.Infrastructure.Mappers
{
    /// <summary>
    /// Mapper Configuration for entity Person
    /// </summary>
    public class ApplicationUserProfileMapperConfiguration : MapperConfigurationBase
    {
        /// <inheritdoc />
        public ApplicationUserProfileMapperConfiguration()
        {
            CreateMap<RegisterViewModel, ApplicationUserProfile>()
                .ForAllOtherMembers(x => x.Ignore());
        }
    }
}