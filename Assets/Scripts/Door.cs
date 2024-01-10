using UnityEngine;

public class Door : MonoBehaviour
{
    public Sprite openDoorSprite;
    public Sprite closeDoorSprite;

    public RoomState roomState;

    private SpriteRenderer spriteRenderer;
    BoxCollider2D boxCollider;

    private void Start()
    {
        spriteRenderer=GetComponent<SpriteRenderer>();
        boxCollider=GetComponent<BoxCollider2D>();
    }
    private void Update()
    {
        if (roomState!=null)
        {
            if (roomState.GetRoomStates()== RoomStates.lockedInRoom)
            {
                spriteRenderer.sprite = closeDoorSprite;
                boxCollider.enabled = true;
            }
            if (roomState.GetRoomStates() == RoomStates.roomClear)
            {
                spriteRenderer.sprite=openDoorSprite;
                boxCollider.enabled = false;
            }
        }
    }
}
