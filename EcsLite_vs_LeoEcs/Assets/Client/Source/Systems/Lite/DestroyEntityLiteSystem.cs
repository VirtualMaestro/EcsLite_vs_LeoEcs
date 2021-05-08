using Client.Source.Components;
using Leopotam.EcsLite;

namespace Client.Source.Systems.Lite
{
    public class DestroyEntityLiteSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _destroyEntityFilter;
        private EcsPool<DestroyEntityComponent> _destroyPool;
        private EcsPool<VisualComponent> _viewPool;
        private EcsWorld _world;

        public void Init(EcsSystems systems)
        {
            _world = systems.GetWorld();
            _destroyPool = _world.GetPool<DestroyEntityComponent>();
            _viewPool = _world.GetPool<VisualComponent>();
            _destroyEntityFilter = _world.Filter<DestroyEntityComponent>().End();
        }

        public void Run(EcsSystems systems)
        {
            if (_destroyEntityFilter.GetEntitiesCount() == 0) return;

            foreach (var entityId in _destroyEntityFilter)
            {
                ref var destroyEntityComponent = ref _destroyPool.Get(entityId);
                destroyEntityComponent.Frames--;

                if (destroyEntityComponent.Frames > 0) continue;

                _world.DelEntity(entityId);
            }
        }
    }
}