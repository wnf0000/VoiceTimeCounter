using System;
using System.Collections.Generic;
using System.Linq;
using Acr.XamForms.UserDialogs;
using FormsApp.Model;
using FormsApp.Service;
using FormsApp.View;
//using FormsApp.ViewModel;
using Xamarin.Forms;

namespace FormsApp.Page
{
    class TimeSetCreatePage : ContentPage
    {
        private TimeSet ViewModel;
        private int Hours;
        private int Minutes;
        private int Seconds;
        private int ReadySeconds;
        //ObservableCollection<Stage> Stages { set; get; }

        private Entry entry;
        private ListView stageListView;
        public TimeSetCreatePage()
        {
            Title = "新建倒计时方案";

            BackgroundColor = Color.FromHex("#eee");
            ViewModel = new TimeSet();
            //ViewModel.AddStage(new FilnalStage(60));
            //ViewModel.AddStage(new Stage(100, 5, ""));
            BindingContext = ViewModel;
            ToolbarItems.Add(new ToolbarItem()
            {
                Icon = "ic_done_white_24dp.png",
                Text = "保存",
                Command = new Command(() =>
                {
                    saveButton_Clicked(this,EventArgs.Empty);
                })
            });
            InitView();
        }

        int ComputeDuration()
        {
            TimeSpan ts = new TimeSpan(0, Hours, Minutes, Seconds);
            return (int)ts.TotalSeconds;
        }
        void InitView()
        {
            var scrollView = new ScrollView()
            {
                //Padding = new Thickness(10, 20)
            };
            entry = new Entry() { Placeholder = "请输入名称" };
            entry.SetBinding(Entry.TextProperty, "Name");
            PanelView panelView1 = new PanelView()
            {
                Text = "名称",
                TextColor = Color.FromHex("999"),
                UnderLineColor = Color.FromHex("999"),
                Content = entry
            };

            Dictionary<string, int> hours = new Dictionary<string, int>();
            for (int i = 0; i <= 2; i++)
            {
                hours.Add(i + "小时", i);
            }
            Dictionary<string, int> minutes = new Dictionary<string, int>();
            for (int i = 0; i <= 59; i++)
            {
                minutes.Add(i + "分钟", i);
            }
            Dictionary<string, int> seconds = new Dictionary<string, int>();
            for (int i = 0; i <= 60; i++)
            {
                seconds.Add(i + "秒钟", i);
            }
            Dictionary<string, int> readySeconds = new Dictionary<string, int>();
            readySeconds.Add("无", 0);
            for (int i = 3; i <= 10; i++)
            {
                readySeconds.Add(i + "秒钟", i);
            }
            Picker hourPicker = new Picker()
            {
                Title = "小时",
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            foreach (var hour in hours)
            {
                hourPicker.Items.Add(hour.Key);
            }
            hourPicker.SelectedIndexChanged += delegate
            {
                if (hourPicker.SelectedIndex == -1)
                {
                    Hours = 0;
                }
                else
                {
                    Hours = hours.ToArray()[hourPicker.SelectedIndex].Value;
                }
                ViewModel.Duration = ComputeDuration();
            };
            Picker minutePicker = new Picker()
            {
                Title = "分钟",
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            foreach (var minute in minutes)
            {
                minutePicker.Items.Add(minute.Key);
            }

            minutePicker.SelectedIndexChanged += delegate
            {
                if (minutePicker.SelectedIndex == -1)
                {
                    Minutes = 0;
                }
                else
                {
                    Minutes = minutes.ToArray()[minutePicker.SelectedIndex].Value;
                }
                ViewModel.Duration = ComputeDuration();
            };
            Picker secondPicker = new Picker()
            {
                Title = "秒钟",
                HorizontalOptions = LayoutOptions.FillAndExpand

            };
            foreach (var second in seconds)
            {
                secondPicker.Items.Add(second.Key);
            }
            secondPicker.SelectedIndexChanged += delegate
            {
                if (secondPicker.SelectedIndex == -1)
                {
                    Seconds = 0;
                }
                else
                {
                    Seconds = seconds.ToArray()[secondPicker.SelectedIndex].Value;
                }
                ViewModel.Duration = ComputeDuration();
            };

            Picker readySecondPicker = new Picker()
            {
                Title = "准备时间",

            };
            foreach (var second in readySeconds)
            {
                readySecondPicker.Items.Add(second.Key);
            }
            readySecondPicker.SelectedIndexChanged += delegate
            {
                if (readySecondPicker.SelectedIndex == -1)
                {
                    ReadySeconds = 0;
                }
                else
                {
                    ReadySeconds = readySeconds.ToArray()[readySecondPicker.SelectedIndex].Value;
                }
                ViewModel.SecendsToReady = ReadySeconds;
            };

            PanelView panelView2 = new PanelView()
            {
                Text = "方案时长",
                TextColor = Color.FromHex("999"),
                UnderLineColor = Color.FromHex("999"),
                Content = new StackLayout()
                {
                    Orientation = StackOrientation.Horizontal,
                    Children = { hourPicker, minutePicker, secondPicker }
                }
            };
            PanelView panelView3 = new PanelView()
            {
                Text = "准备时间",
                TextColor = Color.FromHex("999"),
                UnderLineColor = Color.FromHex("999"),
                Content = readySecondPicker
            };

            //text to speak engines
            var textToSpeak = DependencyService.Get<ITextToSpeech>().New().Init();
            //textToSpeak.NoEngine += delegate
            //{
            //    DependencyService.Get<Acr.XamForms.UserDialogs.IUserDialogService>().ShowLoading("您的设备上没安装TTS引擎");
            //};
            PanelView engineView = null;
            if (textToSpeak.GetEngines().Count == 0)
            {

                var btn = new Button()
                 {
                     Text = "无语音引擎 点击看详情",
                     TextColor = Color.White,
                     BackgroundColor = Consts.ThemeColor
                 };

                btn.Clicked += delegate
                {
                    var uds = DependencyService.Get<Acr.XamForms.UserDialogs.IUserDialogService>();
                    uds.ActionSheet(new ActionSheetConfig()
                    {
                        Title="安装语音引擎",
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
                            new ActionSheetOption("下载安装Google文字转语音 非汉语", () =>
                            {
                                var url = "http://zhushou.360.cn/detail/index/soft_id/885077";
                                Device.OpenUri(new Uri(url));
                                
                            }),
                        }
                    });
                };

                engineView = new PanelView()
                {
                    Text = "语音引擎",
                    TextColor = Color.FromHex("999"),
                    UnderLineColor = Color.FromHex("999"),
                    Content = btn
                };
            }
            else
            {

                Dictionary<string, string> engines = new Dictionary<string, string>();
                foreach (var engine in textToSpeak.GetEngines())
                {
                    engines.Add(engine.Label, engine.Name);
                }
                Picker enginePicker = new Picker()
                {
                    Title = "选择TTS语音引擎",
                    HorizontalOptions = LayoutOptions.FillAndExpand
                };
                foreach (var engine in engines)
                {
                    enginePicker.Items.Add(engine.Key);
                }
                
                enginePicker.SelectedIndexChanged += delegate
                {
                    if (enginePicker.SelectedIndex == -1)
                    {
                        ViewModel.VoiceEngine = engines.ToArray()[0].Value;
                    }
                    else
                    {
                        ViewModel.VoiceEngine = engines.ToArray()[enginePicker.SelectedIndex].Value;
                    }

                };
                if (enginePicker.Items.Count == 1)
                {
                    enginePicker.SelectedIndex = 0;
                }
                else if (enginePicker.Items.Count > 1)
                {
                    enginePicker.SelectedIndex = 1;
                }
                engineView = new PanelView()
                {
                    Text = "语音引擎",
                    TextColor = Color.FromHex("999"),
                    UnderLineColor = Color.FromHex("999"),
                    Content = enginePicker
                };
            }
            textToSpeak.Dispose();
            stageListView = new ListView();
            var addStageBtn = new Button()
            {
                WidthRequest = 140,
                HeightRequest = 40,
                HorizontalOptions = LayoutOptions.Center,
                Text = "普通阶段",
                BackgroundColor = Consts.ThemeColor,
                TextColor = Color.White,
                Image = "ic_add_white_24dp.png"
            };
            addStageBtn.Clicked += addStageBtn_Clicked;
            var addFinalStageBtn = new Button()
            {
                WidthRequest = 140,
                HeightRequest = 40,
                HorizontalOptions = LayoutOptions.Center,
                Text = "冲刺阶段",
                BackgroundColor = Consts.ThemeColor,
                TextColor = Color.White,
                Image = "ic_add_white_24dp.png"
            };
            addFinalStageBtn.IsEnabled = !ViewModel.HasFinalStage;
            //addFinalStageBtn.SetBinding<TimeSet>(Button.IsEnabledProperty, w => w.DoNotveHasFinalStage);
            ViewModel.ObservableStages.CollectionChanged += delegate
            {
                addFinalStageBtn.IsEnabled = !ViewModel.HasFinalStage;
            };
            addFinalStageBtn.Clicked += addFinalStageBtn_Clicked;

            stageListView.Header = new StackLayout()
            {
                Padding = 10,
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Center,
                Children = { addStageBtn, addFinalStageBtn }
            };
            stageListView.SetBinding(ListView.ItemsSourceProperty, "ObservableStages");
            stageListView.ItemTemplate = new DataTemplate(typeof(StagesViewCell));
            stageListView.HeightRequest = ViewModel.ObservableStages.Count * 45 + 70;
            stageListView.ItemTapped += stageListView_ItemTapped;
            PanelView panelView5 = new PanelView()
            {
                Text = "播报阶段设置",
                TextColor = Color.FromHex("999"),
                UnderLineColor = Color.FromHex("999"),
                Content = stageListView
            };

            Button saveButton = new Button()
            {

                HeightRequest = 40,
                Text = "保存",
                BackgroundColor = Consts.ThemeColor,
                TextColor = Color.White,
            };
            saveButton.Clicked += saveButton_Clicked;

            LineView line0 = new LineView()
            {
                HeightRequest = 10,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                //BackgroundColor = Color.FromHex("#f7f7f7")
            };
            LineView line1 = new LineView()
            {
                HeightRequest = 8,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                //BackgroundColor = Color.FromHex("#f7f7f7")
            };
            LineView line2 = new LineView()
            {
                HeightRequest = 8,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                //BackgroundColor = Color.FromHex("#f7f7f7")
            };
            LineView line3 = new LineView()
            {
                HeightRequest = 8,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                //BackgroundColor = Color.FromHex("#f7f7f7")
            };
            LineView line4 = new LineView()
            {
                HeightRequest = 8,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                //BackgroundColor = Color.FromHex("#f7f7f7")
            };
            LineView line5 = new LineView()
            {
                HeightRequest = 8,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                //BackgroundColor = Color.FromHex("#f7f7f7")
            };
            scrollView.Content = new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                Children = {line0, panelView1, line1, panelView2, line2, panelView3, line3, engineView, line4, panelView5, //line5, 
                    new StackLayout()
                    {
                        Padding = new Thickness(10,20),Children = {saveButton}
                    } }
            };
            Content = scrollView;
            ViewModel.ObservableStages.CollectionChanged += ObservableStages_CollectionChanged;

        }

        void stageListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var m = e.Item as Stage;
            if (m.IsFinalStage)
            {
                EditFinalStagePage page = new EditFinalStagePage(ViewModel, m);
                Navigation.PushAsync(page);
            }
            else
            {
                EditStagePage page = new EditStagePage(ViewModel,m);
                Navigation.PushAsync(page);
            }
            
        }

        void addFinalStageBtn_Clicked(object sender, EventArgs e)
        {
            if (ViewModel.Duration < 1)
            {
                var toast = DependencyService.Get<IToast>();
                toast.Show("请先确定方案时长");
                //var uds = DependencyService.Get<Acr.XamForms.UserDialogs.IUserDialogService>();
                //uds.Toast("请先确定方案时长", 2);
                return;
            }
            AddFinalStagePage page = new AddFinalStagePage(ViewModel);
            Navigation.PushAsync(page);
        }

        void ObservableStages_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            stageListView.HeightRequest = ViewModel.ObservableStages.Count * 45 + 70;
        }

        void addStageBtn_Clicked(object sender, EventArgs e)
        {
            if (ViewModel.Duration < 1)
            {
                var toast = DependencyService.Get<IToast>();
                toast.Show("请先确定方案时长");
                //var uds = DependencyService.Get<Acr.XamForms.UserDialogs.IUserDialogService>();
                //uds.Toast("请先确定方案时长", 2);
                return;
            }
            var page = new AddStagePage(ViewModel);
            Navigation.PushAsync(page);
        }

        async void saveButton_Clicked(object sender, EventArgs e)
        {
            var uds = DependencyService.Get<Acr.XamForms.UserDialogs.IUserDialogService>();
            var toast = DependencyService.Get<IToast>();
            
            if (string.IsNullOrEmpty(ViewModel.Name))
            {
                
                entry.Focus();
                toast.Show("名称不能为空");
                //uds.Toast("名称不能为空",2);
                return;
            }

            if (ViewModel.Duration < 1)
            {
                toast.Show("请确定方案时长");
                //uds.Toast("请确定方案时长",2);
                return;
            }
            if (ViewModel.ObservableStages.Count < 1)
            {
                toast.Show("至少设置一个播报阶段");
                //uds.Toast("至少设置一个播报阶段", 2);
                return;
            }
            if (ViewModel.ObservableStages.Any(w => w.StagePoint > ViewModel.Duration))
            {
                toast.Show("阶段时间点不能大于方案时长");
                return;
            }
            uds.ShowLoading("正在保存");

            ViewModel.ListToStagesDataString();
            var result = await DbHelper.AddTimeSet(ViewModel) > 0;
            uds.HideLoading();
            toast.Show("保存成功");
            //uds.Toast("保存成功",2);
           await Navigation.PopAsync();
            MainPage.TimeSets.Add(ViewModel);
        }



        class StagesViewCell : ViewCell
        {
            public StagesViewCell()
            {
                Label label = new Label()
                {
                    YAlign = TextAlignment.Center,
                    TextColor = Color.FromHex("#666"),
                    HorizontalOptions = LayoutOptions.FillAndExpand
                };
                label.SetBinding(Label.TextProperty, "Desc");
                Image removeBtn = new Image()
                {
                    Source = "ic_remove_circle_outline_grey600_24dp.png",
                    WidthRequest = 30,
                    HeightRequest = 30
                };
                removeBtn.GestureRecognizers.Add(new TapGestureRecognizer()
                {
                    Command = new Command(() =>
                    {
                        //var timeSet = this.ParentView.BindingContext as TimeSet;
                        //if (timeSet == null) return;
                        //var vm = this.BindingContext as Stage;
                        //var uds = DependencyService.Get<Acr.XamForms.UserDialogs.IUserDialogService>();
                        var timeSet = this.ParentView.BindingContext as TimeSet;
                        if (timeSet == null) return;
                        var uds = DependencyService.Get<Acr.XamForms.UserDialogs.IUserDialogService>();
                        //if (timeSet.ObservableStages.Count == 1)
                        //{
                        //    uds.Alert(new AlertConfig()
                        //    {
                        //        Message = "至少保留一个阶段",
                        //        OkText = "好的",
                        //        Title = "提示"
                        //    });
                        //    return;
                        //}
                        var vm = this.BindingContext as Stage;
                        uds.Confirm(new ConfirmConfig()
                        {
                            CancelText = "取消",
                            Message = "确定删除该阶段吗？",
                            OkText = "确定",
                            Title = "提示",
                            OnConfirm = (r) =>
                            {
                                if (r)
                                {
                                    timeSet.RemoveStage(vm);
                                }
                            }
                        });
                    })
                });
                StackLayout layout = new StackLayout()
                {
                    Padding = new Thickness(10, 0),
                    BackgroundColor = Color.White,
                    Orientation = StackOrientation.Horizontal,
                    Children = { label, removeBtn }
                };
                View = layout;
            }
        }
    }
    
}
