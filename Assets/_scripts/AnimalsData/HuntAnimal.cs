using Cinemachine;
using UnityEngine;
using DG.Tweening;

public class HuntAnimal : HunterBase, IAttacker
{
    [SerializeField] private CinemachineVirtualCamera _camera;
    private bool _canInput = false;
    
    private IInput _input;
    private PathCreator _pathCreator;
    private HuntManager _hunterManager;
    private float _inputX;
    private float _inputY;
    private float _distOfset;
    private Vector3 _jumpLandPosition = Vector3.zero;

    [SerializeField] private float _jumpDuration = 2f;
    [SerializeField] private float _inputModifire = 0.004f;
    [SerializeField] private float _xAmplitude = 2f;
    [SerializeField] private float _yAmplitude = 3f;

    public float Damage { get => AnimalData.Damage; set => AnimalData.Damage = value; }

    protected override void Awake()
    {
        base.Awake();
        _input = new MobileDesktopInput();
        _pathCreator = GetComponentInChildren<PathCreator>();
        _hunterManager = FindObjectOfType<HuntManager>();
    }

    protected override void Start()
    {
        base.Start();
        _distOfset = _hunterManager.DistanceOfset;
    }

    protected override void Update()
    {
        base.Update();
        if (_canInput)
        {
            if (_input.OnTouch())
            {
                _pathCreator.EnableLine();
            }

            if (_input.OnHold())
            {
                _inputX = _input.GetHorizontalInput() * _inputModifire;
                _inputY = _input.GetVerticalInput() * _inputModifire;

                _inputX = Mathf.Clamp(_inputX, -_xAmplitude, _xAmplitude);
                _inputY = Mathf.Clamp(_inputY, -_yAmplitude, _yAmplitude);

                _jumpLandPosition = _transform.position + Vector3.forward * _distOfset + (_transform.forward * _inputY + _transform.right * _inputX);
                _pathCreator.UpdateLine(_jumpLandPosition);
            }

            if (_input.OnRelease())
            {
                _pathCreator.DisableLine();
                Jump(_pathCreator.GetPositions().ToArray());
            }
        }
    }

    private void Jump(Vector3[] positions)
    {
        _canInput = false;
        base.Jump();
        
        _transform.DOPath(positions, _jumpDuration)
            .SetEase(Ease.Linear)
            .Play()
            .OnComplete(()=>
            {
                HitPrey();
                ActivateRagdoll();
            });
    }

    private void HitPrey()
    {
        GetComponent<Collider>().enabled = false;
        DOTween.KillAll();
        _hunterManager.ChangeHunter(this);
    }

    private void HitEffect()
    {
        ActivateRagdoll(false);
    }

    public void Move()
    {
        base.Run();
    }

    public void Activate()
    {
        _camera.Priority = int.MaxValue;
        _canInput = true;
    }

    public void Deactivate()
    {
        _camera.Priority = 0;
        _canInput = false;
    }

    public void Attack()
    {
        HitPrey();
        HitEffect();
    }
}