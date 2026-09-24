using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Logic.UILogic
{
    public class TastyLabelView : MonoBehaviour
    {
        [SerializeField] private GameObject _tastyLabelObject;
        [SerializeField] private float _displayDuration = 1.0f;
        
        private Camera _camera;

        public void ShowTasty()
        {
            ShowTastyAsync().Forget();
        }

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void LateUpdate()
        {
            if (_tastyLabelObject != null && _tastyLabelObject.activeSelf && _camera != null)
            {
                _tastyLabelObject.transform.rotation = _camera.transform.rotation;
            }
        }

        private async UniTaskVoid ShowTastyAsync()
        {
            _tastyLabelObject.SetActive(true);

            await UniTask.Delay(System.TimeSpan.FromSeconds(_displayDuration),
                cancellationToken: this.GetCancellationTokenOnDestroy());

            if (_tastyLabelObject != null)
            {
                _tastyLabelObject.SetActive(false);
            }
        }
    }
}