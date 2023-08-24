interface IDamagable
{
    float Health { get; set; }
    void TakeDamage(float damage);
    void Death();
}