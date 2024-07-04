using System;
using System.Runtime.CompilerServices;

        // Step 1. Commands
        // Define commands to invoke certain actions in the console to:
        // Diplay the current inventory, create a new item, replace an item, delete an item, or exit the program.

        // Step 2. Read
        // Check if any information stored regarding inventory is present
        // Fetch info of all items and their properties stored in the project and display them in an organized fashion
        // If no inventory info is found inform the user and only allow them to preform actions for creating new items,
        // while disallowing actions letting the user attempt to display, replace, or delete any non-existant item info
        
        // Step 3. Create, Update, and Delete
        // Preform actions outlined in Step 2, if item into exists:
        // promt user to refine their selection based on item ID based on info fetched
        // Have logic to select a specific segment of the inventory info based on item ID
        // After user selects an item, user promted for command action to either Create, Replace, or Delete an Item

        // Step 4. Once info in the database is changed preform Step 2 - Step 3 again

class InventoryItem
{
    public string SKU { get; set; }
    public string Name { get; set; }
    public int Quantity { get; set; }
}

class Program
{
    static Dictionary<string, InventoryItem> inventory = new Dictionary<string, InventoryItem>();

    static void AddInventoryItem(InventoryItem item)
    {
        if (!inventory.ContainsKey(item.SKU))
        {
            inventory.Add(item.SKU, item);
            Console.WriteLine("Item added successfully.");
        }
        else
        {
            Console.WriteLine("An item with this SKU either already exists or is not possible");
        }
    }

    static void Main()
    {
        string[] ConsoleCommands = 
        {
            "refreshInventory", "item-create", "item-delete", "item-replace", "clear", "exit"
        };

        bool continueRunning = true;

        while (continueRunning)
        {
            Console.WriteLine("Enter a command:");
            string? userInput = Console.ReadLine();

            if (userInput != null)
            {
                switch (userInput)
                {
                    case "refreshInventory":
                        Console.Clear();
                        FetchInventory();
                        break;
                    case "item-create":
                        CreateItem();
                        break;
                    case "item-delete":
                        
                    case "clear":
                        Console.Clear();
                        break;
                    case "exit":
                        continueRunning = false;
                        break;
                    default:
                        Console.WriteLine("Inalid Command.");
                        break;
                    // other cases...
                }
            }
        }
    }

    static void CreateItem()
    {
        Console.WriteLine("Enter SKU:");
        string ?sku = Console.ReadLine();

        Console.WriteLine("Enter Name:");
        string ?name = Console.ReadLine(); 

        Console.WriteLine("Enter Quantity:");
        int quantity = int.Parse(Console.ReadLine());

        InventoryItem newItem = new InventoryItem { SKU = sku, Name = name, Quantity = quantity };
        AddInventoryItem(newItem);  
    }

    static void FetchInventory()
    {
            if (inventory.Count == 0)
    {
        Console.WriteLine("The inventory is currently empty.");
        return;
    }
    Console.WriteLine($"{inventory.Count} Items Currently in Inventory:");
    Console.WriteLine(new string('-', Console.WindowWidth));
    Console.WriteLine("SKU\t\tName\t\tQuantity");
    Console.WriteLine(new string('-', Console.WindowWidth));

    foreach (var item in inventory)
    {
        Console.WriteLine($"{item.Key}\t\t{item.Value.Name}\t\t{item.Value.Quantity}");
    }
    Console.WriteLine(new string('-', Console.WindowWidth));
    }

static void LocateItem()
{
    Console.WriteLine("Enter the SKU of the item:");
    string? userInput = Console.ReadLine();

    if (!string.IsNullOrEmpty(userInput))
    {
        if (inventory.TryGetValue(userInput, out InventoryItem item))
        {
            // The item was found, `item` now contains the InventoryItem
            // You can now perform operations on this item

            // Example: Displaying the item
            Console.WriteLine($"Item {item.SKU} found: {item.Name}, Quantity: {item.Quantity}");

            // Here, you can also add further logic to update or delete the item
        }
        else
        {
            Console.WriteLine("Item not found.");
        }
    }
    else
    {
        Console.WriteLine("Invalid input. Please enter a valid SKU.");
    }
}
    };
}




