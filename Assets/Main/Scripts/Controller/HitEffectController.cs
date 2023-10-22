
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
    public void CreateEffect(Vector3 hitterPosition, Character enemy, float dmgPercent, Vector3 toSide = default(Vector3), float minDmgPercent = 0.45f)
    {
        if (enemy.CanGetEffect())
        {
            int sound = 1;
           
            if (dmgPercent <= 0.1)
            {
                sound = 1;
            }
            else if (dmgPercent < 0.45)
            {
                sound = 2;
            }
            else if (dmgPercent < 0.85)
            {
                sound = 3;
            }
            else
            {
                sound = 4;
            }
            if (dmgPercent > minDmgPercent)
            {

                Vector3 dir = CustomMath.Normalize(enemy.transform.position - hitterPosition);
                if (toSide != Vector3.zero)
                {
                    dir = (toSide.z * Vector3.right + Vector3.back * toSide.x) * PUSH_STR * dmgPercent *
                        Mathf.Clamp(Vector3.Dot(toSide, dir), -1, 1);
                }
                else
                {
                    dir = dir * PUSH_STR * dmgPercent;
                }
                Ray ray = new Ray(enemy.transform.position + Vector3.up, dir);
                //Debug.DrawRay(enemy.transform.position + Vector3.up, dir, Color.red, 3);
                //Debug.Log(enemy.transform.position + " " + dir);
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
}
