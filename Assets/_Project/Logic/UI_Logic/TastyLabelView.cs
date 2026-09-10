using Cysharp.Threading.Tasks;
using UnityEngine;
namespace _Project.Logic.UI_Logic
{
    public class TastyLabelView : MonoBehaviour
    {
        [SerializeField] private GameObject tastyLabelObject;
        [SerializeField] private float displayDuration = 1.0f;
        private Camera _camera;
        private void Awake()
        {
            _camera = Camera.main;
        }
        // LateUpdate вызывается ПОСЛЕ того, как змейка повернулась в своем Update
        private void LateUpdate()
        {
            if (tastyLabelObject != null && tastyLabelObject.activeSelf && _camera != null)
            {
                // Текст всегда повернут ровно в плоскость экрана камеры
                tastyLabelObject.transform.rotation = _camera.transform.rotation;
            }
        }
        public void ShowTasty()
        {
            ShowTastyAsync().Forget();
        }
        private async UniTaskVoid ShowTastyAsync()
        {
            tastyLabelObject.SetActive(true);
            
            await UniTask.Delay(System.TimeSpan.FromSeconds(displayDuration),
                cancellationToken: this.GetCancellationTokenOnDestroy());
            
            if (tastyLabelObject != null)
            {
                tastyLabelObject.SetActive(false);
            }
        }
    }
}