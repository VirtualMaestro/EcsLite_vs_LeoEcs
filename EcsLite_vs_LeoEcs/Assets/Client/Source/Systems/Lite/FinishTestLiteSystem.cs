using Client.Source.Components;
using Client.Source.Extensions;
using Leopotam.EcsLite;

namespace Client.Source.Systems.Lite
{
    public class FinishTestLiteSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _testProgressFilter;
        private EcsFilter _destroyEntityFilter;
        private EcsFilter _testBedFilter;
        private EcsPool<TestBedComponent> _testBedPool;

        public void Init(EcsSystems systems)
        {
            _testProgressFilter = systems.GetWorld().Filter<TestProgressEvent>().End();
            _destroyEntityFilter = systems.GetWorld().Filter<DestroyEntityComponent>().End();
            _testBedFilter = systems.GetWorld().Filter<TestBedComponent>().End();
            _testBedPool = systems.GetWorld().GetPool<TestBedComponent>();
        }

        public void Run(EcsSystems systems)
        {
            if (_testProgressFilter.GetEntitiesCount() > 0 && _destroyEntityFilter.GetEntitiesCount() == 0)
            {
                systems.GetWorld().DelEntity(_testProgressFilter.First());

                ref var testBedComponent = ref _testBedPool.Get(_testBedFilter.First());
                var stopwatch = testBedComponent.Timer;
                stopwatch?.Stop();

                testBedComponent.TestBed.EndLiteTest(stopwatch?.ElapsedMilliseconds ?? 0);
            }
        }
    }
}