/*
 * Author: Muhammad Farhan
 * Date: 27/11/2024
 * Description: Script to handle checkout
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CustomerData;

public class CheckoutManager : MonoBehaviour
{
    /// <summary>
    /// To set checkout manager instance
    /// </summary>
    public static CheckoutManager Instance; // Singleton pattern

    private List<BarcodeItem> scannedItems = new List<BarcodeItem>();
    public float totalPrice = 0f;

    /// <summary>
    /// Placing instatiated grocery items
    /// </summary>
    public Transform spawnPoint; // Position where items will spawn
    public float itemSpacing = 0.5f; // Space between items
    private float currentOffset = 0f; // Tracks where to place the next item

    private CustomerData currentCustomer;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Function to assign customer that walked up to counter
    /// </summary>
    /// <param name="customer"></param>
    public void AssignCustomer(CustomerData customer)
    {
        currentCustomer = customer;
        Debug.Log($"Assigned customer: {customer.FullName}");
    }

    /// <summary>
    /// Function to add items scanned to a cart
    /// </summary>
    /// <param name="item"></param>
    public void AddItemToCart(BarcodeItem item)
    {
        scannedItems.Add(item);
        totalPrice += item.itemPrice;
        Debug.Log($"Item added: {item.itemName}, Total Price: ${totalPrice:F2}");
    }

    /// <summary>
    /// Function to reset cart for next customer
    /// </summary>
    public void ResetCart()
    {
        scannedItems.Clear();
        totalPrice = 0f;
        Debug.Log("Cart reset.");
    }

    /// <summary>
    /// Function to spawn items for customers
    /// </summary>
    /// <param name="customer"></param>
    public void SpawnItemsForCustomer(CustomerData customer)
    {
        if (currentCustomer == null) return;

        foreach (ShoppingItem shoppingItem in customer.ShoppingList)
        {
            if (shoppingItem.itemPrefab != null)
            {
                // Instantiate the item at the counter
                GameObject item = Instantiate(
                    shoppingItem.itemPrefab,
                    spawnPoint.position + new Vector3(currentOffset, 0, 0),
                    Quaternion.identity
                );

                // Assign the item properties dynamically
                BarcodeItem barcodeItem = item.GetComponent<BarcodeItem>();
                if (barcodeItem != null)
                {
                    barcodeItem.itemName = shoppingItem.itemName;
                    barcodeItem.itemPrice = shoppingItem.itemPrice;
                }

                // Adjust the offset for the next item
                currentOffset += itemSpacing;
            }
        }

        // Reset the offset for the next customer
        currentOffset = 0f;
    }
}