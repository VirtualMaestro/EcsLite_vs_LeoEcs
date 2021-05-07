using Client.Source.Tests.Components;
using Leopotam.Ecs;

namespace Client.Source.Tests.Systems.Leo
{
    public class AddVisualLeoSystem : IEcsRunSystem
    {
        private EcsFilter<IsNewComponent, DestroyEntityComponent>.Exclude<VisualComponent> _newFilter;
        
        public void Run()
        {
            if (_newFilter.IsEmpty()) return;

            foreach (var idx in _newFilter)
            {
                ref var entity = ref _newFilter.GetEntity(idx);
                entity.Del<IsNewComponent>();
                entity.Get<VisualComponent>();
            }
        }
    }
}