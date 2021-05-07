using Client.Source.Tests.Components;
using Client.Source.Tests.Systems.Leo;
using Client.Source.Tests.Systems.Lite;
using Leopotam.EcsLite;
using UnityEngine;

namespace Client.Source.Tests
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
            _liteWorld = new EcsWorld();
            _liteSystems = new EcsSystems(_liteWorld);

            _liteSystems.Add(new StartTestLiteSystem());
            _liteSystems.Add(new DestroyEntityLiteSystem());
            _liteSystems.Add(new FinishTestLiteSystem());

            _liteSystems.DelHere<TestStartEvent>();
        }

        private void _InitLeo()
        {
            _leoWorld = new Leopotam.Ecs.EcsWorld();
            _leoSystems = new Leopotam.Ecs.EcsSystems(_leoWorld);
            _leoSystems.Add(new StartTestLeoSystem());
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