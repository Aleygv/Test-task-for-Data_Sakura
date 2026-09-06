using _Project.Logic.Extensions;

namespace _Project.Logic.Entities
{
    public interface IMovable
    {
        void Move(LightVector3 direction);
    }
}