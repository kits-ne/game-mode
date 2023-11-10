﻿using System;
using UniBloc;
using UniBloc.Widgets;
using UnityEngine;
using VContainer;

namespace GameMode.VContainer.Bloc
{
    [DefaultExecutionOrder(ConfigurableMonoBehaviour.ExecutionOrder)]
    public abstract class ConfigurableBlocWidget<T, TBloc, TEvent, TState> : BlocWidget<TBloc, TEvent, TState>,
        IConfigurable
        where T : class
        where TBloc : Bloc<TEvent, TState>, new()
        where TEvent : class, IEquatable<TEvent>
        where TState : IEquatable<TState>
    {
        protected override void Awake()
        {
            base.Awake();
            SceneContext.Register(gameObject.scene, this);
        }

        public virtual void Configure(IContainerBuilder builder)
        {
            var instance = GetRegisterInstance();
            builder.RegisterInstance(instance);
        }

        protected T GetRegisterInstance()
        {
            if (this is not T instance)
            {
                throw new Exception($"{gameObject.name} is not {typeof(T).Name}");
            }

            return instance;
        }

        public abstract class Scoped : ConfigurableBlocWidget<T, TBloc, TEvent, TState>
        {
            public override void Configure(IContainerBuilder builder)
            {
                var instance = GetRegisterInstance();
                builder.Register(_ => instance, Lifetime.Scoped);
            }
        }
    }
}