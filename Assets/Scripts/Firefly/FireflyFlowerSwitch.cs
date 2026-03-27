using UnityEngine;

public class FireflyFlowerSwitch : MonoBehaviour
{
    public GameObject fireflyVisual;
    public GameObject flowerVisual;

    public void ShowFirefly()
    {
        if (fireflyVisual != null) fireflyVisual.SetActive(true);
        if (flowerVisual != null) flowerVisual.SetActive(false);
    }

    public void ShowFlower()
    {
        if (fireflyVisual != null) fireflyVisual.SetActive(false);
        if (flowerVisual != null) flowerVisual.SetActive(true);
    }
}