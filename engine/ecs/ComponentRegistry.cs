using System.Collections.Generic;
using System;

namespace engine.ecs
{
    public abstract class BaseComponent
    {
        private int entityID;

        public void setEntityHandle(int entityID)
        {
            this.entityID = entityID;
        }

        public int getEntityHandle()
        {
            return entityID;
        }
    }

    public static class ComponentRegistry
    {
        private static Dictionary<Type, List<BaseComponent>> componentSuperList;

        public static void register(BaseComponent component)
        {
            checkIfInitialized();

            if(!componentSuperList.ContainsKey(component.GetType()))
            {
                componentSuperList.Add(component.GetType(), new List<BaseComponent>());
            }

            componentSuperList[component.GetType()].Add(component);
        }

        public static void removeFromRegistry(BaseComponent component)
        {
            if(checkIfInitialized())
            {
                Console.WriteLine("Line 48: Weird path in removeFromRegistry");
                return;
            }

            componentSuperList[component.GetType()].Remove(component);
        }

        public static List<BaseComponent> getComponentList(Type type)
        {
            checkIfInitialized();

            try
            {
                return componentSuperList[type];
            }
            catch(KeyNotFoundException)
            {
                return null;
            }
        }

        private static bool checkIfInitialized()
        {
            if(componentSuperList == null)
            {
                componentSuperList = new Dictionary<Type, List<BaseComponent>>();
                return true;
            }

            return false;
        }
    }
}