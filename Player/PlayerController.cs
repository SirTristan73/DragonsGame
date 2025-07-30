using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    // Панятна
    Rigidbody _rb;
    public Shoot _weapon;


    // Чтоб знало куда за анимациями идти
    public PlayerAnimation _animator;

    // Для смены модели
    public GameObject _playerCurrentSkin;

    // Панятна
    private Vector2 _moveInput;

    // В зависимости от скина
    private float _playerSpeed;




    /// <summary>
    /// ///////////////////////////////////////


    private void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }

        else
        {
            Instance = this;
        }


        TimeManager.Instance.RegisterFixedUpdateListener(OnFixedUpdate);

        InputManager.Instance._move += GetMovementInput;
        InputManager.Instance._fire += FireAction;
        InputManager.Instance._onEscape += OnPausePressed;

        SetPlayerModel(PlayerChoise.Instance._avalibleSkins[PlayerChoise.Instance._currentChoise]);

        _rb = GetComponent<Rigidbody>();


    }


    private void OnDisable() // Теперь полезно
    {
        TimeManager.Instance.UnregisterFixedUpdateListener(OnFixedUpdate);

        InputManager.Instance._move -= GetMovementInput;
        InputManager.Instance._fire -= FireAction;
        InputManager.Instance._onEscape -= OnPausePressed;
    }


    void OnFixedUpdate(float deltaTime)
    {
        Vector3 movementVector = new Vector3(_moveInput.x, 0, 0) * _playerSpeed * deltaTime;
        _rb.linearVelocity = movementVector;

    }


    /// Потом зарефакторю чтоб анимации зависили не от ввода, а от самого движения
    private void GetMovementInput(Vector2 input)
    {
        _moveInput = input;
        switch (input.x)
        {
            case 1:
                _animator.MoveLeft();
                break;

            case -1:
                _animator.MoveRight();
                break;

            default:
                _animator.DefaultFlight();
                break;
        }
    }


    // Тута стрелять
    private void FireAction(float fireInput)
    {
        if (fireInput > 0)
        {
            _animator.FireAnimation(true);
            _weapon.IsFireActive(true);
        }

        else if (fireInput == 0)
        {
            _animator.FireAnimation(false);
            _weapon.IsFireActive(false);
        }
    }


    // Самое интересное с чем я трахался пару дней
    public void SetPlayerModel(CreatureStats skin)
    {
        if (_playerCurrentSkin != null)
        {
            Destroy(_playerCurrentSkin);
        }

        _playerCurrentSkin = Instantiate
        (skin._model, transform.position, Quaternion.identity, this.transform);

        _playerSpeed = skin.BaseStats._speed;

        var animatorComp = _playerCurrentSkin.GetComponent<Animator>();
        _animator.GetAnimator(animatorComp);

    }


    private void OnPausePressed(float escButton)
    {
        if (escButton > 0)
        {
            GameManager.Instance.PauseIsPressed();
        }

    }


}
