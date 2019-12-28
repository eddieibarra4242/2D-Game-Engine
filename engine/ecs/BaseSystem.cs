using System;
using System.Collections.Generic;

namespace engine.ecs
{
    public class SystemList
    {
        private List<BaseSystem> systems;

        public SystemList()
        {
            systems = new List<BaseSystem>();
        }

        public void addSystem(BaseSystem system)
        {
            systems.Add(system);
        }

        public int size()
        {
            return systems.Count;
        }

        public void clear()
        {
            systems.Clear();
        }

        public BaseSystem this[int key]
        {
            get
            {
                return systems[key];
            }
        }
    }

    public enum Flag
    {
        NONE,
        OPTIONAL
    }

    public abstract class BaseSystem
    {
        private List<Type> componentTypes;
        private List<Flag> componentFlags;

        public BaseSystem()
        {
            componentTypes = new List<Type>();
            componentFlags = new List<Flag>();
        }

        public void addComponentType(Type type)
        {
            componentTypes.Add(type);
            componentFlags.Add(Flag.NONE);
        }

        public void addComponentType(Type type, Flag flag)
        {
            componentTypes.Add(type);
            componentFlags.Add(flag);
        }

        public List<Type> getComponentTypes()
        {
            return componentTypes;
        }

        public List<Flag> getComponentFlags()
        {
            return componentFlags;
        }

        public virtual void preComponentUpdate(double delta) {}

        public virtual void componentUpdate(BaseComponent[] components, double delta) {}

        public virtual void postComponentUpdate(double delta) {}
    }
}