
using System;
using Acr.XamForms.UserDialogs;
using FormsApp.Service;
using Xamarin.Forms;

namespace FormsApp.Page
{
    class AboutPage : ContentPage
    {
        public AboutPage()
        {
            Title = "关于";
            BackgroundColor = Color.FromHex("#eee");

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var tts = DependencyService.Get<ITextToSpeech>().New().Init();
            var engines = tts.GetEngines();
            tts.Dispose();
            var engineView = new ListView()
            {
                BackgroundColor = Color.White,
                RowHeight = 45,
                IsEnabled = false
            };
            Xamarin.Forms.View enginesHeaderView;
            if (engines.Count == 0)
            {
                enginesHeaderView = new StackLayout()
                {
                    Padding = 10,
                    BackgroundColor = Color.FromHex("#eee"),
                    Orientation = StackOrientation.Vertical,
                    Children =
                    {
                        new Label()
                        {
                            Text = "你的设备没有安装语音引擎，可能不能正常使用本应用，是否现在下载安装语音引擎？",
                            TextColor = Color.FromHex("#999"),
                            FontSize = 14
                        },
                        new Button()
                        {
                            Text = "安装",
                            TextColor = Color.White,
                            BackgroundColor = Consts.ThemeColor,
                            HeightRequest = 40,
                            Command = new Command(() =>
                            {
                                var uds = DependencyService.Get<IUserDialogService>();
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
                            })
                        }
                    }
                };
            }
            else if (engines.Count == 1 && (engines[0].Name.Contains("google") || engines[0].Name.Contains("pico")))
            {
                enginesHeaderView = new StackLayout()
                {
                    //Padding = 10,
                    //BackgroundColor = Color.FromHex("#eee"),
                    Orientation = StackOrientation.Vertical,
                    Children =
                    {
                        new Label()
                        {
                            Text = "你的设备可能没有安装中文语音引擎，将不能使用中文播报功能，是否现在下载并安装？",
                            TextColor = Color.FromHex("#999"),
                            FontSize = 14
                        },
                        new Button()
                        {
                            Text = "安装",
                            TextColor = Color.White,
                            BackgroundColor = Consts.ThemeColor,
                            HeightRequest = 40,
                            Command = new Command(() =>
                            {
                                var uds = DependencyService.Get<IUserDialogService>();
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
                            })
                        }
                    }
                };
            }
            else
            {
                enginesHeaderView=new BoxView()
                {
                    HeightRequest = 0
                };
                //enginesHeaderView = new Label()
                //{
                //    Text = "已安装的语音引擎",
                //    TextColor = Color.FromHex("#999"),
                //    FontSize = 14
                //};
            }
            enginesHeaderView.HorizontalOptions = LayoutOptions.FillAndExpand;

            if (engines.Count > 0)
            {
                engineView.Header = new Label()
                {
                    HeightRequest = 32,
                    Text = "已安装的引擎",
                    TextColor = Color.FromHex("#999"),
                    BackgroundColor = Color.FromHex("#eee"),
                    FontSize = 14,
                    YAlign = TextAlignment.Center
                };
                engineView.ItemsSource = engines;
                engineView.HeightRequest = 45 * engines.Count+35;
            }
            var layout = new StackLayout()
            {
                //VerticalOptions = LayoutOptions.Fill,
                Padding = 20,
                Spacing = 20,
                Children =
                {
                    //new BoxView()
                    //{
                    //    HeightRequest = 40,
                    //    BackgroundColor = Color.Blue
                    //},
                    new Label()
                    {
                        Text = "语音倒计时 "+DependencyService.Get<IServ>().GetAppVersionName(),
                        TextColor = Color.FromHex("#666"),
                        FontSize = 24,
                        XAlign = TextAlignment.Center
                    },
                    new Label()
                    {
                        Text = "分阶段 多频率 语音播报",
                        TextColor = Color.FromHex("#999"),
                        FontSize = 16,
                        XAlign = TextAlignment.Center
                    },
                    enginesHeaderView,
                    engineView,
                    new Label()
                    {
                        Text = "@好丫技术团队",
                        TextColor = Color.FromHex("#666"),
                        FontSize = 20,
                        XAlign = TextAlignment.Center,
                        FontAttributes = FontAttributes.Bold
                    },
                    new Label()
                    {
                        Text = "QQ交流群:205328850",
                        TextColor = Color.FromHex("#999"),
                        FontSize = 16,
                        XAlign = TextAlignment.Center
                    },
                    new Label()
                    {
                        Text = "邮箱:2862439512@qq.com",
                        TextColor = Color.FromHex("#999"),
                        FontSize = 16,
                        XAlign = TextAlignment.Center
                    }
                }
            };
            Content = new ScrollView()
            {
                Content = layout
            };
        }
    }
}
