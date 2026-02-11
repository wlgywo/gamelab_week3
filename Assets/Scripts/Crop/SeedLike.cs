using UnityEngine;

[CreateAssetMenu(fileName = "New SeedLikeData", menuName = "SeedLikeData")]
public class SeedLike : ScriptableObject
{
    [Header("호감도 설정")]
    public int likeability;
    public bool isTodayLikeGiven = false;
    public bool isTodayAlreadyTalk = false;
    public bool isTodayAlreadyGift = false;

    [Header("호감도별 이미지")]
    public Sprite hateImage;
    public Sprite smallLikeImage;
    public Sprite midLikeImage;
    public Sprite veryLikeImage;
    public Sprite marryImage;
}
