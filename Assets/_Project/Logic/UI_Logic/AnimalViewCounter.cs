using TMPro;
using UnityEngine;
using Zenject;

namespace _Project.Logic.UI_Logic
{
    public class AnimalViewCounter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI preyCounter;
        [SerializeField] private TextMeshProUGUI predatorCounter;

        public void UpdatePreyCounter(int amount)
        {
            preyCounter.text = amount.ToString();
        }

        public void UpdatePredatorCounter(int amount)
        {
            predatorCounter.text = amount.ToString();
        }

        public class Fabric : PlaceholderFactory<AnimalViewCounter>
        {
        }
    }
}