using System;
using System.Collections.Generic;

class InventoryItem
{
    public string? SKU { get; set; }
    public string? Name { get; set; }
    public int Quantity { get; set; }
}

class Program
{
    static readonly Dictionary<string, InventoryItem> inventory = new();
    static bool isItemLocated = false;
    static bool continueRunning = true;
    static InventoryItem? currentItem = null;

    static readonly Dictionary<string, (Action, string)> commandDefinitions = new()

    //command user enters into CLI, method command refrences, description of what a command does - SSOT for all CLI commands.
    {
        { "help", (PrintCommandList, "Displays the list of available commands.") },
        { "refreshInventory", (FetchInventory, "Displays the current inventory.") },
        { "item-create", (CreateItem, "Creates a new item in the inventory.") },
        { "item-locate", (LocateItem, "Locates an item in the inventory by SKU.") },
        { "item-delete", (DeleteItem, "Deletes the currently located item from the inventory.") },
        { "item-replace", (ReplaceItem, "Replaces the details of the currently located item.") },
        { "clear", (Console.Clear, "Clears the console screen.") },
        { "exit", (() => continueRunning = false, "Exits the program.") }
    };

    static void Main()
   {
        while (continueRunning)
        {
            Console.WriteLine("Enter a command:");
            string? userInput = Console.ReadLine();

            if (!string.IsNullOrEmpty(userInput) && commandDefinitions.TryGetValue(userInput, out (Action, string) value))
            {
                value.Item1.Invoke();
            }
            else
            {
                Console.WriteLine("Invalid Command.");
            }
        }
    }

    static void CreateItem()
    {
        Console.WriteLine("Enter SKU:");
        string? sku = Console.ReadLine();

        // Check if SKU already exists in the inventory
        if (!string.IsNullOrEmpty(sku) && !inventory.ContainsKey(sku))
        {
            Console.WriteLine("Enter Name:");
            string? name = Console.ReadLine();

            Console.WriteLine("Enter Quantity:");
            int quantity;
            while (!int.TryParse(Console.ReadLine(), out quantity))
            {
                Console.WriteLine("Invalid input. Please enter a numeric quantity:");
            }

            InventoryItem newItem = new() { SKU = sku, Name = name, Quantity = quantity };
            inventory.Add(sku, newItem);
            Console.WriteLine("Item added successfully.");
        }
        else
        {
            Console.WriteLine("Invalid SKU or item already exists.");
        }
    }
    
    static void LocateItem()
    {
        Console.WriteLine("Enter the SKU of the item:");
        string? userInput = Console.ReadLine();

        if (!string.IsNullOrEmpty(userInput) && inventory.TryGetValue(userInput, out InventoryItem? item))
        {
            currentItem = item; // Assign the locally found item to the global currentItem
            isItemLocated = true;
            Console.WriteLine($"Item {currentItem.SKU} found: {currentItem.Name}, Quantity: {currentItem.Quantity}");
        }
        else
        {
            currentItem = null;
            isItemLocated = false;
            Console.WriteLine("Item not found.");
        }
    }

    static void DeleteItem()
    {
        if (isItemLocated && currentItem != null)
        {
            inventory.Remove(currentItem.SKU!); // If current item is invalid or non-existant, handled by print message below.
            isItemLocated = false;
            currentItem = null;
            Console.WriteLine("Item deleted successfully.");
        }
        else
        {
            Console.WriteLine("Item is non-existant or invalid! Please locate a valid item using 'item-locate' before using this function.");
        }
    }

    static void ReplaceItem()
    {
        if (isItemLocated && currentItem != null)
        {
            // Code to replace the item's details
            isItemLocated = false;
            currentItem = null;
        }
        else
        {
            Console.WriteLine("Please use command 'item-locate' to first locate an item before using this function.");
        }
    }
    static void PrintCommandList()
    {
        Console.WriteLine("Available commands:");
        foreach (var command in commandDefinitions)
        {
            Console.WriteLine($"{command.Key}: {command.Value.Item2}");
        }
    }

    static void FetchInventory()
    {
        if (inventory.Count == 0)
        {
            Console.Clear();
            Console.WriteLine("The inventory is currently empty.");
            return;
        }
        
        // Initial column width setup
        int maxSkuLength = 9; // Based on SKU format
        int quantityHeaderLength = "Quantity".Length;
        int maxQuantityLength = inventory.Values.Max(item => item.Quantity.ToString().Length);
        maxQuantityLength = Math.Max(maxQuantityLength, quantityHeaderLength);

        // Find the maximum length of item names and calculate initial widths
        int maxNameLength = inventory.Values.Max(item => item.Name?.Length ?? 0);
        int skuColumnWidth = maxSkuLength;
        int nameColumnWidth = maxNameLength;
        int quantityColumnWidth = maxQuantityLength;

        // Calculate the total width for the table
        int visualWidthBuffer = 4; //An addtional buffer refrenced by all proceeding width calculations
        int totalTableWidth = skuColumnWidth + nameColumnWidth + quantityColumnWidth + visualWidthBuffer; 


        string inventoryCountLine = $"{inventory.Count} Items Currently in Inventory:";
        int minimumSeparatorLength = inventoryCountLine.Length + visualWidthBuffer; 

        // Adjust table width based on the longest line
        totalTableWidth = Math.Max(totalTableWidth, minimumSeparatorLength);

        // Adjust name column width for centering if names are short
        if (totalTableWidth > (skuColumnWidth + maxNameLength + quantityColumnWidth + visualWidthBuffer))
        {
            nameColumnWidth = totalTableWidth - skuColumnWidth - quantityColumnWidth - visualWidthBuffer;
        }

        string tableSeparator = new string('-', totalTableWidth);

        Console.Clear();
        Console.WriteLine(inventoryCountLine);
        Console.WriteLine(tableSeparator);

        // Print headers
        string headerFormat = "{0,-" + skuColumnWidth + "} {1,-" + nameColumnWidth + "} {2,-" + quantityColumnWidth + "}";
        Console.WriteLine(headerFormat, "SKU", "Name", "Quantity");
        Console.WriteLine(tableSeparator);

        foreach (var item in inventory)
        {
            // Print each item row
            Console.WriteLine(headerFormat, item.Key, item.Value.Name, item.Value.Quantity);
        }
        Console.WriteLine(tableSeparator);
    }
}
