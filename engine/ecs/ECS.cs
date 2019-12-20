using System;
using System.Collections.Generic;

namespace engine.ecs
{
    public class ECS
    {
        private List<Entity> entities;

        public ECS()
        {
            entities = new List<Entity>();
        }

        public Entity makeEntity(params BaseComponent[] comps)
        {
            Entity newEntity = new Entity(comps);
            entities.Add(newEntity);
            return newEntity;
        }

        public Entity addEntity(Entity entity)
        {
            entities.Add(entity);
            return entity;
        }

        public void removeEntity(Entity entity)
        {
            entities.Remove(entity);
        }

        public void updateSystems(SystemList systems, float delta)
        {
            for(int i = 0; i < systems.size(); i++)
            {
                BaseSystem curSystem = systems[i];
                curSystem.preComponentUpdate(delta);

                if(curSystem.getComponentTypes().Count == 1)
                {
                    List<BaseComponent> components = ComponentRegistry.getComponentList(curSystem.getComponentTypes()[0]);

                    if(components == null)
                    {
                        continue;
                    }

                    foreach(BaseComponent comp in components)
                    {
                        curSystem.componentUpdate(new BaseComponent[] {comp}, delta);
                    }
                }
                else
                {
                    updateSystemsWithMultipleComponents(curSystem, delta);
                }

                curSystem.postComponentUpdate(delta);
            }
        }

        private int findMinListIndex(List<List<BaseComponent>> components, List<Flag> flags)
        {
            int index = 0;
            int min = int.MaxValue;

            for(int i = 0; i < components.Count; i++)
            {
                if(components[i] == null)
                {
                    continue;
                }

                if(components[i].Count < min && flags[i] == Flag.FLAG_NONE)
                {
                    index = i;
                    min = components.Count;
                }
            }

            return index;
        }

        private BaseComponent findComponentWithEntityHandle(List<BaseComponent> compList, int entityID)
        {
            BaseComponent res = null;

            foreach(BaseComponent component in compList)
            {
                if(entityID == component.getEntityHandle())
                {
                    res = component;
                    break;
                }
            }

            return res;
        }

        private void updateSystemsWithMultipleComponents(BaseSystem system, float delta)
        {
            List<Type> types = system.getComponentTypes();
            List<Flag> flags = system.getComponentFlags();

            List<List<BaseComponent>> components = new List<List<BaseComponent>>();

            for(int i = 0; i < types.Count; i++)
            {
                components.Add(ComponentRegistry.getComponentList(types[i]));
            }

            int minIndex = findMinListIndex(components, flags);

            List<BaseComponent> compList = components[minIndex];

            for(int i = 0; i < compList.Count; i++)
            {
                int entityID = compList[i].getEntityHandle();
                List<BaseComponent> compParams = new List<BaseComponent>();
                bool isValid = true;

                for(int j = 0; j < components.Count; j++)
                {
                    //check if type is good
                    if(components[j] == null && flags[j] == Flag.FLAG_NONE)
                    {
                        isValid = false;
                        break;
                    }
                    else if(components[j] == null && flags[j] == Flag.FLAG_OPTIONAL)
                    {
                        compParams.Add(null);
                        continue;
                    }

                    //find component in the same entity
                    BaseComponent component = findComponentWithEntityHandle(components[j], entityID);

                    //component could not be found and is needed don't update system
                    if(component == null && flags[j] == Flag.FLAG_NONE)
                    {
                        isValid = false;
                        break;
                    }

                    compParams.Add(component);
                }

                if(isValid)
                {
                    system.componentUpdate(compParams.ToArray(), delta);
                }
            }
        }
    }
}