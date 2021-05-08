using Client.Source.Components;
using Client.Source.Systems.Leo;
using Client.Source.Systems.Lite;
using Leopotam.Ecs;
using UnityEngine;
using EcsSystems = Leopotam.EcsLite.EcsSystems;
using EcsWorld = Leopotam.EcsLite.EcsWorld;

namespace Client.Source
{
    public class Entry : MonoBehaviour
    {
        public static bool IsLiteEnable;
        private EcsSystems _liteSystems;
        private static EcsWorld _liteWorld;
        public static EcsWorld LiteWorld => _liteWorld;

        public static bool IsLeoEnable;
        private Leopotam.Ecs.EcsSystems _leoSystems;
        private static Leopotam.Ecs.EcsWorld _leoWorld;
        public static Leopotam.Ecs.EcsWorld LeoWorld => _leoWorld;

        private void Awake()
        {
            _InitLite();
            _InitLeo();

            DontDestroyOnLoad(this);
        }

        private void _InitLite()
        {
            var config = new EcsWorld.Config()
            {
                Entities = 1_000_000
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

        private void _InitLeo()
        {
            var config = new EcsWorldConfig()
            {
                WorldEntitiesCacheSize = 1_000_000
            };
            
            _leoWorld = new Leopotam.Ecs.EcsWorld(config);
            _leoSystems = new Leopotam.Ecs.EcsSystems(_leoWorld);
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
            _liteSystems.Init();
            _leoSystems.Init();
        }

        private void Update()
        {
            if (IsLiteEnable)
                _liteSystems.Run();

            if (IsLeoEnable)
                _leoSystems.Run();
        }
    }
}