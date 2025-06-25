using UnityEngine;

public class PlayerConfig : MonoBehaviour
{
    public float Hp = 50;
    public float Power = 5;
    public float MoveSpeed = 1;
    public float MagicAttackSpeed = 2;
    public float Defense = 1;
    public float AxAttackSpeed = 3;
    public float SwordAttackSpeed = 0.5f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        int layer = collision.gameObject.layer;
        if (layer == LayerMask.NameToLayer("HpDown")) Hp--;
        if (layer == LayerMask.NameToLayer("HpUp")) Hp++;
        if (layer == LayerMask.NameToLayer("SpeedUp")) MoveSpeed++;
        if (layer == LayerMask.NameToLayer("PowerUp")) Power++;
        if (layer == LayerMask.NameToLayer("DefenseUp")) Defense++;
        if (layer == LayerMask.NameToLayer("MagicAttackSpeed")) MagicAttackSpeed -= 0.1f;
        if (layer == LayerMask.NameToLayer("AxAttackSpeed")) AxAttackSpeed -= 0.1f;
        if (layer == LayerMask.NameToLayer("SwordAttackSpeed")) SwordAttackSpeed -= 0.1f;
    }
}
