using System.Runtime.CompilerServices;
using Leopotam.EcsLite;

namespace Client.Source.Extensions
{
    public static class FilterExtension
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int First(this EcsFilter filter)
        {
            foreach (var entityId in filter)
                return entityId;

            return -1;
        }
    }
}