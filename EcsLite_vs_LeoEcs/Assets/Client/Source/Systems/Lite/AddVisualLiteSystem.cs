using Client.Source.Components;
using Leopotam.EcsLite;

namespace Client.Source.Systems.Lite
{
    public class AddVisualLiteSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _newFilter;
        private EcsPool<IsNewComponent> _isNewPool;
        private EcsPool<VisualComponent> _visualPool;
        
        public void Init(EcsSystems systems)
        {
            _world = systems.GetWorld();
            _newFilter = _world.Filter<IsNewComponent>().Inc<DestroyEntityComponent>().Exc<VisualComponent>().End();
            _isNewPool = _world.GetPool<IsNewComponent>();
            _visualPool = _world.GetPool<VisualComponent>();
        }

        public void Run(EcsSystems systems)
        {
            if (_newFilter.GetEntitiesCount() == 0) return;

            foreach (var entityId in _newFilter)
            {
                _isNewPool.Del(entityId);
                _visualPool.Add(entityId);
            }
        }
    }
}