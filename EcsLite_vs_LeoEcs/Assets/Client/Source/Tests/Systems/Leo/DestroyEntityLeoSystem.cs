using Client.Source.Tests.Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Client.Source.Tests.Systems.Leo
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

                if (entity.Has<VisualComponent>())
                    Object.Destroy(entity.Get<VisualComponent>().View);

                entity.Destroy();
            }
        }
    }
}