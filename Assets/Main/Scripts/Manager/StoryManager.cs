using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryManager : MonoBehaviour
{
    private Story story;
    CharacterStory[] characters;
    private List<List<string>> list;
    private List<string> current;
    private Action nextAction;
    private CharacterEnemy lastCharacterEnemy;
    internal void SetJsonCode(string storyJsonFileName)
    {
        BlankNextAction();
        TextAsset info = Resources.Load<TextAsset>("Story/" + storyJsonFileName);
        story = JsonUtility.FromJson<Story>(info.text);

        int counter = 0;
        list = new List<List<string>>();
        list.Add(new List<string>());
        foreach (var item in story.story)
        {
            if (item == "&")
            {
                list.Add(new List<string>());
                counter++;
            }
            else
            {
                list[counter].Add(item);
            }
        }

    }

    private void BlankNextAction()
    {
        nextAction = () => { };
    }

    public void CallStory(EventData arg = null)
    {
        nextAction();
        BlankNextAction();

        if (list.Count > 0)
        {
            current = list[0];
            list.RemoveAt(0);

            SendMessage(current[0]);
        }
    }
    private void KillAll()
    {
        var cm = FindObjectOfType<CharacterManager>();
        cm.KillExtraTeam(0);
        CallStory();
    }
    private void ForceFollowMain()//1-id 2-rotation
    {
        CharacterStory character = GetCharacter(int.Parse(current[1]));
        var recluit = character.gameObject.AddComponent<EnemyStateAddForceRecluit>();
        recluit.formationGrad = int.Parse(current[2]);
        recluit.Init(character.characterEnemy);
        CallStory();
    }
    private void BlockCharacter()
    {
        FindObjectOfType<CharacterMain>().StateMachine.ChangeState<StateCharacterMainIdle>();
        CallStory();
    }
    private void SetDelay()
    {
        LeanTween.delayedCall(float.Parse(current[1]), () => { CallStory(); });
    }
    private void WebTrap()//id - 1 on-2
    {
        CharacterStory character = GetCharacter(int.Parse(current[1]));
        if (int.Parse(current[2]) >= 1)
            character.characterEnemy.GeneralParticleHandler.webTrap.Play();
        else
            character.characterEnemy.GeneralParticleHandler.webTrap.Stop();

        CallStory();
    }
    private void ShowCaccon()//id - 1 on-2
    {
        CharacterStory character = GetCharacter(int.Parse(current[1]));
        character.characterEnemy.GeneralParticleHandler.CacconOn(int.Parse(current[2]) >= 1);
        CallStory();
    }
    private void EnemyLookAtEnemy()//id1- 1 id2-2
    {
        CharacterStory character = GetCharacter(int.Parse(current[1]));
        CharacterStory character2 = GetCharacter(int.Parse(current[2]));
        character.characterEnemy.model.transform.forward = character2.transform.position - character.transform.position;
        CallStory();
    }
    private void LookDirection()//1-id 2-x -3-z
    {
        CharacterStory character = GetCharacter(int.Parse(current[1]));
        character.characterEnemy.model.transform.forward = Vector3.right * int.Parse(current[2]) + Vector3.forward * int.Parse(current[3]);
        CallStory();
    }
    private void LookAtCharacter()
    {
        CharacterStory character = GetCharacter(int.Parse(current[1]));

        character.characterEnemy.model.transform.forward = FindObjectOfType<CharacterMain>().transform.position - character.transform.position;
        CallStory();
    }
    private void CharacterLookAt()
    {
        var main = FindObjectOfType<CharacterMain>();
        CharacterStory character = GetCharacter(int.Parse(current[1]));

        main.model.transform.forward = character.transform.position - main.transform.position;
        CallStory();
    }
    private void SetAsParent()//1 - parent 2-child
    {
        CharacterStory character = GetCharacter(int.Parse(current[1]));
        CharacterStory character2 = GetCharacter(int.Parse(current[2]));
        character2.characterEnemy.Rigidbody.isKinematic = true;
        character2.transform.SetParent(character.transform);
        CallStory();
    }

    private void FollowExit()//1 - speed
    {
        FindObjectOfType<CameraHandler>().GoCinematicStory(FindObjectOfType<ExitController>().gameObject, false, float.Parse(current[1]));
        current[1] = EventName.STORY_CAM_ARRIVE;
        Wait4Trigger();
    }
    private void Follow()//1-id 2-warp 3-cameraSize
    {
        int id = int.Parse(current[1]);
        GameObject character;
        if (id != -1)
        {
            character = GetCharacter(id).gameObject;
        }
        else
        {
            character = FindObjectOfType<CharacterMain>().gameObject;

        }
        bool warp = int.Parse(current[2]) == 1;
        FindObjectOfType<CameraHandler>().GoCinematicStory(character, warp, float.Parse(current[3]));
        if (warp)
        {
            CallStory();
        }
        else
        {
            current[1] = EventName.STORY_CAM_ARRIVE;
            Wait4Trigger();
        }

    }
    private void HideArmy()
    {
        FindObjectOfType<CharacterMain>().ArmyOffset(25);
        CallStory();
    }
    private void CamOffsetZ()//1-offset
    {
        FindObjectOfType<CameraHandler>().OFFSET_Z = int.Parse(current[1]);
        CallStory();
    }

    private void CallExpresion()//1-id 2-itemname 3-offset
    {
        Vector3 pos;
        int id = int.Parse(current[1]);
        if (id == -1)
        {
            pos = FindObjectOfType<CharacterMain>().transform.position + Vector3.up * int.Parse(current[3]);
        }
        else
        {
            pos = GetCharacter(id).transform.position + Vector3.up * int.Parse(current[1]);
        }
        GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Expresion/" + current[2])).transform.position = pos;
        CallStory();
    }
    private void CharacterAnimation()//1-animation
    {
        FindObjectOfType<CharacterMain>().SetAnimation(current[1], 0.01f);
        CallStory();
    }
    private void RemoveColliders()//1-id 
    {
        CharacterStory character = GetCharacter(int.Parse(current[1]));
        character.characterEnemy.DisableCollider();
        CallStory();
    }
    private void Animation()//1-id 2-animation
    {
        CharacterStory character = GetCharacter(int.Parse(current[1]));
        character.characterEnemy.GoVulnerable(99);
        character.characterEnemy.SetAnimation(current[2], 0.3f);
        CallStory();
    }
    private void Teleport()//1-id 2-x 3-z
    {
        CharacterStory character = GetCharacter(int.Parse(current[1]));
        character.transform.position = Vector3.right * float.Parse(current[2]) + Vector3.forward * float.Parse(current[3]);
        CallStory();
    }
    private void MainGoToPositionStory()//1-pos x 3-pos z 4-speed
    {
        var character = FindObjectOfType<CharacterMain>();
        character.GoToPosition(float.Parse(current[1]), float.Parse(current[2]), float.Parse(current[3]));
        CallStory();
    }
    private void GoToPositionStory()//1-id 2-pos x 3-pos z 4-speed
    {
        CharacterStory character = GetCharacter(int.Parse(current[1]));
        character.GoToPosition(float.Parse(current[2]), float.Parse(current[3]), float.Parse(current[4]));
        CallStory();

    }
    private CharacterStory GetCharacter(int id)
    {
        if (characters == null)
            characters = FindObjectsOfType<CharacterStory>();
        CharacterStory character = null;
        foreach (var c in characters)
        {
            if (c.characterEnemy.id == id)
            {
                character = c;
                break;
            }
        }
        return character;
    }
    private void TeleportMainCharacter()//1-x 2-z
    {
        FindObjectOfType<CharacterMain>().transform.position = Vector3.right * float.Parse(current[1]) + Vector3.forward * float.Parse(current[2]);
        CallStory();
    }
    private void SpawnCharacter()//1-id 2-x 3-z 4-rotation 5-team
    {
        CharacterManager characterManager = FindObjectOfType<CharacterManager>();
        lastCharacterEnemy = GameObject.Instantiate<CharacterEnemy>(characterManager.Loader.GetCharacter(int.Parse(current[1])));
        lastCharacterEnemy.transform.position = Vector3.right * float.Parse(current[2]) + Vector3.forward * float.Parse(current[3]);
        lastCharacterEnemy.model.transform.rotation = Quaternion.Euler(0, float.Parse(current[4]), 0);
        lastCharacterEnemy.team = int.Parse(current[5]);
        CallStory();
    }
    private void GoInGame()
    {
        CharacterMain characterMain = FindObjectOfType<CharacterMain>();
        characterMain.StateMachine.ChangeState(typeof(StateCharacterMainIdle));
        characterMain.StateMachine.ChangeState(characterMain.IdleState);
        characterMain.ArmyOffset(1);
        FindObjectOfType<CameraHandler>().GoInGame(characterMain.gameObject, false);
        CallStory();
    }
    private void ForceRotation()//1-id 2-rotation
    {
        CharacterStory character = GetCharacter(int.Parse(current[1]));
        float rotation = float.Parse(current[2]);
        character.characterEnemy.model.transform.rotation = Quaternion.Euler(0, rotation, 0);
        CallStory();
    }
    private void Wait4Trigger()
    {
        string eventName = current[1];
        nextAction = () =>
        {
            EventManager.StopListening(eventName, CallStory);
        };
        EventManager.StartListening(eventName, CallStory);
    }

    private void HintSinglePressUI()//1 CharacterId //2 tutorial id
    {
        if (SaveData.GetInstance().GetValue("tutorial" + current[2]) == 0)
        {
            var hspui = gameObject.AddComponent<HintSinglePressUINoDrag>();
            int tutorialID = int.Parse(current[2]);
            hspui.SetID(tutorialID);
            RecluitController rc = FindObjectOfType<RecluitController>();
            int enemyID = int.Parse(current[1]);
            LeanTween.delayedCall(gameObject, 2, () =>
            {
                foreach (var item in rc.iconUI)
                {

                    if (item.CharacterEnemy != null && item.CharacterEnemy.id == enemyID && item.button.interactable)
                    {
                        item.tutorialID = tutorialID;
                        EventManager.TriggerEvent(EventName.TUTORIAL_START, EventManager.Instance.GetEventData().SetInt(tutorialID).SetTransform(item.transform));
                        break;
                    }
                }
            });

        }
        CallStory();
    }
}
