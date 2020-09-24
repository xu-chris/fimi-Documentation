using _Project.Scripts.DomainObjects;

namespace _Project.Scripts.Core
{
    public interface ISkeletonOrchestrator
    {
        void Update(Person[] detectedPersons);
        void InitializeAllSkeletons();
    }
}