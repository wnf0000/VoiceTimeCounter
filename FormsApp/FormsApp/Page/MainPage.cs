using System;
using System.Collections.ObjectModel;
using Acr.XamForms.UserDialogs;
using FormsApp.Model;
using FormsApp.Service;
//using FormsApp.ViewModel;
using Xamarin.Forms;

namespace FormsApp.Page
{
    class MainPage : ContentPage
    {
        private readonly ListView _listView;

        readonly Label _footerLabel = new Label()
        {
            XAlign = TextAlignment.Center,
            YAlign = TextAlignment.Center,
            TextColor = Color.FromHex("#999"),
            FontSize = 14,
            HeightRequest = 40
        };

        private const int RowHeight = 60;
        public static ObservableCollection<TimeSet> TimeSets { set; get; }
        public MainPage()
        {

            var settings = DbHelper.GetSettings();
            if (settings == null)
            {
                var tts = DependencyService.Get<ITextToSpeech>().New().Init();
                var engines = tts.GetEngines();
                tts.Dispose();
                var engine = "";
                if (engines.Count == 1)
                {
                    engine = engines[0].Name;
                }
                else if (engines.Count > 1)
                {
                    engine = engines[1].Name;
                }
                //1
                var ts1 = new TimeSet()
                {
                    Duration = 120,
                    Name = "单手撑地2分钟 是男人就得坚持 你坚持住了吗",
                    SecendsToReady = 5,
                    VoiceEngine = engine//"com.google.android.tts",

                };
                ts1.AddStage(new Stage(120, 10, ""));
                ts1.AddStage(new Stage(60, 5, ""));
                ts1.AddStage(new Stage(20, 1, ""));
                ts1.ListToStagesDataString();
                DbHelper.AddTimeSet(ts1);
                //2
                var ts2 = new TimeSet()
                {
                    Duration = 60,
                    Name = "这是示例方案 你可以修改也可以删除",
                    SecendsToReady = 5,
                    VoiceEngine = engine//"com.google.android.tts",

                };
                ts2.AddStage(new Stage(60, 5, ""));
                ts2.AddStage(new Stage(10, 1, ""));
                ts2.ListToStagesDataString();
                DbHelper.AddTimeSet(ts2);
                //3
                var ts3 = new TimeSet()
                {
                    Duration = 30,
                    Name = "这是示例方案 你可以修改也可以删除",
                    SecendsToReady = 0,
                    VoiceEngine = engine//"com.google.android.tts",

                };
                ts3.AddStage(new Stage(30, 5, ""));
                ts3.AddStage(new Stage(10, 1, ""));
                ts3.ListToStagesDataString();
                DbHelper.AddTimeSet(ts3);
                settings = new Settings()
                {
                    IsFirstRun = false
                };
                DbHelper.AddSettings(settings);
                if (engines.Count == 0)
                {
                    var uds = DependencyService.Get<IUserDialogService>();
                    uds.Confirm(new ConfirmConfig()
                    {
                        CancelText = "取消",
                        OkText = "确定",
                        Message = "你的设备没有安装语音引擎，可能不能正常使用本应用，是否现在下载安装语音引擎？",
                        Title = "提示",
                        OnConfirm = (r) =>
                        {
                            if (r)
                                //Device.BeginInvokeOnMainThread(() =>
                                //{
                                    uds.ActionSheet(new ActionSheetConfig()
                                    {
                                        Title = "安装语音引擎",
                                        Options =
                                        {
                                            new ActionSheetOption("下载安装讯飞语音+", () =>
                                            {
                                                var url = "http://zhushou.360.cn/detail/index/soft_id/676960";
                                                Device.OpenUri(new Uri(url));
                                            }),
                                            new ActionSheetOption("下载安装灵犀语音助手", () =>
                                            {
                                                var url = "http://zhushou.360.cn/detail/index/soft_id/194066";
                                                Device.OpenUri(new Uri(url));
                                            }),
                                            new ActionSheetOption("下载安装Google文字转语音 英语", () =>
                                            {
                                                var url = "http://zhushou.360.cn/detail/index/soft_id/885077";
                                                Device.OpenUri(new Uri(url));
                                
                                            }),
                                        }
                                    });
                                //});
                        }
                    });
                }
                else if (engines.Count == 1 && (engines[0].Name.Contains("google") || engines[0].Name.Contains("pico")))
                {
                    var uds = DependencyService.Get<IUserDialogService>();
                    uds.Confirm(new ConfirmConfig()
                    {
                        CancelText = "取消",
                        OkText = "确定",
                        Message = "你的设备可能没有安装中文语音引擎，将不能使用中文播报功能，是否现在下载并安装？",
                        Title = "提示",
                        OnConfirm = (r) =>
                        {
                            if (r)
                                //Device.BeginInvokeOnMainThread(() =>
                                //{
                                    uds.ActionSheet(new ActionSheetConfig()
                                    {
                                        Title = "安装语音引擎",
                                        Options =
                                        {
                                            new ActionSheetOption("下载安装讯飞语音+", () =>
                                            {
                                                var url = "http://zhushou.360.cn/detail/index/soft_id/676960";
                                                Device.OpenUri(new Uri(url));
                                            }),
                                            new ActionSheetOption("下载安装灵犀语音助手", () =>
                                            {
                                                var url = "http://zhushou.360.cn/detail/index/soft_id/194066";
                                                Device.OpenUri(new Uri(url));
                                            }),
                                            //new ActionSheetOption("下载安装Google文字转语音 英语", () =>
                                            //{
                                            //    var url = "http://zhushou.360.cn/detail/index/soft_id/885077";
                                            //    Device.OpenUri(new Uri(url));
                                
                                            //}),
                                        }
                                    });
                                //});
                        }
                    });
                }
            }
            DbHelper.CloseConnection();
            Title = "语音倒计时";
            BackgroundColor = Color.FromHex("#eee");

            ToolbarItem addToolbarItem = new ToolbarItem()
            {
                Icon = "ic_add_white_36dp.png",
                Text = "新建倒计时方案"
            };
            ToolbarItems.Add(addToolbarItem);
            addToolbarItem.Clicked += delegate
            {
                var page = new TimeSetCreatePage();
                Navigation.PushAsync(page, true);
            };
            ToolbarItem aboutToolbarItem = new ToolbarItem()
            {
                Icon = "ic_info_outline_white_36dp.png",
                Text = "关于"
            };
            ToolbarItems.Add(aboutToolbarItem);
            aboutToolbarItem.Clicked += delegate
            {
                var page = new AboutPage();
                Navigation.PushAsync(page, true);
            };
            TimeSets = new ObservableCollection<TimeSet>();
            TimeSets.CollectionChanged += TimeSets_CollectionChanged; ;
            _listView = new ListView
            {
                BindingContext = TimeSets,
                RowHeight = RowHeight,
                VerticalOptions = LayoutOptions.FillAndExpand,
                SeparatorVisibility = SeparatorVisibility.Default,
                SeparatorColor = Color.FromHex("#fff"),
                ItemTemplate = new DataTemplate(typeof(ListViewCell))
            };
            _listView.SetBinding(ListView.ItemsSourceProperty, ".");
            _listView.ItemTapped += listView_ItemTapped;
            _listView.Footer = _footerLabel;

            Content = new ScrollView()
            {
                Padding = new Thickness(10, 0),
                BackgroundColor = Color.FromHex("#eee"),
                Content = new StackLayout()
                {
                    Children = { new StackLayout()
            {
                BackgroundColor = Color.FromHex("#eee"),
                Orientation = StackOrientation.Vertical,
                
                Children =
                {
                    new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                     new Label()
                        {
                            Text = "倒计时方案",
                            HeightRequest = 32,
                            FontSize = 16,
                            YAlign = TextAlignment.End,
                            TextColor = Color.FromHex("#666"),
                
                        },
                        new Label()
                        {
                            Text =Device.OnPlatform("左滑可修改或删除","(长按可修改或删除)","") ,
                            HeightRequest = 32,
                            FontSize = 12,
                            YAlign = TextAlignment.End,
                            TextColor = Color.FromHex("#999"),
                
                        }
                }
            }
                }
            },
            _listView
                    }
                }

            };
            LoadData();
        }

        void TimeSets_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            _listView.HeightRequest = 50 + RowHeight * TimeSets.Count;
            _footerLabel.Text = TimeSets.Count > 0 ? string.Format("已有{0}个方案", TimeSets.Count) : "没方案呢^-^";
        }

        void listView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var page = new TimeSetRunPage(e.Item as TimeSet);
            Navigation.PushAsync(page);
        }

        async void LoadData()
        {
            var data = (await DbHelper.GetTimeSets());
            data.ForEach(w => TimeSets.Add(w));
            _listView.HeightRequest = 50 + RowHeight * TimeSets.Count;
            _footerLabel.Text = TimeSets.Count > 0 ? string.Format("已有{0}个方案", TimeSets.Count) : "没方案呢 从右上角建立方案吧";
        }

        class ListViewCell : ViewCell
        {
            public ListViewCell()
            {
                Label nameLabel = new Label()
                {
                    YAlign = TextAlignment.Center,
                    TextColor = Color.FromHex("#fff"),
                    FontSize = 16,
                    LineBreakMode = LineBreakMode.MiddleTruncation,
                    FontAttributes = FontAttributes.Bold
                };
                nameLabel.SetBinding(Label.TextProperty, "Name");
                Label durationLabel = new Label()
                {
                    YAlign = TextAlignment.Center,
                    TextColor = Color.FromHex("#f7f7f7"),
                    FontSize = 12,
                };
                durationLabel.SetBinding(Label.TextProperty, "FormatedDuration");
                StackLayout layout = new StackLayout()
                {
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Padding = new Thickness(8, 0),
                    Spacing = 5,

                    Orientation = StackOrientation.Vertical,
                    Children = { nameLabel, durationLabel }
                };
                ContextActions.Add(new MenuItem()
                {
                    Text = "删除",
                    Command = new Command(() =>
                        {
                            var uds = DependencyService.Get<Acr.XamForms.UserDialogs.IUserDialogService>();
                            uds.Confirm(new ConfirmConfig()
                            {
                                CancelText = "取消",
                                Message = "确定删除吗？",
                                OkText = "确定",
                                Title = "提示",
                                OnConfirm = async (r) =>
                                {
                                    if (r)
                                    {
                                        var m = this.BindingContext as TimeSet;
                                        var s = await DbHelper.DeleteTimeSet(m);
                                        if (s > 0)
                                            MainPage.TimeSets.Remove(m);
                                        else
                                            DependencyService.Get<IToast>().Show("删除失败");
                                    }
                                }
                            });


                        })
                });
                ContextActions.Add(new MenuItem()
                {
                    Text = "修改",
                    Command = new Command(() =>
                    {
                        var m = this.BindingContext as TimeSet;
                        var page = new TimeSetEditPage(m);
                        this.ParentView.Navigation.PushAsync(page);
                    })
                });
                var arrow = new Image() { Source = "ic_chevron_right_white_24dp.png" };
                var item = new StackLayout()
                {
                    HeightRequest = RowHeight,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Orientation = StackOrientation.Horizontal,
                    Children = { layout, arrow }
                };
                item.SetBinding(BackgroundColorProperty, "Color");
                View = item;

            }

        }
    }


}
