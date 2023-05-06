public interface ISkill
{
    string GetName();
    string GetDescription();
    int ExtraDamage();
    int ExtraHealth();
    int ExtraRecluit();
    int ExtraDefense();
    void ExecuteOnHit(Character characterHit, float damage);
    void ExecuteOnKill(Character character,Character characterKill);
    void ExecuteOnGrab(Character character);
    void ExecuteAura();
    bool IsAvailable();
}