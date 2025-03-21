using UnityEngine;

[CreateAssetMenu(fileName = "ExperienceCollectable", menuName = "Collectable/ExperienceCollectable")]
public class ExperienceCollectable : CollectableSO
{
    [Header("Experience Attributes")]
    public int xpAmount = 10;

    public override void Collect(GameObject objectThatCollected)
    {
        xpAmount = UnityEngine.Random.Range(5, 20);
        GameObject.FindGameObjectWithTag("Player").GetComponent<ExperienceManager>().GainExperience(xpAmount);
    }
}
