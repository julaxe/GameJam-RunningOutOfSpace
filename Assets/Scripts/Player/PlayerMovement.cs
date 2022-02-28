using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        public GameObject pauseScreen;
        private NavMeshAgent _agent;
        private Animator _animator;
        private RaycastHit _hit;

        private bool _isMoving;
        private bool _isBlocking;
        private bool _isPaused;

        private readonly int _isMovingHash = Animator.StringToHash("isMoving");
        private readonly int _attackHash = Animator.StringToHash("Attack");
        private readonly int _isBlockingHash = Animator.StringToHash("isBlocking");

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (!_agent.hasPath || _agent.isStopped)
            {
                _isMoving = false;
                _animator.SetBool(_isMovingHash, _isMoving);
            }
        }
        
        public void OnMove(InputValue value)
        {
            if (_isBlocking) return;
            
            _isMoving = true;
            SetDestinationToMousePosition();
            _animator.SetBool(_isMovingHash, _isMoving);
            _agent.isStopped = !_isMoving;
        }

        public void OnAttack(InputValue value)
        {
            _agent.isStopped = true;
            _animator.SetTrigger(_attackHash);
        }

        public void OnBlock(InputValue value)
        {
            _agent.isStopped = true;
            _isBlocking = value.isPressed;
            _animator.SetBool(_isBlockingHash, _isBlocking);
        }

        public void OnPause(InputValue value)
        {
            if (!_isPaused)
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }

        public void Pause()
        {
            _isPaused = true;
            pauseScreen.SetActive(true);
            Time.timeScale = 0.0f; 
        }
        public void Resume()
        {
            _isPaused = false;
            pauseScreen.SetActive(false);
            Time.timeScale = 1.0f;
        }
        
        private void SetDestinationToMousePosition()
        {
            if (Camera.main is { })
            {
                Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
                if (Physics.Raycast(ray, out _hit))
                {
                    _agent.SetDestination(_hit.point);
                }
            }
        }
        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(_hit.point, 0.5f);
        }
    }
}
