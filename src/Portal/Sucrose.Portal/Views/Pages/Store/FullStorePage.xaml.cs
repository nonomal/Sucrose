﻿using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Controls;
using SMMG = Sucrose.Manager.Manage.General;
using SMMO = Sucrose.Manager.Manage.Objectionable;
using SMMP = Sucrose.Manager.Manage.Portal;
using SMMRF = Sucrose.Memory.Manage.Readonly.Folder;
using SMMRG = Sucrose.Memory.Manage.Readonly.General;
using SMMRP = Sucrose.Memory.Manage.Readonly.Path;
using SPMI = Sucrose.Portal.Manage.Internal;
using SPVCSC = Sucrose.Portal.Views.Controls.StoreCard;
using SRER = Sucrose.Resources.Extension.Resources;
using SSSHC = Sucrose.Shared.Space.Helper.Clean;
using SSSIC = Sucrose.Shared.Store.Interface.Category;
using SSSIS = Sucrose.Shared.Store.Interface.Store;
using SSSIW = Sucrose.Shared.Store.Interface.Wallpaper;

namespace Sucrose.Portal.Views.Pages.Store
{
    /// <summary>
    /// FullStorePage.xaml etkileşim mantığı
    /// </summary>
    public partial class FullStorePage : Page, IDisposable
    {
        public static ICollection<NavigationViewItem> MenuItems { get; set; }

        private SSSIS Store = new();

        private bool Searching;

        internal FullStorePage(SSSIS Store)
        {
            this.Store = Store;
            DataContext = this;

            ToolTip SymbolTip = new()
            {
                Content = SRER.GetValue("Portal", "Category", "All")
            };

            ObservableCollection<NavigationViewItem> Categories = new();

            NavigationViewItem AllMenu = new(SRER.GetValue("Portal", "Category", "All"), SPMI.AllIcon, null)
            {
                Tag = string.Empty,
                ToolTip = SymbolTip,
                IsActive = SPMI.CategoryService.CategoryTag == string.Empty
            };

            AllMenu.Click += (s, e) => CategoryClick(s);

            Categories.Add(AllMenu);

            if (Store != null && Store.Categories != null && Store.Categories.Any())
            {
                foreach (KeyValuePair<string, SSSIC> Category in Store.Categories)
                {
                    if (Category.Value.Wallpapers.Any() && (SMMP.StoreAdult || Category.Value.Wallpapers.Count(Wallpaper => Wallpaper.Value.Adult) != Category.Value.Wallpapers.Count()))
                    {
                        SymbolRegular Symbol = SPMI.DefaultIcon;

                        SymbolTip = new()
                        {
                            Content = SRER.GetValue("Portal", "Category", Category.Key.Replace(" ", ""))
                        };

                        if (SPMI.CategoryIcons.TryGetValue(Category.Key, out SymbolRegular Icon))
                        {
                            Symbol = Icon;
                        }

                        NavigationViewItem Menu = new(SRER.GetValue("Portal", "Category", Category.Key.Replace(" ", "")), Symbol, null)
                        {
                            Tag = Category.Key,
                            ToolTip = SymbolTip,
                            IsActive = SPMI.CategoryService.CategoryTag == Category.Key
                        };

                        Menu.Click += (s, e) => CategoryClick(s);

                        Categories.Add(Menu);
                    }
                }
            }

            Categories = new(Categories.OrderBy(Menu => Menu.Content));

            Categories.Move(Categories.IndexOf(Categories.FirstOrDefault(Menu => Menu == AllMenu)), 0);

            MenuItems = Categories;

            InitializeComponent();

            Pagination();
            Category();
            Search();
        }

        private void Search()
        {
            string Search = SPMI.SearchService.SearchText;

            SPMI.SearchService.Dispose();

            SPMI.SearchService = new()
            {
                SearchText = Search
            };

            SPMI.SearchService.SearchTextChanged += SearchService_SearchTextChanged;
        }

        private void Category()
        {
            string Tag = SPMI.CategoryService.CategoryTag;

            SPMI.CategoryService.Dispose();

            SPMI.CategoryService = new()
            {
                CategoryTag = Tag
            };

            SPMI.CategoryService.CategoryTagChanged += CategoryService_CategoryTagChanged;
        }

        private void Pagination()
        {
            ThemePagination.SelectPageChanged += ThemePagination_SelectPageChanged;
        }

        private void CategoryClick(object s)
        {
            NavigationViewItem sender = s as NavigationViewItem;

            sender.IsActive = true;

            SPMI.CategoryService.CategoryTag = sender.Tag.ToString();

            CategoryView.MenuItems
                .OfType<NavigationViewItem>()
                .Where(Item => Item.IsActive)
                .ToList()
                .ForEach(Item =>
                {
                    if (Item != sender)
                    {
                        Item.IsActive = false;
                    }
                });
        }

        private async Task AddThemes(string[] Search, string Text, int Page, string Tag)
        {
            await Application.Current.Dispatcher.InvokeAsync(async () =>
            {
                int Count = 0;

                PageScroll.ScrollToVerticalOffset(0);

                ThemePagination.Visibility = Visibility.Collapsed;

                if (Search.Any())
                {
                    foreach ((string CategoryKey, string WallpaperKey, SSSIW Wallpaper) in GetSortedWallpapers(Store, Search))
                    {
                        if (string.IsNullOrEmpty(SPMI.CategoryService.CategoryTag) || CategoryKey == SPMI.CategoryService.CategoryTag)
                        {
                            if (!Wallpaper.Adult || (Wallpaper.Adult && SMMP.StoreAdult))
                            {
                                if (ThemePagination.SelectPage == Page && SPMI.CategoryService.CategoryTag == Tag && SPMI.SearchService.SearchList.Any() && SPMI.SearchService.SearchText == Text)
                                {
                                    if (SMMP.StorePagination * Page > Count && SMMP.StorePagination * Page <= Count + SMMP.StorePagination)
                                    {
                                        string Theme = Path.Combine(SMMRP.ApplicationData, SMMRG.AppName, SMMRF.Cache, SMMRF.Store, CategoryKey, SSSHC.FileName(WallpaperKey));

                                        SPVCSC StoreCard = new(Theme, new(WallpaperKey, Wallpaper), SMMG.UserAgent, SMMO.PersonalAccessToken);

                                        ThemeStore.Children.Add(StoreCard);

                                        Empty.Visibility = Visibility.Collapsed;

                                        await Task.Delay(50);
                                    }

                                    Count++;
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    foreach (KeyValuePair<string, SSSIC> Category in Store.Categories)
                    {
                        if (string.IsNullOrEmpty(SPMI.CategoryService.CategoryTag) || Category.Key == SPMI.CategoryService.CategoryTag)
                        {
                            foreach (KeyValuePair<string, SSSIW> Wallpaper in Category.Value.Wallpapers)
                            {
                                if (!Wallpaper.Value.Adult || (Wallpaper.Value.Adult && SMMP.StoreAdult))
                                {
                                    if (ThemePagination.SelectPage == Page && SPMI.CategoryService.CategoryTag == Tag && !SPMI.SearchService.SearchList.Any())
                                    {
                                        if (SMMP.StorePagination * Page > Count && SMMP.StorePagination * Page <= Count + SMMP.StorePagination)
                                        {
                                            string Theme = Path.Combine(SMMRP.ApplicationData, SMMRG.AppName, SMMRF.Cache, SMMRF.Store, Category.Key, SSSHC.FileName(Wallpaper.Key));

                                            SPVCSC StoreCard = new(Theme, Wallpaper, SMMG.UserAgent, SMMO.PersonalAccessToken);

                                            ThemeStore.Children.Add(StoreCard);

                                            Empty.Visibility = Visibility.Collapsed;

                                            await Task.Delay(50);
                                        }

                                        Count++;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }

                if (ThemeStore.Children.Count <= 0)
                {
                    Empty.Visibility = Visibility.Visible;
                }

                ThemePagination.MaxPage = (int)Math.Ceiling((double)Count / SMMP.StorePagination);
            });
        }

        private static int CountMatchingWords(string Text, string[] Pattern)
        {
            //return Text.Split(' ').Count(Word => Pattern.Any(Words => Word.Contains(Words)));
            return Pattern.Count(Word => Text.Split(' ').Any(TextWord => TextWord.Contains(Word)));
        }

        private async void FullStorePage_Loaded(object sender, RoutedEventArgs e)
        {
            Dispose();

            ThemeStore.ItemMargin = new Thickness(SMMP.AdaptiveMargin);
            ThemeStore.MaxItemsPerRow = SMMP.AdaptiveLayout;

            await AddThemes(SPMI.SearchService.SearchList, SPMI.SearchService.SearchText, ThemePagination.SelectPage, SPMI.CategoryService.CategoryTag);
        }

        private async void SearchService_SearchTextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(SPMI.SearchService.SearchText) || !string.IsNullOrWhiteSpace(SPMI.SearchService.SearchText))
            {
                Dispose();

                Searching = true;

                ThemePagination.SelectPage = 1;

                await AddThemes(SPMI.SearchService.SearchList, SPMI.SearchService.SearchText, ThemePagination.SelectPage, SPMI.CategoryService.CategoryTag);

                Searching = false;
            }
        }

        private async void ThemePagination_SelectPageChanged(object sender, EventArgs e)
        {
            if (!Searching)
            {
                Dispose();

                await AddThemes(SPMI.SearchService.SearchList, SPMI.SearchService.SearchText, ThemePagination.SelectPage, SPMI.CategoryService.CategoryTag);
            }
        }

        private async void CategoryService_CategoryTagChanged(object sender, EventArgs e)
        {
            Dispose();

            await AddThemes(SPMI.SearchService.SearchList, SPMI.SearchService.SearchText, ThemePagination.SelectPage, SPMI.CategoryService.CategoryTag);
        }

        private static List<(string CategoryKey, string WallpaperKey, SSSIW Wallpaper)> GetSortedWallpapers(SSSIS Store, string[] Words)
        {
            return Store.Categories.SelectMany(Category => Category.Value.Wallpapers
                .Select(Wallpaper => new
                {
                    CategoryKey = Category.Key,
                    Wallpaper = Wallpaper.Value,
                    WallpaperKey = Wallpaper.Key,
                    MatchCount = CountMatchingWords(Wallpaper.Value.Pattern ?? Wallpaper.Key.ToLowerInvariant(), Words)
                }))
                .Where(Pair => Pair.MatchCount > 0)
                .OrderByDescending(Pair => Pair.MatchCount)
                .Select(Pair => (Pair.CategoryKey, Pair.WallpaperKey, Pair.Wallpaper))
                .ToList();
        }

        public void Dispose()
        {
            ThemeStore.Children.Clear();

            GC.Collect();
            GC.SuppressFinalize(this);
        }
    }
}