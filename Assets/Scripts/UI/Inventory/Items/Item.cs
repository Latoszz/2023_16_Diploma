using UnityEngine;

[System.Serializable]
public abstract class Item: MonoBehaviour {
    [SerializeField] protected string itemName;
    [SerializeField] protected Sprite sprite;

    public string GetName() {
        return itemName;
    }

    public Sprite GetSprite() {
        return sprite;
    }
    
    public void SetName(string name) {
        itemName = name;
    }

    public void SetSprite(Sprite sprite) {
        this.sprite = sprite;
    }
}
