using System;

namespace ProjectVS.Util
{
    public interface IPoolable
    {
        Action OnSpawn { get; set; }
        Action OnDespawn { get; set; }

        void OnSpawned();
        void OnDespawned();
    }
}
