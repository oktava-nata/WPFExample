using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Commands;
using ViewModelBaseSolutions.Items;

namespace Shared.ViewModels.Panels
{
    public class SelectedInfoViewModel<T> : Microsoft.Practices.Prism.ViewModel.NotificationObject
    {
        #region Commands
        public DelegateCommand SelectAllCommand { get; private set; }
        public DelegateCommand DeselectCommand { get; private set; }
        #endregion

        ListCheckedItemViewModel<T> List;

        #region Properties
        public bool IsEnabled
        {
            get { return _IsEnabled; }
            set { _IsEnabled = value; RaisePropertyChanged(() => this.IsEnabled); }
        }
        bool _IsEnabled;

        public string SelectedText
        {
            get { return _SelectedText; }
            set { _SelectedText = value; RaisePropertyChanged(() => this.SelectedText); }
        }
        string _SelectedText;
        #endregion

        public SelectedInfoViewModel(ListCheckedItemViewModel<T> list)
        {
            this.SelectAllCommand = new DelegateCommand(this.OnSelectAllCommand_Executed);
            this.DeselectCommand = new DelegateCommand(this.OnDeselectCommand_Executed);
            List = list;
            List.OnCheckedChanged = UpdateSelectedText;
            UpdateSelectedText();
        }

        public void UpdateSelectedText( int selected,int all)
        {
            SelectedText = string.Format(Properties.Resources.txtSelectedOf, selected, all);
        }

        void UpdateSelectedText()
        {
            UpdateSelectedText(List.Selected, List.All);
        }

        #region On Command
        void OnSelectAllCommand_Executed()
        {
            List.CheckedAll();
        }

        void OnDeselectCommand_Executed()
        {
            List.UncheckedAll();
        }
        #endregion
    }
    //public class SelectedInfoViewModel : Microsoft.Practices.Prism.ViewModel.NotificationObject
    //{
    //    #region Commands
    //    public DelegateCommand SelectAllCommand { get; private set; }
    //    public DelegateCommand DeselectCommand { get; private set; }        
    //    #endregion

    //    Action OnSelectAll;
    //    Action OnDeselect;

    //    #region Properties   
    //    public bool IsEnabled
    //    {
    //        get { return _IsEnabled; }
    //        set { _IsEnabled = value; RaisePropertyChanged(() => this.IsEnabled); }
    //    }
    //    bool _IsEnabled;        

    //    public string SelectedText
    //    {
    //        get { return _SelectedText; }
    //        set { _SelectedText = value; RaisePropertyChanged(() => this.SelectedText); }
    //    }
    //    string _SelectedText;
    //    #endregion

    //    public SelectedInfoViewModel(ListSimpleCheckedItemViewModel<T>)
    //    {
    //        this.SelectAllCommand = new DelegateCommand(this.OnSelectAllCommand_Executed);
    //        this.DeselectCommand = new DelegateCommand(this.OnDeselectCommand_Executed);
    //        OnSelectAll = SelectAll;
    //        OnDeselect = Deselect;
    //    }    

    //    public void UpdateSelectedText(int all, int selected)
    //    {
    //        SelectedText = string.Format(Properties.Resources.txtSelectedOf,selected, all);               
    //    }

    //    #region On Command
    //    void OnSelectAllCommand_Executed()
    //    {
    //        if (OnSelectAll != null)
    //            OnSelectAll();
    //    }

    //    void OnDeselectCommand_Executed()
    //    {
    //        if (OnDeselect != null)
    //            OnDeselect();
    //    }
    //    #endregion
    //}

    //public class SelectedInfoViewModel : Microsoft.Practices.Prism.ViewModel.NotificationObject
    //{
    //    #region Commands
    //    public DelegateCommand SelectAllCommand { get; private set; }
    //    public DelegateCommand DeselectCommand { get; private set; }
    //    #endregion

    //    Action OnSelectAll;
    //    Action OnDeselect;

    //    #region Properties
    //    public bool IsEnabled
    //    {
    //        get { return _IsEnabled; }
    //        set { _IsEnabled = value; RaisePropertyChanged(() => this.IsEnabled); }
    //    }
    //    bool _IsEnabled;

    //    public string SelectedText
    //    {
    //        get { return _SelectedText; }
    //        set { _SelectedText = value; RaisePropertyChanged(() => this.SelectedText); }
    //    }
    //    string _SelectedText;
    //    #endregion

    //    public SelectedInfoViewModel(Action SelectAll, Action Deselect)
    //    {
    //        this.SelectAllCommand = new DelegateCommand(this.OnSelectAllCommand_Executed);
    //        this.DeselectCommand = new DelegateCommand(this.OnDeselectCommand_Executed);
    //        OnSelectAll = SelectAll;
    //        OnDeselect = Deselect;
    //    }

    //    public void UpdateSelectedText(int all, int selected)
    //    {
    //        SelectedText = string.Format(Properties.Resources.txtSelectedOf, selected, all);
    //    }

    //    #region On Command
    //    void OnSelectAllCommand_Executed()
    //    {
    //        if (OnSelectAll != null)
    //            OnSelectAll();
    //    }

    //    void OnDeselectCommand_Executed()
    //    {
    //        if (OnDeselect != null)
    //            OnDeselect();
    //    }
    //    #endregion
    //}
}
