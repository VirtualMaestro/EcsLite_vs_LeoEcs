using Client.Source.Components;
using Leopotam.Ecs;

namespace Client.Source.Systems.Leo
{
    public class DestroyEntityLeoSystem : IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter<DestroyEntityComponent> _destroyEntityFilter;

        public void Run()
        {
            if (_destroyEntityFilter.IsEmpty()) return;

            foreach (var entityId in _destroyEntityFilter)
            {
                ref var entity = ref _destroyEntityFilter.GetEntity(entityId);
                ref var destroyEntityComponent = ref entity.Get<DestroyEntityComponent>();
                destroyEntityComponent.Frames--;

                if (destroyEntityComponent.Frames > 0) continue;

                entity.Destroy();
            }
        }
    }
}