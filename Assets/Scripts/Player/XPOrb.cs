using UnityEngine;

public class XPOrb : MonoBehaviour
{
    [SerializeField] private int xpAmount = 1;

    public void SetXP(int amount)
    {
        xpAmount = amount;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        XPManager xpManager =
            other.GetComponent<XPManager>();

        if (xpManager == null)
            return;

        xpManager.AddXP(xpAmount);

        Destroy(gameObject);
    }
}