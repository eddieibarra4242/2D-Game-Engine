using engine.ecs;

namespace engine
{
    public abstract class Scene
    {
        private ECS ecs;
        private SystemList sceneList;
        private SystemList renderPipeline;

        public Scene()
        {
            ecs = new ECS();
            sceneList = new SystemList();
            renderPipeline = new SystemList();
        }

        public ECS getECS()
        {
            return ecs;
        }

        public void addSceneSystem(BaseSystem sceneSystem)
        {
            sceneList.addSystem(sceneSystem);
        }

        public SystemList getRenderPipeLine()
        {
            return renderPipeline;
        }

        public bool engineUpdate(double delta)
        {
            ecs.updateSystems(sceneList, delta);
            return update(delta);
        }

        public void engineRender(double delta)
        {
            ecs.updateSystems(renderPipeline, delta);
            render(delta);
        }

        public abstract bool update(double delta);
        public abstract void render(double delta);
    }
}