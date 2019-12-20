using engine.ecs;
using engine.math;

namespace engine.components
{
    public class TransformComponent : BaseComponent
    {
        public Transform transform;
        
        public TransformComponent(Transform transform)
        {
            this.transform = transform;
        }
    }
}