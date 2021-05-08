using Client.Source.Components;
using Leopotam.Ecs;

namespace Client.Source.Systems.Leo
{
    public class RemoveTestStartEventLeoSystem : IEcsRunSystem
    {
        private EcsFilter<TestStartEvent>.Exclude<PrewarmComponent> _startEventFilter;
        
        public void Run()
        {
            if (_startEventFilter.IsEmpty()) return;
            
            _startEventFilter.GetEntity(0).Destroy();
        }
    }
}