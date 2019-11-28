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
using BaseLib;
using DBServices.UsualEntity;
using System.Collections.ObjectModel;

namespace Shared.Vessels
{
    /// <summary>
    /// Логика взаимодействия для VesselChoicePanel.xaml
    /// </summary>
    public partial class VesselChoicePanel : UserControl
    {
        public VesselChoicePanel()
        {
            InitializeComponent();
           
        }

        //Olga 22.04.15
        //Чтобы popup "выбора судна" не зависал, если щелкнуть по шапке таблицы 
        private void tab_GotMouseCapture(object sender, MouseEventArgs e)
        {
            Mouse.Capture(tab);
        }
    }
}
