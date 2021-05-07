// ----------------------------------------------------------------------------
// The MIT License
// Lightweight ECS framework https://github.com/Leopotam/ecslite
// Copyright (c) 2021 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using System.Collections.Generic;
using System.Runtime.CompilerServices;

#if ENABLE_IL2CPP
using Unity.IL2CPP.CompilerServices;
#endif

namespace Leopotam.EcsLite {
    public interface IEcsSystem { }

    public interface IEcsInitSystem : IEcsSystem {
        void Init (EcsSystems systems);
    }

    public interface IEcsRunSystem : IEcsSystem {
        void Run (EcsSystems systems);
    }

    public interface IEcsDestroySystem : IEcsSystem {
        void Destroy (EcsSystems systems);
    }

#if ENABLE_IL2CPP
    [Il2CppSetOption (Option.NullChecks, false)]
    [Il2CppSetOption (Option.ArrayBoundsChecks, false)]
#endif
    public sealed class EcsSystems {
        readonly EcsWorld _defaultWorld;
        readonly Dictionary<string, EcsWorld> _worlds;
        readonly List<IEcsSystem> _allSystems;
        readonly object _shared;
        IEcsRunSystem[] _runSystems;
        int _runSystemsCount;

        public EcsSystems (EcsWorld defaultWorld, object shared = null) {
            _defaultWorld = defaultWorld;
            _shared = shared;
            _worlds = new Dictionary<string, EcsWorld> (32);
            _allSystems = new List<IEcsSystem> (128);
        }

        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public T GetShared<T> () where T : class {
            return _shared as T;
        }

        public void Destroy () {
            for (var i = _allSystems.Count - 1; i >= 0; i--) {
                if (_allSystems[i] is IEcsDestroySystem destroySystem) {
                    destroySystem.Destroy (this);
#if DEBUG
                    // World.CheckForLeakedEntities ($"{destroySystem.GetType ().Name}.Destroy ()");
#endif
                }
            }
            _allSystems.Clear ();
            _runSystems = null;
        }

        public EcsSystems AddWorld (EcsWorld world, string name) {
            _worlds[name] = world;
            return this;
        }

        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public EcsWorld GetWorld (string name = null) {
            if (name == null) {
                return _defaultWorld;
            }
            _worlds.TryGetValue (name, out var world);
            return world;
        }

        public EcsSystems Add (IEcsSystem system) {
            _allSystems.Add (system);
            if (system is IEcsRunSystem) {
                _runSystemsCount++;
            }
            return this;
        }

        public void Init () {
            if (_runSystemsCount > 0) {
                _runSystems = new IEcsRunSystem[_runSystemsCount];
            }
            var runIdx = 0;
            foreach (var system in _allSystems) {
                if (system is IEcsInitSystem initSystem) {
                    initSystem.Init (this);
                }
                if (system is IEcsRunSystem runSystem) {
                    _runSystems[runIdx++] = runSystem;
                }
            }
        }

        public void Run () {
            for (int i = 0, iMax = _runSystemsCount; i < iMax; i++) {
                _runSystems[i].Run (this);
            }
        }

        public EcsSystems DelHere<T> (string worldName = null) where T : struct {
            return Add (new DelHereSystem<T> (GetWorld (worldName)));
        }
    }

#if ENABLE_IL2CPP
    [Il2CppSetOption (Option.NullChecks, false)]
    [Il2CppSetOption (Option.ArrayBoundsChecks, false)]
#endif
    sealed class DelHereSystem<T> : IEcsRunSystem where T : struct {
        readonly EcsFilter _filter;
        readonly EcsPool<T> _pool;

        public DelHereSystem (EcsWorld world) {
            _filter = world.Filter<T> ().End ();
            _pool = world.GetPool<T> ();
        }

        public void Run (EcsSystems systems) {
            foreach (var entity in _filter) {
                _pool.Del (entity);
            }
        }
    }
}