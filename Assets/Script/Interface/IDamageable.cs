// สิ่งที่รับความเสียหายได้ (ปัจจุบัน: Character ทุกชนิด, อนาคต: ถังไม้/กำแพงพัง ฯลฯ)
public interface IDamageable
{
    void TakeDamage(int damage, Character attacker);
}
