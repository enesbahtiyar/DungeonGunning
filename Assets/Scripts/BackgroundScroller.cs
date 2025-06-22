using UnityEngine;
using UnityEngine.UI;

public class BackgroundScroller : MonoBehaviour
{
    [SerializeField]private RawImage backGround;
    [Range(0f, 1f)]
    [SerializeField] private float scrollSpeed;

    public void ScrollBackGround(Transform player)
    {

        Vector2 playerPos = new Vector2(player.position.x, player.position.y) * scrollSpeed;
        backGround.uvRect = new Rect(playerPos, backGround.uvRect.size);

    }
}
