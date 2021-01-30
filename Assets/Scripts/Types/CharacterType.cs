//Written Gabriel Tupy 1-29-2021

using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacterType", menuName ="Types/CharacterType")]
public class CharacterType : ScriptableObject
{
    [SerializeField] private string characterName = "";
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float dashSpeed = 2f;

    public string GetCharacterName()
    {
        return characterName;
    }

    public float GetMoveSpeed()
    {
        return moveSpeed;
    }

    public float GetDashSpeed()
    {
        return dashSpeed;
    }
}
