using DG.Tweening;
using UnityEngine;

public abstract class AnimalBase : MonoBehaviour
{
    [SerializeField] protected GameObject _baseVisual;
    [SerializeField] protected GameObject _ragdollVisual;
    [SerializeField] protected Rigidbody _ragdollMain;
    [SerializeField, Header("Smooth Settings")] protected float _punchForce = 0.3f;
    [SerializeField] protected float _punchDuration = 0.5f;
    [SerializeField] protected float _ragdollStartYVelocity = 60f;
    protected Transform _transform;
    protected Animator _animator;
    protected float _speed = 1f;
    protected bool _isMove = false;
    protected bool _enable = true;
    private int _idle_AnimKey;
    private int _run_AnimKey;
    private int _jump_AnimKey;

    protected virtual void Awake()
    {
        _transform = transform;
        AssembleAnimations();
    }

    protected virtual void Start()
    {
        Idle();
    }

    protected virtual void Update()
    {
        if (!_enable) return;
        if (_isMove) _transform.Translate(_transform.forward * _speed * Time.deltaTime);
    }

    public void ActivateRagdoll(bool addForce = true)
    {
        _enable = false;
        _baseVisual.SetActive(false);
        _ragdollVisual.SetActive(true);
        if (addForce) _ragdollMain.AddForce(Vector3.up * _ragdollStartYVelocity, ForceMode.VelocityChange);
    }

    protected virtual void Idle()
    {
        _animator.SetTrigger(_idle_AnimKey);
    }

    protected virtual void Run()
    {
        _animator.SetTrigger(_run_AnimKey);
        _isMove = true;
    }

    protected virtual void Jump()
    {
        _animator.SetTrigger(_jump_AnimKey);
    }

    protected virtual void AssembleAnimations()
    {
        _animator = GetComponentInChildren<Animator>();

        _idle_AnimKey = Animator.StringToHash("Idle");
        _run_AnimKey = Animator.StringToHash("Move");
        _jump_AnimKey = Animator.StringToHash("Jump");
    }

    public void Punch()
    {
        _transform.DOKill(true);
        _transform.DOPunchScale(Vector3.one * _punchForce, _punchDuration).Play();
    }
}