using System;
using System.Collections.Generic;
using System.Linq;
using FormsApp.Model;
using FormsApp.Service;
using FormsApp.View;
using Xamarin.Forms;

namespace FormsApp.Page
{
    class AddStagePage : ContentPage
    {
        private int Hours;
        private int Minutes;
        private int Seconds;
        private int VoiceInterval;
        private TimeSet TimeSet { set; get; }
        Stage ViewModel { set; get; }
        public AddStagePage(TimeSet timeSet)
        {
            Title = "新增普通阶段";
            BackgroundColor = Color.FromHex("#eee");
            TimeSet = timeSet;
            ViewModel = new Stage();
            BindingContext = ViewModel;
            InitView();
        }

        void InitView()
        {
            var scrollView = new ScrollView()
            {
                //Padding = new Thickness(10, 20)
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
            for (int i = 2; i <= 60; i++)
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
            };



            Picker readySecondPicker = new Picker()
            {
                Title = "秒钟",

            };
            foreach (var second in readySeconds)
            {
                readySecondPicker.Items.Add(second.Key);
            }
            readySecondPicker.SelectedIndex = readySeconds.ToList().FindIndex(w => w.Value == ViewModel.VoiceInterval);
            readySecondPicker.SelectedIndexChanged += delegate
            {
                if (readySecondPicker.SelectedIndex == -1)
                {
                    VoiceInterval = readySeconds.ToArray()[0].Value; ;
                }
                else
                {
                    VoiceInterval = readySeconds.ToArray()[readySecondPicker.SelectedIndex].Value;
                }
                //ViewModel.VoiceInterval = VoiceInterval;
            };

            PanelView panelView1 = new PanelView()
            {
                Text = "阶段时间点",
                TextColor = Color.FromHex("999"),
                UnderLineColor = Color.FromHex("999"),
                Content = new StackLayout()
                {
                    Orientation = StackOrientation.Horizontal,
                    Children = { hourPicker, minutePicker, secondPicker }
                }
            };
            PanelView panelView2 = new PanelView()
            {
                Text = "播报间隔时间",
                TextColor = Color.FromHex("999"),
                UnderLineColor = Color.FromHex("999"),
                Content = readySecondPicker
            };

            Button saveButton = new Button()
            {

                HeightRequest = 40,
                Text = "确定",
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
            scrollView.Content = new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                Children = { line0,panelView1, line1, panelView2, line2, new StackLayout()
                {
                    Padding = 10,
                    Children = {saveButton}
                } }
            };
            Content = scrollView;
            //Content = new StackLayout()
            //{
            //    Padding = 10,
            //    Orientation = StackOrientation.Vertical,
            //    Children = { panelView1, line1, panelView2, line2, saveButton }
            //};
        }

        void saveButton_Clicked(object sender, System.EventArgs e)
        {
            var uds = DependencyService.Get<Acr.XamForms.UserDialogs.IUserDialogService>();
            var toast = DependencyService.Get<IToast>();
            TimeSpan ts = new TimeSpan(0, Hours, Minutes, Seconds);
            if (ts.TotalSeconds < 1)
            {
                toast.Show("请确定阶段时间点");
                //uds.Toast("请确定阶段时间点", 2);
                return;
            }
            if (ts.TotalSeconds > TimeSet.Duration)
            {
                toast.Show("阶段时间点不能超过方案的总时长");
                //uds.Toast("阶段时间点不能超过方案的总时长", 2);
                return;
            }
            if (TimeSet.ObservableStages.Count(w => w != ViewModel && w.StagePoint == (int)ts.TotalSeconds) > 0)
            {
                toast.Show("已存在此阶段时间点");
               // uds.Toast("已存在此阶段时间点", 2);
                return;
            }
            var finalStage = TimeSet.GetFinalStage();
            if (finalStage != null && (int)ts.TotalSeconds < finalStage.StagePoint)
            {
                toast.Show("普通阶段时间点不能小于冲刺阶段时间点");
                //uds.Toast("普通阶段时间点不能小于冲刺阶段时间点", 2);
                return;
            }
            ViewModel.StagePoint = (int)ts.TotalSeconds;
            if (VoiceInterval > 0)
                ViewModel.VoiceInterval = VoiceInterval;
            if (ViewModel.VoiceInterval==0)
            {
                toast.Show("请确定播报间隔时间");
                //uds.Toast("请确定播报间隔时间", 2);
                return;
            }
            TimeSet.AddStage(ViewModel);

            Navigation.PopAsync();

        }


    }
}