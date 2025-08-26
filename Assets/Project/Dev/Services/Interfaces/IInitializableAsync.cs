using System.Threading;
using System.Threading.Tasks;

namespace Project.Dev.Services.Interfaces
{
    public interface IInitializableAsync
    {
        public Task InitializeAsync(CancellationToken cancellationToken = default);
    }
}

