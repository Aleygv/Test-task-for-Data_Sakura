using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
namespace _Project.Logic.Entities.AI_Movement
{
    public class JumpMovement : AnimalMovementBase
    {
        [Header("Frog Jump Settings")]
        [SerializeField] private float jumpDistance = 2.5f; // Фиксированная дистанция прыжка по ТЗ
        [SerializeField] private float jumpHeight = 1.0f;   // Высота подъема в прыжке
        [SerializeField] private float jumpDuration = 0.35f; // Длительность полета
        
        private Vector3 _generalTarget;
        private bool _isJumping;
        
        protected override void SetNewDestination()
        {
            if (_isJumping) return;
            // 1. Если общая цель на экране еще не выбрана или мы до нее почти допрыгали — выбираем новую
            if (_generalTarget == Vector3.zero || Vector3.Distance(transform.position, _generalTarget) < 1.5f)
            {
                _generalTarget = GetRandomPositionInCameraView(MainCamera, transform.position.y);
            }
            // 2. Считаем направление прыжка в сторону общей цели
            Vector3 jumpDirection = _generalTarget - transform.position;
            jumpDirection.y = 0;
            if (jumpDirection.sqrMagnitude < 0.01f)
            {
                jumpDirection = transform.forward;
            }
            else
            {
                jumpDirection.Normalize();
            }
            // 3. Точка приземления строго на фиксированном расстоянии вперед
            Vector3 targetLandingPos = transform.position + jumpDirection * jumpDistance;
            // 4. Проверяем точку на полигоне NavMesh и запускаем прыжок
            if (NavMesh.SamplePosition(targetLandingPos, out NavMeshHit hit, jumpDistance, NavMesh.AllAreas))
            {
                PerformJumpAsync(hit.position, jumpDirection).Forget();
            }
            else
            {
                // Если уперлись в край сетки/препятствие — сбрасываем цель, чтобы развернуться
                _generalTarget = Vector3.zero;
            }
        }
        private async UniTaskVoid PerformJumpAsync(Vector3 landingPosition, Vector3 direction)
        {
            _isJumping = true;
            agent.isStopped = true;
            agent.ResetPath();
            // Поворачиваем лягушку мордочкой в сторону прыжка
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(direction);
            }
            Vector3 startPos = transform.position;
            float elapsed = 0f;
            var ct = this.GetCancellationTokenOnDestroy();
            while (elapsed < jumpDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / jumpDuration);
                // Горизонтальное движение от старта до финиша
                Vector3 currentPos = Vector3.Lerp(startPos, landingPosition, t);
                // Парабола подъема по высоте: Y = 4 * height * t * (1 - t)
                // При t = 0 и t = 1 подъем = 0. В середине прыжка (t = 0.5) подъем максимальный (jumpHeight).
                currentPos.y += 4f * jumpHeight * t * (1f - t);
                transform.position = currentPos;
                await UniTask.Yield(PlayerLoopTiming.Update, ct);
            }
            // Приземляемся строго на полигон NavMesh
            agent.Warp(landingPosition);
            agent.isStopped = false;
            _isJumping = false;
        }
    }
}