using UnityEngine;

public class SnapToGrid : MonoBehaviour {
    public float PPU = 32; // pixels per unit (your tile size)

    private void LateUpdate() {//setzt die position auf ein fielfaches von der PPU
        Vector3 position = this.transform.position;

        position.x = Mathf.Round(position.x * PPU) / PPU;
        position.y = Mathf.Round(position.y * PPU) / PPU;

        this.transform.position = position;
    }
}