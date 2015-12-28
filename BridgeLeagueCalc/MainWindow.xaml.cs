using System.Diagnostics;
using System.IO;
using System.Windows;

namespace BridgeLeagueCalc
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			
			_dropTarget.DragEnter += _dropTarget_DragEnter;
			_dropTarget.DragLeave += _dropTarget_DragLeave;
			_dropTarget.DragOver += _dropTarget_DragOver;
			_dropTarget.Drop += _dropTarget_Drop;
		}

		private void _dropTarget_DragLeave( object sender, DragEventArgs e )
		{
			_dropTargetContents.Visibility = Visibility.Hidden;
		}

		private void _dropTarget_DragEnter( object sender, DragEventArgs e )
		{
			_dropTargetContents.Visibility = Visibility.Visible;
		}

		private void _dropTarget_DragOver( object sender, DragEventArgs e )
		{
			if ( e.Data.GetDataPresent( DataFormats.FileDrop ) )
			{
				e.Effects = DragDropEffects.All;
			}
			else
			{
				e.Effects = DragDropEffects.None;
			}
			e.Handled = false;
		}

		private void _dropTarget_Drop( object sender, DragEventArgs e )
		{
			if ( e.Data.GetDataPresent( DataFormats.FileDrop ) )
			{
				string[] docPath = (string[])e.Data.GetData(DataFormats.FileDrop);

				LoadCSVDialog dialog = new LoadCSVDialog()
				{
					File = docPath[ 0 ]
				};

				dialog.OkClicked += ImportCsv;

				dialog.Show();
			}

			_dropTargetContents.Visibility = Visibility.Hidden;
		}

		private void ImportCsv( string file, string delimiterStr )
		{
			char delimiter = delimiterStr[ 0 ];

			Parser parser = new Parser();

			parser.Parse( file, delimiter );
		}
	}
}
