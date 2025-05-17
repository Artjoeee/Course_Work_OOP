using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Sportics.View
{
    /// <summary>
    /// Логика взаимодействия для CoachesWindow.xaml
    /// </summary>
    public partial class CoachesWindow : Window
    {
        public CoachesWindow()
        {
            InitializeComponent();
        }

        public static implicit operator CoachesWindow(MembershipsWindow v)
        {
            throw new NotImplementedException();
        }
    }
}
