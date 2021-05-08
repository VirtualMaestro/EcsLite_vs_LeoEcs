using Client.Source.Components;
using Client.Source.Systems.Lite;
using Leopotam.EcsLite;
using UnityEngine;

namespace Client.Source
{
    public class TestLite : MonoBehaviour
    {
        private EcsSystems _systems;
        private EcsWorld _world;
        
        public EcsWorld World => _world;

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

            _systems.DelHere<TestStartEvent>();
        }
        
        private void Start()
        {
            _systems.Init();
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
        }
    }
}