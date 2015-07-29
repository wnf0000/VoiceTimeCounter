//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using FormsApp.Model;
//using SQLite;

//namespace FormsApp.ViewModel
//{
//    public class TimeSetViewModel:TimeSet
//    {
//        //public TimeSetViewModel(IList<Stage> set=null)
//        //{
//        //    if (set != null) ObservableStages = new ObservableCollection<Stage>();
//        //}
//        //public ObservableCollection<Stage> ObservableStages { get; private set; }
        
//        private ObservableCollection<Stage> _ObservableStages;
        
//        public ObservableCollection<Stage> ObservableStages
//        {
//            get { return _ObservableStages ?? (_ObservableStages = new ObservableCollection<Stage>(base.Stages())); }
//        }
//    }
//}
