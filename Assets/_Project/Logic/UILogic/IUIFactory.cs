using Cysharp.Threading.Tasks;

namespace _Project.Logic.UILogic
{
    public interface IUIFactory
    {
        UniTask<AnimalViewCounter> CreateAnimalCounterWindow();
    }
}