using System.Collections;
using Project.Dev.Infrastructure.GameStateMachine.Interface;
using UnityEngine;
using Zenject;

namespace Project.Dev.Infrastructure.Factories
{
    public class StateFactories
    {
        private readonly DiContainer _container;

        public StateFactories(DiContainer container)
        {
            _container = container;
        }
    
        public T CreateState<T>() where T : IExitableState => 
            _container.Resolve<T>();
    }
}

