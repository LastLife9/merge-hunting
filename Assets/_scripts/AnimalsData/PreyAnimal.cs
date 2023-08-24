using TMPro;
using UnityEngine;

public class PreyAnimal : AnimalBase, IDamagable
{
    [SerializeField] private TextMeshPro _healthTxt;
    private float _health;
    private bool _isDie = false;
    public float Health 
    {
        get => _health;
        set 
        {
            _health = value;
            UpdateHealth();
        }
    }

    public void TakeDamage(float damage)
    {
        if (_isDie) return;
        Punch();
        Health -= damage;
        if (Health <= 0)
        {
            Health = 0;
            Death();
        }
    }

    public void Death()
    {
        _isDie = true;
        FindObjectOfType<HuntManager>().PreyDie();
        _isMove = false;
        ActivateRagdoll(false);
    }
    
    public void Move()
    {
        base.Run();
    }

    private void UpdateHealth()
    {
        _healthTxt.text = Health.ToString();
        if (Health <= 0) _healthTxt.alpha = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out IAttacker attacker))
        {
            TakeDamage(attacker.Damage);
            attacker.Attack();
        }
    }
}
