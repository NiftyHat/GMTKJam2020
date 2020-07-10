using System;
using System.Collections.Generic;

namespace NiftyFramework
{
    public class ServiceSet<TServiceBase>
    {
        private readonly Dictionary<Type, TServiceBase> _dictionary;

        public ServiceSet()
        {
            _dictionary = new Dictionary<Type, TServiceBase>();
        }

        public TService Get<TService>() where TService : TServiceBase, new()
        {
            Type serviceKey = typeof(TService);
            if (!_dictionary.ContainsKey(serviceKey))
            {
                return Add<TService>();
            }
            return (TService)_dictionary[serviceKey];
        }
        
        public TService Add<TService>() where TService : TServiceBase, new()
        {
            Type serviceKey = typeof(TService);
            var instance = new TService();
            _dictionary[serviceKey] = instance;
            return instance;
        }
    }
}