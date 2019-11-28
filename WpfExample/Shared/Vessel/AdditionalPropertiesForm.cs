using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows;
using UIMessager.Services.Message;
using Resources;
using Resources.Validators;
using DBServices.UsualEntity;
using BaseLib.Services;

namespace Shared.Vessel
{
    public partial class AdditionalPropertiesForm
    {
        #region Properties
        ObservableCollection<ShipProperty> ShipPropertyList
        {
            get { return (ObservableCollection<ShipProperty>)GridOfProperties.ItemsSource; }
            set { GridOfProperties.ItemsSource = value; }
        }

        ShipProperty EditingShipProperty
        {
            get { return (ShipProperty)EditPanel.DataContext; }
            set { EditPanel.DataContext = value; }
        }

        public static DependencyProperty SelectShipPropertyProperty;
        public ShipProperty SelectShipProperty
        {
            get { return (ShipProperty)GetValue(SelectShipPropertyProperty); }
            set { SetValue(SelectShipPropertyProperty, value); }
        }
        #endregion

        #region Loader
        public bool Load(ShipProperty selectProperty = null)
        {
            if (selectProperty == null && SelectShipProperty != null) selectProperty = SelectShipProperty;

            ShipPropertyList = ShipManager.GetShipProperties();
            if (ShipPropertyList == null) return false;
            Select(selectProperty);

            return true;
        }

        void Select(ShipProperty property)
        {
            SelectShipProperty = (property != null) ? ShipPropertyList.Where(p => p.Id == property.Id).FirstOrDefault() : null;
            if (SelectShipProperty != null) GridOfProperties.ScrollIntoView(SelectShipProperty);
        }


        void ShowSelectShipProperty()
        {
            EditingShipProperty = SelectShipProperty;
            Action = (SelectShipProperty == null) ? ActionMode.Empty : ActionMode.Viewing;
        }
        #endregion

        #region Get Ready
        void GetReadyForAdding()
        {
            EditingShipProperty = new ShipProperty();
            Action = ActionMode.Adding;
            txtName.Focus();
        }

        bool GetReadyForEditing()
        {
            try
            {
                EditingShipProperty = new ShipProperty(SelectShipProperty.Id);

                Action = ActionMode.Editing;
                txtName.Focus();
                return true;
            }
            catch (UserMsg.UserMsgException uExp) { uExp.Show(); }
            catch (Exception exp) { new UserMsg().Show(MsgError.BaseType.Loading, exp); }
            return false;
        }
        #endregion

        #region Actions
        bool Add()
        {
            if (ValidateHelper.HasErrors(EditPanel, true))
            {
                MsgError.Show(global::UIMessager.Properties.Resources.mErIncorrectInputDate);
                return false;
            }
            try
            {
                EditingShipProperty.AddComplex();
                return true;
            }
            catch (UserMsg.UserMsgException uExp) { uExp.Show(); }
            catch (Exception exp) { new UserMsg().Show(MsgError.BaseType.Adding, exp); }
            return false;
        }

        bool Save(out bool WasSaved)
        {
            WasSaved = false;
            if (ValidateHelper.HasErrors(EditPanel, false))
            {
                MsgError.Show(global::UIMessager.Properties.Resources.mErIncorrectInputDate);
                return false;
            }
            try
            {
                WasSaved = EditingShipProperty.Save();
                return true;
            }
            catch (UserMsg.UserMsgException uExp) { uExp.Show(); }
            catch (Exception exp) { new UserMsg().Show(MsgError.BaseType.Saving, exp); }
            return false;
        }

        bool Delete()
        {
            try
            {
                if (MsgConfirm.Show(Properties.Resources.mConfirmDelShipProperty) == MessageBoxResult.No) return false;
                SelectShipProperty.Delete();
                return true;
            }
            catch (UserMsg.UserMsgException uExp) { uExp.Show(); }
            catch (Exception exp) { new UserMsg().Show(MsgError.BaseType.Deleting, exp); }
            return false;
        }
        #endregion
    }
}
