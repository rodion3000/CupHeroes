using UnityEngine;
using System.Threading.Tasks;
using Project.Dev.GamePlay.Location;

namespace Project.Dev.Infrastructure.Factories.Interfaces
{
    public interface IStageFactorie
    {
        Task WarmUp(string locationName);
        void CleanUp(string locationName);
        Task<LocationManager> CreateLocation(string locationName);

    }
}
