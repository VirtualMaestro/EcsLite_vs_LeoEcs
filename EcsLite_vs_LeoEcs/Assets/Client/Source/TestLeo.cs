using Client.Source.Components;
using Client.Source.Systems.Leo;
using Leopotam.Ecs;
using UnityEngine;

namespace Client.Source
{
    public class TestLeo : MonoBehaviour
    {
        [SerializeField]
        private int numCachedEntities;
        
        private EcsSystems _leoSystems;
        private EcsWorld _leoWorld;
        
        public EcsWorld World => _leoWorld;

        private void Awake()
        {
            var config = new EcsWorldConfig
            {
                WorldEntitiesCacheSize = numCachedEntities
            };

            _leoWorld = new EcsWorld(config);
            _leoSystems = new EcsSystems(_leoWorld);
            _leoSystems.Add(new StartTestLeoSystem());
            _leoSystems.Add(new AddVisualLeoSystem());
            _leoSystems.Add(new RemoveVisualLeoSystem());
            _leoSystems.Add(new DestroyEntityLeoSystem());
            _leoSystems.Add(new FinishTestLeoSystem());

            _leoSystems.OneFrame<TestStartEvent>();

            _leoSystems.ProcessInjects();
        }

        private void Start()
        {
            _leoSystems.Init();
        }

        private void Update()
        {
            _leoSystems.Run();
        }
    }
}