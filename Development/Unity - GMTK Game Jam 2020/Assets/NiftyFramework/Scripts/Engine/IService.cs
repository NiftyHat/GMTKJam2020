using System;

namespace NiftyFramework
{
    public interface IService
    {
        bool isLoaded { get; }
        void Load(Action<IService> onLoaded);
        void Unload();
    }
}