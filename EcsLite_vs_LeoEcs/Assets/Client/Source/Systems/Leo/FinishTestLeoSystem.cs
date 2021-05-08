using System.Diagnostics;
using Client.Source.Components;
using Client.Source.Monobehs;
using Leopotam.Ecs;

namespace Client.Source.Systems.Leo
{
    public class FinishTestLeoSystem : IEcsRunSystem
    {
        private EcsFilter<TestProgressEvent> _testProgressFilter;
        private EcsFilter<DestroyEntityComponent> _destroyEntityFilter;
        private TestBed _testBed;
        private Stopwatch _stopwatch;

        public void Run()
        {
            if (!_testProgressFilter.IsEmpty() && _destroyEntityFilter.IsEmpty())
            {
                _testProgressFilter.GetEntity(0).Destroy();

                _stopwatch?.Stop();
                _testBed.EndLeoTest(_stopwatch?.ElapsedMilliseconds ?? 0);
            }
        }
    }
}