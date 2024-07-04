using UnityEngine;

namespace _Project.Scripts
{
    public class TransformSwayMovement : MonoBehaviour 
    {
        [SerializeField] private new Transform transform;
        // The sway's deviation from the original position in the y-axis.
        [SerializeField] private float swayYAmplitude = 2f;
        // A factor to control the time it takes for the sway animation to complete.
        // (This is the angular frequency of the y-axis oscillation)
        [SerializeField] private float swayYFrequency = 3f;
        // Factor to scale the sway movements, perhaps unique to different situations (crouching).
        [SerializeField] private float swayScale = 100f;
        // Time control for the sway. Setting it to zero disables the sway.
        [SerializeField] private float swayLerpSpeed = 10f;
        // Fields to control the x-axis oscillations (not currently used).
        [SerializeField] private float swayXAmplitude = 1f;
        [SerializeField] private float swayXFrequency = 1f;
        // The oscillations are time-dependent, so t = 0 is initial.
        private float _swayTime = 0f;
        // Vector representing the sway movement.
        private Vector3 _swayPosition;
        // Vector representing the origin of the given transform.
        private Vector3 _startPosition;

        void Awake()
        {
            _startPosition = transform.localPosition;
        }

        void Update() 
        {
            CalculateSway();
        }

        private void CalculateSway()
        {
            Vector3 targetPosition = SwayCurve(_swayTime, swayXAmplitude, swayXFrequency, swayYAmplitude, swayYFrequency) / swayScale;

            _swayPosition = Vector3.Lerp(_swayPosition, targetPosition, Time.smoothDeltaTime * swayLerpSpeed);

            _swayTime += Time.deltaTime;

            transform.localPosition = _startPosition + _swayPosition;
        }
        // This function returns an instantaneous vector representing the following equations:
        // f(t) = A * sin(a*t + d) and g(t) = B * sin(b*t)
        // Together, these equations describe time-dependent oscillations in the x and y axes at different angular frequencies.
        private Vector3 SwayCurve(float Time, float curveXAmplitude, float curveXFrequency, float curveYAmplitude, float curveYFrequency, float apparentRotation = Mathf.PI) 
        {
            float curveX = curveXAmplitude * Mathf.Sin(curveXFrequency * Time);
            float curveY = curveYAmplitude * Mathf.Sin(curveYFrequency * Time + apparentRotation);
            return new Vector3(curveX, curveY);
        }

    }
}