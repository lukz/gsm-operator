using UnityEngine;


namespace BlendModeShader2D.Demo
{
    public class SimpleMouseMove2D : MonoBehaviour
    {
        public float moveSpeed = 0.1f;

        Vector2 _targetPos;


        void Start()
        {
            _targetPos = transform.position;
        }

        void Update()
        {
            if (Input.GetMouseButton(1))
            {
                _targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
        
            if (Vector2.Distance(transform.position, _targetPos) > 0.05f)
            {
                transform.position = Vector2.Lerp(transform.position, _targetPos, moveSpeed * Time.deltaTime);
            }
        }
    }
}
