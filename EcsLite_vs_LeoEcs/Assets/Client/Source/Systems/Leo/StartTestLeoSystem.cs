using Client.Source.Components;
using Leopotam.Ecs;

namespace Client.Source.Systems.Leo
{
    public class StartTestLeoSystem : IEcsRunSystem
    {
        private EcsFilter<TestStartEvent> _startEventFilter;
        private EcsFilter<TestBedComponent> _testBedFilter;
        private EcsWorld _world;

        public void Run()
        {
            if (_startEventFilter.IsEmpty()) return;

            ref var testBedComponent = ref _testBedFilter.Get1(0);
            testBedComponent.Timer.Reset();
            testBedComponent.Timer.Start();

            ref var startEvent = ref _startEventFilter.Get1(0);
            var numEntities = startEvent.NumEntities;
            var delay = startEvent.DelayDestroy;

            for (var i = 0; i < numEntities; i++)
            {
                var entity = _world.NewEntity();
                entity.Get<DestroyEntityComponent>().Frames = delay;
                entity.Get<IsNewComponent>();
            }
                

            _world.NewEntity().Get<TestProgressEvent>();

            testBedComponent.TestBed.WriteLeoLog("Start testing...");
        }
    }
}