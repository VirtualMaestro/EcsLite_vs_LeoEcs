using Client.Source.Components;
using Client.Source.Extensions;
using Leopotam.EcsLite;

namespace Client.Source.Systems.Lite
{
    public class PrewarmFinishLiteSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _prewarmFilter;
        private EcsFilter _destroyEntityFilter;
        private EcsFilter _testProgressFilter;
        private EcsWorld _world;
        
        public void Init(EcsSystems systems)
        {
            _world = systems.GetWorld();
            _prewarmFilter = _world.Filter<PrewarmComponent>().End();
            _destroyEntityFilter = _world.Filter<DestroyEntityComponent>().End();
            _testProgressFilter = _world.Filter<TestProgressEvent>().End();
        }

        public void Run(EcsSystems systems)
        {
            if (_prewarmFilter.GetEntitiesCount() == 0) return;

            foreach (var entityId in _destroyEntityFilter)
            {
                _world.DelEntity(entityId);
            }
            
            _world.DelEntity(_testProgressFilter.First());
        }
    }
}