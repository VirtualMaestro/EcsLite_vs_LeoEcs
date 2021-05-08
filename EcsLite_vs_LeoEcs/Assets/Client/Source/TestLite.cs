using Client.Source.Components;
using Client.Source.Systems.Lite;
using Leopotam.EcsLite;
using UnityEngine;

namespace Client.Source
{
    public class TestLite : MonoBehaviour
    {
        [SerializeField]
        private int numCachedEntities;

        private EcsSystems _liteSystems;
        private EcsWorld _liteWorld;
        
        public EcsWorld World => _liteWorld;

        private void Awake()
        {
            var config = new EcsWorld.Config
            {
                Entities = numCachedEntities
            };
            
            _liteWorld = new EcsWorld(config);
            _liteSystems = new EcsSystems(_liteWorld);

            _liteSystems.Add(new StartTestLiteSystem());
            _liteSystems.Add(new AddVisualLiteSystem());
            _liteSystems.Add(new RemoveVisualLiteSystem());
            _liteSystems.Add(new DestroyEntityLiteSystem());
            _liteSystems.Add(new FinishTestLiteSystem());

            _liteSystems.DelHere<TestStartEvent>();
        }
        
        private void Start()
        {
            _liteSystems.Init();
        }

        private void Update()
        {
            _liteSystems.Run();
        }
    }
}