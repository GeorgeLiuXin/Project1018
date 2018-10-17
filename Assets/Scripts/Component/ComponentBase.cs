using System;
using System.Collections.Generic;
using UnityEngine;

namespace XWorld
{
    public class ComponentBase : MonoBehaviour, IComponent
    {
        public ActorObj Owner
        {
            get;
            set;
        }

        void Awake()
        {
            InitComponent();
        }

        void Start()
        {
            OnComponentReadyToStart();
            OnComponentStart();
        }

        void OnDestroy()
        {
            OnPreDestroy();
        }

        public virtual void SetOwner(ActorObj logicObject)
        {
            this.Owner = logicObject;
        }

        protected virtual void InitComponent()
        {

        }

        public virtual void OnComponentReadyToStart()
        {

        }

        public virtual void OnComponentStart()
        {

        }

        public virtual void OnPreDestroy()
        {

        }
    }
}