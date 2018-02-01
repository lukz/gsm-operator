using UnityEngine;
using UnityEngine.UI;


//public enum Debuff
//{
//    None,
//    Frozen,
//    Poison
//}

namespace BlendModeShader2D.Demo
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Slider))]
    public class HealthBarController : MonoBehaviour
    {
        public ImageBlendModeFX imageBlendModeFX;
        public int maxHealth = 255;
        public int health;
        public float healthChangeSpeed = 10f;
        public float healthBarColorChangeSpeed = 10f;


        Slider _slider;
        int _lastHealth;
        Color _healthBarColor = Color.red;

        void Start ()
        {
            _slider = GetComponent<Slider>();
            _lastHealth = health;
            _slider.value = _lastHealth / (float)maxHealth;
            _healthBarColor = imageBlendModeFX.mainColor;
        }	

        void Update ()
        {
            health = Mathf.Clamp(health, 0, maxHealth);
            if (_lastHealth != health)
            {
                _lastHealth = Mathf.RoundToInt(Mathf.Lerp(_lastHealth, health, healthChangeSpeed * Time.deltaTime));
                if (Mathf.Abs(_lastHealth - health) < 0.01f)
                {
                    _lastHealth = health;
                }
                _slider.value = _lastHealth / (float)maxHealth;
            }

            imageBlendModeFX.mainColor = Color.Lerp(imageBlendModeFX.mainColor, _healthBarColor, healthBarColorChangeSpeed * Time.deltaTime);                 
        }

        public void TakeDamage(int damage)
        {
            health -= damage;
        }

        public void RecoverHealth(int value)
        {
            health += value;
        }


        public void NormalState()
        {
            _healthBarColor = Color.red;
        }

        public void FrozenState()
        {
            _healthBarColor = Color.blue;
        }

        public void PoisonState()
        {
            _healthBarColor = Color.green;
        }
    }
}
