using System.Collections.Generic;

namespace engine.ecs
{
    public class Entity
    {
        private static class EntityRegistry
        {
            private static int entityCounter = 0;

            public static int getEntityID()
            {
                return entityCounter++;
            }
        }

        private int entityID;
        private bool isListed;
        private List<BaseComponent> components;

        public Entity(params BaseComponent[] comps)
        {
            entityID = EntityRegistry.getEntityID();
            isListed = true;
            components = new List<BaseComponent>();

            foreach(BaseComponent comp in comps)
            {
                addComponent(comp);
            }
        }

        public void addComponent(BaseComponent component)
        {
            components.Add(component);
            component.setEntityHandle(entityID);
            ComponentRegistry.register(component);
        }

        public void removeComponent(BaseComponent component)
        {
            components.Remove(component);
            ComponentRegistry.removeFromRegistry(component);
        }

        public void unlist()
        {
            if(!isListed)
            {
                return;
            }

            for(int i = 0; i < components.Count; i++)
            {
                ComponentRegistry.removeFromRegistry(components[i]);
            }

            isListed = false;
        }

        public void relist()
        {
            if(isListed)
            {
                return;
            }

            for(int i = 0; i < components.Count; i++)
            {
                ComponentRegistry.register(components[i]);
            }

            isListed = true;
        }

        public int size()
        {
            return components.Count;
        }

        public int getID()
        {
            return entityID;
        }

        public BaseComponent this[int key]
        {
            get
            {
                return components[key];
            }
        }
    }
}