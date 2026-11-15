using Item;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Player;

public class BouteilleAlcool : BasePickableItem
{
    [Header("View")]
    [SerializeField] private GameObject _goutteAlcool;
    [SerializeField] private float _stepInstantiate = 0.1f;
    [SerializeField] private Transform _goutteDropInstantiatePos;
    private float _buffer = 0;

    [Header("Settings")] 
    [SerializeField] private float _alcoolQuantity = 100;
    [SerializeField] private float _alcoolLooseBySecond = 25;
    private float _alcoolAmount = 0;

    [Header("Rotate")] 
    [SerializeField] private float _rotateStep = 5;

    [SerializeField] private float _warningAngleCaVaDeborder = 90;
    float _rotateAngle;

    private PlayerInputs _inputs;

    void OnEnable()
    {
        _inputs = new PlayerInputs();
        
        _inputs.Enable();

        _inputs.Gameplay.Scroll.performed += UpdateRotation;
        _inputs.Gameplay.RightClick.performed += Drink;
        
        _alcoolAmount = _alcoolQuantity;
    }

    void OnDisable()
    {
        _inputs.Gameplay.Scroll.performed -= UpdateRotation;
        _inputs.Gameplay.RightClick.performed -= Drink;
        
        _inputs.Disable();
    }

    private void UpdateRotation(InputAction.CallbackContext obj)
    {
        if(!_isInHand)
            return;
        
        float d = obj.ReadValue<float>();
        
        _rotateAngle = (_rotateAngle + _rotateStep * d) % 360f;
        transform.rotation = Quaternion.Euler(0f, 0f, _rotateAngle);
    }
    
    private void Drink(InputAction.CallbackContext obj)
    {
        if (!_isInHand) return;

        Player.Player.instance.StartDrunk();
        
        Destroy(gameObject);
    }

    protected override void Update()
    {
        base.Update();

        if (Mathf.Abs(_rotateAngle) > _warningAngleCaVaDeborder)
        {
            if (_buffer < _stepInstantiate)
            {
                _buffer += Time.deltaTime;

                if (_buffer >= _stepInstantiate)
                {
                    _buffer = 0;
                    
                    Instantiate(_goutteAlcool, _goutteDropInstantiatePos.position, _goutteDropInstantiatePos.rotation);
                }
            }
            
            _alcoolAmount -= Time.deltaTime * _alcoolLooseBySecond;
        }

        if (_alcoolAmount <= 0)
        {
            Destroy(gameObject);
        }
    }
}
