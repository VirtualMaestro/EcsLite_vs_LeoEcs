using System.Diagnostics;
using Client.Source.Components;
using Client.Source.Systems.Lite;
using Leopotam.EcsLite;
using UnityEngine;

namespace Client.Source.Monobehs
{
    public class TestLite : MonoBehaviour
    {
        private EcsSystems _systems;
        private EcsWorld _world;
        
        public EcsWorld World => _world;

        private TestBed _testBed;
        private Stopwatch _stopwatch;

        private void Awake()
        {
            var config = new EcsWorld.Config
            {
                Entities = 1_000_000
            };
            
            _world = new EcsWorld(config);
            _systems = new EcsSystems(_world);

            _systems.Add(new StartTestLiteSystem());
            _systems.Add(new AddVisualLiteSystem());
            _systems.Add(new RemoveVisualLiteSystem());
            _systems.Add(new DestroyEntityLiteSystem());
            _systems.Add(new FinishTestLiteSystem());

            _systems.Add(new RemoveStartEventLiteSystem());
            
            _systems.Add(new PrewarmFinishLiteSystem());
            
            _systems.DelHere<PrewarmComponent>();
        }

        public void InjectTestBed(TestBed testBed)
        {
            _testBed = testBed;
        }
        
        public void InjectStopwatch(Stopwatch stopwatch)
        {
            _stopwatch = stopwatch;
        }

        private void _InitTestBedComponent()
        {
            ref var testBedComponent = ref _world.GetPool<TestBedComponent>().Add(_world.NewEntity());
            testBedComponent.TestBed = _testBed;
            testBedComponent.Timer = _stopwatch;
        }
        
        private void Start()
        {
            _systems.Init();
            _InitTestBedComponent();
        }

        private void Update()
        {
            _systems.Run();
        }
        
        private void OnDestroy()
        {
            _systems.Destroy();
            _systems = null;
            _world.Destroy();
            _world = null;
            _testBed = null;
            _stopwatch = null;
        }
    }
}