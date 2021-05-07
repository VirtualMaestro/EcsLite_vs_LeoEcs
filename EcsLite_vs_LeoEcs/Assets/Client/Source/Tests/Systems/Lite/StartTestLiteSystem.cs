using Client.Source.Tests.Components;
using Client.Source.Tests.Extensions;
using Leopotam.EcsLite;

namespace Client.Source.Tests.Systems.Lite
{
    public class StartTestLiteSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _testStartEventFilter;
        private EcsFilter _testBedFilter;
        private EcsPool<DestroyEntityComponent> _destroyEntityPool;
        private EcsPool<TestStartEvent> _testStartPool;
        private EcsPool<TestProgressEvent> _testProgressPool;
        private EcsPool<TestBedComponent> _testBedPool;
        private EcsPool<IsNewComponent> _isNewPool;
        
        public void Init(EcsSystems systems)
        {
            _world = systems.GetWorld();
            _testStartEventFilter = _world.Filter<TestStartEvent>().End();
            _testBedFilter = _world.Filter<TestBedComponent>().End();
            _destroyEntityPool = _world.GetPool<DestroyEntityComponent>();
            _testStartPool = _world.GetPool<TestStartEvent>();
            _testProgressPool = _world.GetPool<TestProgressEvent>();
            _testBedPool = _world.GetPool<TestBedComponent>();
            _isNewPool = _world.GetPool<IsNewComponent>();
        }

        public void Run(EcsSystems systems)
        {
            if (_testStartEventFilter.GetEntitiesCount() == 0) return;
            
            ref var testBedComponent = ref _testBedPool.Get(_testBedFilter.First());
            testBedComponent.Timer.Reset();
            testBedComponent.Timer.Start();

            ref var startEvent = ref _testStartPool.Get(_testStartEventFilter.First());
            var numEntities = startEvent.NumEntities;
            var delay = startEvent.DelayDestroy;

            for (var i = 0; i < numEntities; i++)
            {
                var entityId = _world.NewEntity();
                _destroyEntityPool.Add(entityId).Frames = delay;
                _isNewPool.Add(entityId);
            }

            _testProgressPool.Add(_world.NewEntity());
            
            testBedComponent.TestBed.WriteLiteLog("Start testing...");
        }
    }
}