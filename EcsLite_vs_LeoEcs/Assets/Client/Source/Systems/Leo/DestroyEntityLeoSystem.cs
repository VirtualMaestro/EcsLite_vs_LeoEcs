using Client.Source.Components;
using Leopotam.Ecs;

namespace Client.Source.Systems.Leo
{
    public class DestroyEntityLeoSystem : IEcsRunSystem
    {
        private EcsFilter<DestroyEntityComponent> _destroyEntityFilter;
        private EcsFilter<PrewarmComponent> _prewarmFilter;

        public void Run()
        {
            if (_destroyEntityFilter.IsEmpty() || !_prewarmFilter.IsEmpty()) return;

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