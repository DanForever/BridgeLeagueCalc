using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Runtime.CompilerServices;

namespace BridgeLeagueCalc
{
	/// <summary>
	/// Interaction logic for LoadCSVDialog.xaml
	/// </summary>
	public partial class LoadCSVDialog : Window, INotifyPropertyChanged
	{
		private string _delimiter = ";";

		public string File { get; set; }
		public string Delimiter
		{
			get { return _delimiter; }
			set { _delimiter = value; }
		}

		public LoadCSVDialog()
		{
			InitializeComponent();
			DataContext = this;
		}

		private void OnOk( object sender, RoutedEventArgs e )
		{
			if( OkClicked != null )
			{
				OkClicked( File, Delimiter );
			}

			Close();
		}

		private void OnCancel( object sender, RoutedEventArgs e )
		{
			Close();
		}

		public event PropertyChangedEventHandler PropertyChanged;
		protected void OnPropertyChanged( [CallerMemberName] string name = "" )
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if ( handler != null )
			{
				handler( this, new PropertyChangedEventArgs( name ) );
			}
		}

		public delegate void OkClickedDelegate( string filepath, string delimeter );
		public event OkClickedDelegate OkClicked;
	}
}
