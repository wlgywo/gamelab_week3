using UnityEngine;

[CreateAssetMenu(fileName = "New SeedLikeData", menuName = "SeedLikeData")]
public class SeedLike : ScriptableObject
{
    [Header("ȣ���� ����")]
    public int likeability;
    public bool isTodayLikeGiven = false;
    public bool isTodayAlreadyTalk = false;
    public bool isTodayAlreadyGift = false;

    [Header("ȣ������ �̹���")]
    public Sprite hateImage;
    public Sprite smallLikeImage;
    public Sprite midLikeImage;
    public Sprite veryLikeImage;
    public Sprite marryImage;
}
