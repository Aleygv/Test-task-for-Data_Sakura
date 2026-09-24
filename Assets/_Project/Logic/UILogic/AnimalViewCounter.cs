using TMPro;
using UnityEngine;
using Zenject;

namespace _Project.Logic.UILogic
{
    public class AnimalViewCounter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _preyCounter;
        [SerializeField] private TextMeshProUGUI _predatorCounter;

        public void UpdatePreyCounter(int amount)
        {
            _preyCounter.text = amount.ToString();
        }

        public void UpdatePredatorCounter(int amount)
        {
            _predatorCounter.text = amount.ToString();
        }

        public class Fabric : PlaceholderFactory<AnimalViewCounter>
        {
        }
    }
}