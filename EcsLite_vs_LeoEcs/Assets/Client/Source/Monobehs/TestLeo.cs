using Client.Source.Components;
using Client.Source.Systems.Leo;
using Leopotam.Ecs;
using UnityEngine;

namespace Client.Source.Monobehs
{
    public class TestLeo : MonoBehaviour
    {
        private EcsSystems _systems;
        private EcsWorld _world;

        public EcsWorld World => _world;

        private void Awake()
        {
            var config = new EcsWorldConfig
            {
                WorldEntitiesCacheSize = 1_000_000
            };

            _world = new EcsWorld(config);
            _systems = new EcsSystems(_world);
            _systems.Add(new StartTestLeoSystem());
            _systems.Add(new AddVisualLeoSystem());
            _systems.Add(new RemoveVisualLeoSystem());
            _systems.Add(new DestroyEntityLeoSystem());
            _systems.Add(new FinishTestLeoSystem());

            _systems.Add(new RemoveTestStartEventLeoSystem());
            
            _systems.Add(new PrewarmFinishLeoSystem());

            _systems.OneFrame<PrewarmComponent>();
        }

        public void Inject(object obj)
        {
            _systems.Inject(obj);
        }

        private void Start()
        {
            _systems.ProcessInjects();
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