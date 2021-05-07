using Client.Source.Tests.Components;
using Leopotam.Ecs;

namespace Client.Source.Tests.Systems.Leo
{
    public class RemoveVisualLeoSystem : IEcsRunSystem
    {
        private EcsFilter<DestroyEntityComponent, VisualComponent>.Exclude<IsNewComponent> _removeVisualFilter;
        
        public void Run()
        {
            if (_removeVisualFilter.IsEmpty()) return;

            foreach (var idx in _removeVisualFilter)
            {
                ref var entity = ref _removeVisualFilter.GetEntity(idx);
                entity.Del<VisualComponent>();
                entity.Get<IsNewComponent>();
            }
        }
    }
}