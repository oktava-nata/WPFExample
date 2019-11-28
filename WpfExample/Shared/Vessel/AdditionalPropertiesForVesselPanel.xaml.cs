using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DBServices;
using System.Collections.ObjectModel;
using DBServices.UsualEntity;

namespace Shared.Vessel
{
    /// <summary>
    /// Логика взаимодействия для AdditionalPropertiesForVesselPanel.xaml
    /// </summary>
    public partial class AdditionalPropertiesForVesselPanel : UserControl
    {
        public AdditionalPropertiesForVesselPanel()
        {
            InitializeComponent();
        }

    // Ksanti  - перенесем во вью-модель
    //    public static DependencyProperty IsViewModeProperty;
    //    public bool IsViewMode
    //    {
    //        get { return (bool)GetValue(IsViewModeProperty); }
    //        set { SetValue(IsViewModeProperty, value); }
    //    }
        
    //    public static DependencyProperty ShipPropertyValuesProperty;
    //    public ObservableCollection<UI_ShipPropertyValue> ShipPropertyValues
    //    {
    //        get { return (ObservableCollection<UI_ShipPropertyValue>)GetValue(ShipPropertyValuesProperty); }
    //        set { SetValue(ShipPropertyValuesProperty, value); }
    //    }

    //    #region Constructors
    //    public AdditionalPropertiesForVesselPanel()
    //    {
    //        InitializeComponent();
    //    }

    //    static AdditionalPropertiesForVesselPanel()
    //    {
    //        FrameworkPropertyMetadata metadataShipPropertyValues = new FrameworkPropertyMetadata(null);
    //        ShipPropertyValuesProperty = DependencyProperty.Register("ShipPropertyValues", typeof(ObservableCollection<UI_ShipPropertyValue>),
    //            typeof(AdditionalPropertiesForVesselPanel), metadataShipPropertyValues);

    //        FrameworkPropertyMetadata metadataIsViewMode = new FrameworkPropertyMetadata(false);
    //        IsViewModeProperty = DependencyProperty.Register("IsViewMode", typeof(bool), typeof(AdditionalPropertiesForVesselPanel), metadataIsViewMode);
    //    }

    //    #endregion

    //    public bool LoadForViewing(Ship ship)
    //    {
    //        ShipPropertyValues = UI_ShipPropertyValue.GetPropertyValuesForShip(ship);
    //        return ShipPropertyValues != null;
    //    }

    //    public bool LoadForEditing(Ship ship)
    //    {
    //        ShipPropertyValues = UI_ShipPropertyValue.GetAllPropertyWithShipValues(ship);
    //        return ShipPropertyValues != null;
    //    }

    //    public bool LoadForAdding(Ship ship)
    //    {
    //        if (ship.PropertyValues==null) ship.PropertyValues = new ObservableCollection<ShipPropertyValue>();
    //        ShipPropertyValues = UI_ShipPropertyValue.GetAllProperty(ship);
    //        return ShipPropertyValues != null;
    //    }
    }
    
}
