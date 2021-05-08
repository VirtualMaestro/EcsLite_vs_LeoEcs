using Client.Source.Components;
using Client.Source.Extensions;
using Leopotam.EcsLite;

namespace Client.Source.Systems.Lite
{
    public class RemoveStartEventLiteSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _startEventFilter;
        private EcsWorld _world;
        
        public void Init(EcsSystems systems)
        {
            _world = systems.GetWorld();
            _startEventFilter = _world.Filter<TestStartEvent>().Exc<PrewarmComponent>().End();
        }

        public void Run(EcsSystems systems)
        {
            if (_startEventFilter.GetEntitiesCount() == 0) return;

            _world.DelEntity(_startEventFilter.First());
        }
    }
}