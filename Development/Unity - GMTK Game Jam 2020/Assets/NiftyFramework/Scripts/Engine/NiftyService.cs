namespace NiftyFramework
{
    public abstract class NiftyService
    {
        public delegate void OnLoaded();
        public delegate void OnReady();

        public abstract void Init(OnReady onReady);
    }
}