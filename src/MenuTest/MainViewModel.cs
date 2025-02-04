﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace MenuTest;

public partial class MainViewModel : ObservableObject
{
	public Page? MenuHostingPage { get;  set; }

	[ObservableProperty]
	public partial bool CanAddFlyoutItem { get; set; }

	[ObservableProperty]
	public partial bool CanRemoveFlyoutItem { get; set; }

	[ObservableProperty]
	public partial bool CanAddFlyoutSubItem { get; set; }

	[ObservableProperty]
	public partial bool CanRemoveFlyoutSubItem { get; set; }

	public MainViewModel()
    {
		//ValidateSubmittable();

		CanAddFlyoutItem = true;
		CanAddFlyoutSubItem = true;
	}

	partial void OnCanAddFlyoutItemChanged(bool value)
	{
		CanRemoveFlyoutItem = !value;
	}

	partial void OnCanAddFlyoutSubItemChanged(bool value)
	{
		CanRemoveFlyoutSubItem = !value;
	}

	[RelayCommand]
	async void AddFlyOutItem()
	{
		MenuBarItem menuBarItem = GetMenuBarItem();

        MenuFlyoutItem itemToAdd = new()
		{
            Text = "Added Item", 
        };
		menuBarItem.Add(itemToAdd);

		CanAddFlyoutItem = false;
	}

	[RelayCommand]
	void RemoveFlyOutItem()
	{
		MenuBarItem menuBarItem = GetMenuBarItem();
		var itemToRemove = GetMenuFlyoutItem("Added Item");
		menuBarItem.Remove(itemToRemove);
		CanAddFlyoutItem = true;
	}

	private MenuBarItem GetMenuBarItem()
	{
		return MenuHostingPage!.MenuBarItems.ToList().SingleOrDefault(menuBarItem => menuBarItem.Text == "Menu Flyout Item")!;
	}

    public IMenuFlyoutItem? GetMenuFlyoutItem(string name)
    {
        IMenuFlyoutItem? result = null;

        MenuHostingPage!.MenuBarItems.ToList().ForEach(menuBarItem =>
        {
			IMenuElement? foundItem = menuBarItem.SingleOrDefault(menuElement => menuElement is MenuFlyoutItem menuItem && menuItem.Text == name);

            if (foundItem != null)
			{
                result = foundItem as MenuFlyoutItem;
			}
        });
            
        return result;
    }

	[RelayCommand]
	void AddFlyOutSubItem()
	{
		MenuFlyoutSubItem menuFlyoutSubItem = GetSubMenu("Flyout")!;

		MenuFlyoutItem itemToAdd = new()
		{
			Text = "Added Sub Item",
			IsEnabled = true,
			Parent = menuFlyoutSubItem
		};
		menuFlyoutSubItem.Add(itemToAdd);
		CanAddFlyoutSubItem = false;

		System.Diagnostics.Debug.WriteLine("");
		foreach (var item in menuFlyoutSubItem)
		{
			System.Diagnostics.Debug.WriteLine(item.Text);
		}
	}

	[RelayCommand]
	void RemoveFlyOutSubItem()
	{
		MenuFlyoutSubItem parentSubMenu = GetSubMenu("Flyout")!;
		IMenuFlyoutItem itemToRemove = GetSubMenuFlyoutItem(parentSubMenu, "Added Sub Item")!;
		parentSubMenu.Remove(itemToRemove);

		CanAddFlyoutSubItem = true;

		System.Diagnostics.Debug.WriteLine("Removed: "+itemToRemove.Text);
	}

	public MenuFlyoutSubItem? GetSubMenu(string name)
	{
		MenuFlyoutSubItem? result = null;

		MenuHostingPage!.MenuBarItems.ToList().ForEach(menuBarItem =>
		{
			var foundItem = menuBarItem.SingleOrDefault(menuElement => menuElement is MenuFlyoutSubItem subMenu && subMenu.Text == name);

			if (foundItem != null)
			{
				result = foundItem as MenuFlyoutSubItem;
			}
		});

		return result;
	}

	public IMenuFlyoutItem? GetSubMenuFlyoutItem(MenuFlyoutSubItem parentSubMenu, string name)
	{
		return parentSubMenu?.SingleOrDefault(element => element.Text == name) as MenuFlyoutItem;
	}
}