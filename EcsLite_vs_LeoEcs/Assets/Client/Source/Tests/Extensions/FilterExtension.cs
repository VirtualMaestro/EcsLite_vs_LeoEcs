using Leopotam.EcsLite;

namespace Client.Source.Tests.Extensions
{
    public static class FilterExtension
    {
        public static int First(this EcsFilter filter)
        {
            foreach (var entityId in filter)
                return entityId;

            return -1;
        }
    }
}