using System.Collections.Generic;

namespace engine.ecs
{
    public static class EntityRegistry
    {
        private static int entityCounter = 0;

        public static int getEntityID()
        {
            return entityCounter++;
        }
    }

    public class Entity
    {
        private int entityID;
        private List<BaseComponent> components;

        public Entity(params BaseComponent[] comps)
        {
            entityID = EntityRegistry.getEntityID();
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

        public int size()
        {
            return components.Count;
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