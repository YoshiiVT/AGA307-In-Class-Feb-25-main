using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public enum WeaponType
{
    Sword,
    Mace,
    Axe,
    GreatAxe
}

public class Weapon : MonoBehaviour
{
    [SerializeField] private WeaponType myWeaponType;
    [SerializeField] private int myDamageValue;
    private Collider col;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (GetComponent<Collider>() != null)
        {
            col = GetComponent<Collider>();
            col.enabled = false;
        }

        else
        {
            Debug.LogError("Weapon Collider not found.");
            return;
        }

            switch (myWeaponType)
            {
                case WeaponType.Sword:
                    myDamageValue = 10;
                    break;
                case WeaponType.Axe:
                    myDamageValue = 15;
                    break;
                case WeaponType.Mace:
                    myDamageValue = 20;
                    break;
                case WeaponType.GreatAxe:
                    myDamageValue = 30;
                    break;

            }
    }

    public void SetCollider(bool _enabled) => col.enabled = _enabled;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("entered the trigger");
        if(other.GetComponent<PlayerController>() != null)
        {
            other.GetComponent<PlayerController>().TakeDamage(myDamageValue);
        }
    }
}
