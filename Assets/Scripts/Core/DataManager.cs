using UnityEngine;
public static class DataManager
{
    public static readonly string[] destinationName =
    {
        "House","Kitchen","Gate","Room"
    };
    public static readonly string Start = "Start";
    public static readonly string SelectMap = "SelectMap";
    public static readonly string ARMap = "ARMap";

    // Temporary Data
    public static Vector3 destinationTransform = new Vector3(1.033f, 0, 0.571f);
    public static Vector3 playerTransform = new Vector3(-1.15f, 0, -0.46f);
}
