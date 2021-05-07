using Client.Source.Tests.Components;
using Leopotam.Ecs;

namespace Client.Source.Tests.Systems.Leo
{
    public class FinishTestLeoSystem : IEcsRunSystem
    {
        private EcsFilter<TestProgressEvent> _testProgressFilter;
        private EcsFilter<DestroyEntityComponent> _destroyEntityFilter;
        private EcsFilter<TestBedComponent> _testBedFilter;

        public void Run()
        {
            if (!_testProgressFilter.IsEmpty() && _destroyEntityFilter.IsEmpty())
            {
                _testProgressFilter.GetEntity(0).Destroy();

                ref var testBedComponent = ref _testBedFilter.Get1(0);
                var stopwatch = testBedComponent.Timer;
                stopwatch.Stop();

                var testBed = testBedComponent.TestBed;
                testBed.SetLeoResult(stopwatch.ElapsedMilliseconds);
            }
        }
    }
}