using Client.Source.Components;
using Leopotam.Ecs;

namespace Client.Source.Systems.Leo
{
    public class PrewarmFinishLeoSystem : IEcsRunSystem
    {
        private EcsFilter<DestroyEntityComponent> _destroyEntityFilter;
        private EcsFilter<PrewarmComponent> _prewarmFilter;
        private EcsFilter<TestProgressEvent> _testProgressFilter;
        
        public void Run()
        {
            if (_prewarmFilter.IsEmpty()) return;

            foreach (var idx in _destroyEntityFilter)
            {
                _destroyEntityFilter.GetEntity(idx).Destroy();
            }
            
            _testProgressFilter.GetEntity(0).Destroy();
        }
    }
}