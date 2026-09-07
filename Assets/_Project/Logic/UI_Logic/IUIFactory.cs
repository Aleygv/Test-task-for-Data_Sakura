using Cysharp.Threading.Tasks;

namespace _Project.Logic.UI_Logic
{
    public interface IUIFactory
    {
        UniTask<AnimalViewCounter> CreateAnimalCounterWindow();
    }
}