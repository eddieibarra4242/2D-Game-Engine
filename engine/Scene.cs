using engine.ecs;

namespace engine
{
    public abstract class Scene
    {
        private ECS ecs;
        private SystemList sceneList;
        private SystemList simulation;
        private SystemList collisionDetection;
        private SystemList collisionResponse;
        private SystemList renderPipeline;

        public Scene()
        {
            ecs = new ECS();
            sceneList = new SystemList();
            simulation = new SystemList();
            collisionDetection = new SystemList();
            collisionResponse = new SystemList();
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

        public void addSimulatorSystem(BaseSystem simulator)
        {
            simulation.addSystem(simulator);
        }

        public void addCollisionDetectionSystem(BaseSystem collisionDetector)
        {
            collisionDetection.addSystem(collisionDetector);
        }

        public void addCollisionResponseSystem(BaseSystem responseSystem)
        {
            collisionResponse.addSystem(responseSystem);
        }

        public SystemList getRenderPipeLine()
        {
            return renderPipeline;
        }

        public bool engineUpdate(double delta)
        {
            ecs.updateSystems(sceneList, delta);
            bool result = update(delta);

            ecs.updateSystems(simulation, delta);
            ecs.updateSystems(collisionDetection, delta);
            ecs.updateSystems(collisionResponse, delta);

            return result;
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