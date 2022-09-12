using System.Collections;
using UnityEngine;

namespace Stupesoft.Doors
{
    public class SimpleDoor : MonoBehaviour
    {
        [Header("Target object")] [Tooltip("Automatically uses an object from LeafRoot")] [SerializeField]
        private Transform _rotatingLeaf;

        [Header("Main")] [SerializeField] private DoorState _state = DoorState.Close;
        [SerializeField] private AnimationCurve _animationCurve;
        [SerializeField] private float _duration = 1.0f;
        [Range(-180, 180)] [SerializeField] private float _openAngle = 90.0f;
        [Header("Audio")] [SerializeField] private AudioClip _openingClip;
        [SerializeField] private AudioClip _closingClip;
        [Header("Optional")] [SerializeField] private AudioSource _audioSource;
        private Coroutine _rotateCoroutine;

        private void Awake()
        {
            AssignLeaf();

            if (!_audioSource)
                _audioSource = gameObject.AddComponent<AudioSource>();
        }

        public void Toggle()
        {
            var currentAngle = GetCurrentAngle();
            if (GetDoorState(currentAngle) == DoorState.Close)
                Open();
            else if (GetDoorState(currentAngle) == DoorState.Open)
                Close();
        }

        public void Open()
        {
            var currentAngle = GetCurrentAngle();

            if (GetDoorState(currentAngle) == DoorState.Open)
                return;

            if (_rotateCoroutine != null)
                StopCoroutine(_rotateCoroutine);

            PlaySound(_closingClip);
            _rotateCoroutine = StartCoroutine(Rotate(currentAngle, _openAngle));
        }

        public void Close()
        {
            var currentAngle = GetCurrentAngle();

            if (GetDoorState(currentAngle) == DoorState.Close)
                return;

            if (_rotateCoroutine != null)
                StopCoroutine(_rotateCoroutine);

            PlaySound(_openingClip);
            _rotateCoroutine = StartCoroutine(Rotate(currentAngle, 0));
        }

        private void OnValidate()
        {
            AssignLeaf();

            switch (_state)
            {
                case DoorState.Open:
                    _rotatingLeaf.transform.rotation = Quaternion.Euler(0, _openAngle, 0);
                    break;
                case DoorState.Close:
                    _rotatingLeaf.transform.rotation = Quaternion.identity;
                    break;
            }
        }

        private IEnumerator Rotate(float start, float end)
        {
            for (float i = 0; i < 1; i += Time.deltaTime / _duration)
            {
                _rotatingLeaf.transform.rotation = Quaternion.Lerp(
                    Quaternion.Euler(0, start, 0),
                    Quaternion.Euler(0, end, 0),
                    _animationCurve.Evaluate(i));

                yield return null;
            }

            _rotatingLeaf.transform.rotation = Quaternion.Euler(0, end, 0);
            _rotateCoroutine = null;
        }

        private float GetCurrentAngle()
        {
            float currentAngle = Quaternion.Angle(Quaternion.identity, _rotatingLeaf.transform.rotation);
            currentAngle *= _openAngle > 0 ? 1 : -1;
            return currentAngle;
        }

        private void AssignLeaf()
        {
            if (!_rotatingLeaf)
                _rotatingLeaf = transform;
        }

        private void PlaySound(AudioClip clip)
        {
            _audioSource.clip = clip;
            _audioSource.Play();
        }

        private DoorState GetDoorState(float currentAngle)
        {
            if (Mathf.Approximately(0, currentAngle))
                return DoorState.Close;

            if (Mathf.Approximately(_openAngle, currentAngle))
                return DoorState.Open;

            return DoorState.Undefined;
        }

        private enum DoorState
        {
            Undefined,
            Open,
            Close,
        }
    }
}