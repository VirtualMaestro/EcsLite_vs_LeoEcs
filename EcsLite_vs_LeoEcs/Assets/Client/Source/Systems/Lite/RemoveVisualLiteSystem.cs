using Client.Source.Components;
using Leopotam.EcsLite;

namespace Client.Source.Systems.Lite
{
    public class RemoveVisualLiteSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _removeVisualFilter;
        private EcsPool<IsNewComponent> _isNewPool;
        private EcsPool<VisualComponent> _visualPool;

        public void Init(EcsSystems systems)
        {
            _world = systems.GetWorld();
            _removeVisualFilter = _world.Filter<VisualComponent>().Inc<DestroyEntityComponent>().Exc<IsNewComponent>()
                .End();
            _isNewPool = _world.GetPool<IsNewComponent>();
            _visualPool = _world.GetPool<VisualComponent>();
        }

        public void Run(EcsSystems systems)
        {
            if (_removeVisualFilter.GetEntitiesCount() == 0) return;

            foreach (var entityId in _removeVisualFilter)
            {
                _visualPool.Del(entityId);
                _isNewPool.Add(entityId);
            }
        }
    }
}