using System;
using FormsApp.Model;
using FormsApp.Service;
using FormsApp.View;
using Xamarin.Forms;

namespace FormsApp.Page
{
    class TimeSetRunPage : ContentPage
    {
        TimeSet TimeSet { set; get; }
        private ITextToSpeech _textToSpeech;
        private Performer _performer;
        private ISoundService _soundService;
        private bool _isEnglish=false;

        public TimeSetRunPage(TimeSet timeSet)
        {
            this.TimeSet = timeSet;
            _isEnglish = timeSet.IsEnglish;
            Title = "执行倒计时";
            BackgroundColor = Color.FromHex("#eee");
            _textToSpeech = DependencyService.Get<ITextToSpeech>().New();
            _textToSpeech.Inited += _textToSpeech_Init;
            _textToSpeech.InitedError += _textToSpeech_InitedError;
            _textToSpeech.Init(TimeSet.VoiceEngine);

            _soundService = DependencyService.Get<ISoundService>();

            //
            _performer = new Performer(this.TimeSet);
            _performer.Speak += performer_Speak;
            _performer.PrepareTick += performer_PrepareTick;
            _performer.Tick += performer_HeartBeat;
            _performer.CurrentSecondChanged += performer_CurrentSecondChanged;
            _performer.StageChanged += performer_StageChanged;
            InitView();
            //_textToSpeech.Speak(timeSet.Name);
        }

        void _textToSpeech_InitedError(object sender, EventArgs e)
        {
            DependencyService.Get<IToast>().Show("语音引擎初始化失败");
        }

        void _textToSpeech_Init(object sender, EventArgs e)
        {
            //var uds = DependencyService.Get<Acr.XamForms.UserDialogs.IUserDialogService>();
            //uds.Toast("1");
            //if (_textToSpeech.CurrentEngine !=TimeSet.VoiceEngine)
            //{
            //    _textToSpeech.SetEngine(TimeSet.VoiceEngine);
            //}
            //_textToSpeech.SetEngine(TimeSet.VoiceEngine);
            if(_isEnglish)
            _textToSpeech.SetLanguage(Language.en_US);
            else _textToSpeech.SetLanguage(Language.zh_CN);
        }

        void performer_StageChanged(object sender, Performer.StageChangedEventArgs e)
        {
            try
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    _stageListView.SelectedItem = e.CurrentStage;
                });
            }
            catch (Exception)
            { }
        }

        void performer_PrepareTick(object sender, Performer.PrepareTickEventArgs e)
        {
            try
            {
                if (e.IsAtStartPoint)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        _circle.Text = "准备";
                    });
                    //var timeSet = sender as TimeSet;
                    //if (_textToSpeech.CurrentEngine != timeSet.VoiceEngine)
                    //{
                    //    _textToSpeech.SetEngine(timeSet.VoiceEngine);
                    //}
                    //var text = string.Format("{0}秒准备", e.TotalSeconds);
                    string text = "";
                    if (_isEnglish)
                    {
                        text = string.Format("{0} second{1} for ready ", e.TotalSeconds, e.TotalSeconds > 1 ? "s" : "");
                    }
                    else
                    {
                        text = string.Format("{0}秒准备", e.TotalSeconds);
                    }
                    _textToSpeech.Speak(text);
                }
                _soundService.PlayDi();
            }
            catch (Exception)
            { }
        }

        void performer_CurrentSecondChanged(object sender, Performer.SecondChangedEvenArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                try
                {
                    _circle.Text = e.Current.ToString();
                    if (e.Current == 0)
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            _startBtn.IsEnabled = true;
                            _stopBtn.IsEnabled = false;
                            _pauseBtn.IsEnabled = false;
                        });
                        _circle.RotateTo(360, length: 500).ContinueWith(r =>
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                _circle.Text = "结束";
                            });

                        });
                    }
                }
                catch (Exception)
                { }
            });

        }

        void performer_HeartBeat(object sender, EventArgs e)
        {
            try
            {
                _soundService.PlayDong();
            }
            catch (Exception)
            { }
        }

        void performer_Speak(object sender, Performer.SpeakEventArgs e)
        {
            try
            {
                //if (e.VoiceInterval > 2)
                //{
                //    var ts = TimeSpan.FromSeconds(e.Seconds);
                //    var text = "";
                //    var hours = ts.Hours;
                //    var minutes = ts.Minutes;
                //    var seconds = ts.Seconds;
                //    if (_isEnglish)
                //    {
                //        if (hours > 0)
                //        {
                //            text += string.Format("{0} hour{1}", hours, hours > 1 ? "s" : "");
                //        }
                //        if (minutes > 0)
                //        {
                //            text += string.Format(" {0} minute{1}", minutes, minutes > 1 ? "s" : "");
                //        }
                //        if (seconds > 0)
                //        {
                //            text += string.Format(" and {0} second{1}", seconds, seconds > 1 ? "s" : "");
                //        }
                //    }
                //    else
                //    {
                //        if (hours > 0)
                //        {
                //            text += string.Format("{0}小时", hours);
                //        }
                //        if (minutes > 0)
                //        {
                //            text += string.Format("{0}分", minutes);
                //        }
                //        if (seconds > 0)
                //        {
                //            text += string.Format("{0}秒", seconds);
                //        }
                //    }
                //    _textToSpeech.Speak(text);
                //}
                //else
                //{
                //    _textToSpeech.Speak(e.Seconds.ToString());
                //}
                _textToSpeech.Speak(e.Text);
            }
            catch (Exception)
            { }
        }

        private UCircle _circle;
        private ListView _stageListView;
        private Button _startBtn;
        private Button _stopBtn;
        private Button _pauseBtn;
        void InitView()
        {

            Label nameLabel = new Label()
            {
                Text = TimeSet.Name,
                YAlign = TextAlignment.Center,
                FontSize = 16,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.FromHex("#666"),
                //HeightRequest = 32,

            };
            Label detaiLabel = new Label()
            {
                Text = TimeSet.FormatedDuration + (TimeSet.SecendsToReady > 0 ? " 准备时间：" + TimeSet.SecendsToReady + "秒" : ""),
                YAlign = TextAlignment.Center,
                FontSize = 14,
                TextColor = Color.FromHex("#999"),
                HeightRequest = 24,
            };
            _circle = new UCircle()
            {
                Radius = 80,
                //FillColor = Color.Green,
                FillColor = TimeSet.Color,
                BorderColor = Color.FromHex("#f7f7f7"),
                TextColor = Color.White,
                BorderWidth = 5,
                Font = Font.SystemFontOfSize(56),
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Center,
            };
            //_circle.Text = _performer.Position.ToString();
            _circle.Text = TimeSet.Duration.ToString();
            _startBtn = new Button()
            {
                Text = "开始",
                WidthRequest = 100,
                BackgroundColor = Color.Red,
                HorizontalOptions = LayoutOptions.Center,
                Image = "ic_play_arrow_white_24dp.png"

            };
            _stopBtn = new Button()
           {
               Text = "停止",
               WidthRequest = 100,
               BackgroundColor = Color.Gray,
               HorizontalOptions = LayoutOptions.Center,
               Image = "ic_stop_white_24dp.png",
               IsEnabled = false,
           };
            _pauseBtn = new Button()
            {
                Text = "暂停",
                WidthRequest = 100,
                BackgroundColor = Color.Gray,
                HorizontalOptions = LayoutOptions.Center,
                Image = "ic_pause_white_24dp.png",
                IsEnabled = false,

            };
            _startBtn.Clicked += delegate
            {
                _startBtn.IsEnabled = false;
                _stopBtn.IsEnabled = true;
                _pauseBtn.IsEnabled = true;
                _performer.Start();
            };
            _stopBtn.Clicked += delegate
            {
                _startBtn.IsEnabled = true;
                _stopBtn.IsEnabled = false;
                _pauseBtn.IsEnabled = false;
                _performer.Stop();
            };
            _pauseBtn.Clicked += delegate
            {
                _startBtn.IsEnabled = true;
                _stopBtn.IsEnabled = false;
                _pauseBtn.IsEnabled = false;
                _performer.Pause();
            };
            StackLayout layout1 = new StackLayout()
            {
                Padding = 10,
                BackgroundColor = Color.White,
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    new StackLayout()
                    {
                        
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Orientation = StackOrientation.Horizontal,
                        Children = { 
                            _circle, 
                            new StackLayout()
                        {
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            Children = {_startBtn, _pauseBtn, _stopBtn }
                        }}
                    }
                }
            };
            Label stageLabel = new Label()
            {
                Text = "播报阶段",
                YAlign = TextAlignment.Center,
                FontSize = 14,
                TextColor = Color.FromHex("#666"),

            };
            _stageListView = new ListView();
            _stageListView.BackgroundColor = Color.White;
            _stageListView.HeightRequest = 45 * TimeSet.ObservableStages.Count;
            _stageListView.VerticalOptions = LayoutOptions.FillAndExpand;
            _stageListView.IsEnabled = false;
            _stageListView.BindingContext = TimeSet;
            _stageListView.SetBinding(ListView.ItemsSourceProperty, "ObservableStages");
            Content = new ScrollView()
            {
                Content = new StackLayout()
                {
                    Padding = 10,

                    Orientation = StackOrientation.Vertical,
                    Children =
                {
                    new StackLayout()
                    {
                        //BackgroundColor = Color.FromHex("#f7f7f7"),
                        BackgroundColor = Color.White,
                        Padding = 5,
                        Orientation = StackOrientation.Vertical,
                        Spacing = 0,
                        Children =
                        {
                            nameLabel,detaiLabel
                        }
                    }, layout1, new StackLayout()
                    {
                        //BackgroundColor = Color.FromHex("#f7f7f7"),
                        Padding = 5,Children = {stageLabel}
                    }, _stageListView
                }
                }
            };
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            if (_performer != null)
                _performer.Dispose();
            if (_textToSpeech != null)
                _textToSpeech.Dispose();
            //_soundService.Dispose();
            _performer = null;
            _textToSpeech = null;
            //_soundService = null;
        }
    }
}
