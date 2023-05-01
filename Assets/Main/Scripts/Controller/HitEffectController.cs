
using System;
using UnityEngine;
using UnityEngine.UI;

public class HitEffectController
{
    private int mask;
    private const float PUSH_STR = 6;
    private const float PUSH_TIME = 0.7f;
    public HitEffectController()
    {
        mask = LayerMask.GetMask(new string[] { "Bound", "Wall", "Water" });
    }
    public void CreateEffect(Character character, Character enemy, float dmgPercent, bool toSide = false)
    {
        int sound = 1;
        if (dmgPercent > 0.45f)
        {
            sound = 2;
            Vector3 dir = CustomMath.Normalize(enemy.transform.position - character.transform.position);
            if (toSide)
            {
                dir = (character.model.transform.forward.z * Vector3.right + Vector3.back * character.model.transform.forward.x) * PUSH_STR * dmgPercent *
                    Mathf.Clamp(Vector3.Dot(character.model.transform.forward, dir), -1, 1);
            }
            else
            {
                dir = dir * PUSH_STR * dmgPercent;
            }
            Ray ray = new Ray(enemy.transform.position + Vector3.up, dir);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, PUSH_STR * dmgPercent, mask))
            {
                LeanTween.moveX(enemy.gameObject, hit.point.x, PUSH_TIME * 0.5f).setEaseInOutBack();
                LeanTween.moveZ(enemy.gameObject, hit.point.z, PUSH_TIME * 0.5f).setEaseInOutBack().setOnComplete(enemy.WallHit);
                enemy.model.transform.forward = hit.normal;
            }
            else
            {
                LeanTween.moveX(enemy.gameObject, enemy.transform.position.x + dir.x, PUSH_TIME).setEaseOutQuart();
                LeanTween.moveZ(enemy.gameObject, enemy.transform.position.z + dir.z, PUSH_TIME).setEaseOutQuart();
            }

        }
        EventManager.TriggerEvent(EventName.PLAY_FX, EventManager.Instance.GetEventData().SetString("hit" + sound));
    }
}
