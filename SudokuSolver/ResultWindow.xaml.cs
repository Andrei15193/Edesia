using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
namespace SudokuSolver
{
	public partial class ResultWindow
		: Window, INotifyPropertyChanged
	{
		public ResultWindow(IEnumerable<IReadOnlyDictionary<SudokuPosition, int>> solutions)
		{
			_sudokuMatrix = new int[9][];
			for (int index = 0; index < 9; index++)
				_sudokuMatrix[index] = new int[9];

			IEnumerator<IReadOnlyDictionary<SudokuPosition, int>> solution = solutions.GetEnumerator();

			_nextSolutionCommand = new DelegateCommand(delegate
			{
				if (solution.MoveNext())
				{
					foreach (var cell in solution.Current)
						_sudokuMatrix[cell.Key.Row][cell.Key.Column] = cell.Value;
					_sudokuMatrixWithInitialValues = solution.Current.OrderBy(cell => cell.Key.Row)
																	 .ThenBy(cell => cell.Key.Column)
																	 .ToList();

					if (PropertyChanged != null)
						PropertyChanged(this, new PropertyChangedEventArgs("SudokuMatrix"));
				}
				else
					MessageBox.Show("You ran out of solutions!", "No solutions left", MessageBoxButton.OK, MessageBoxImage.Information);
			});
			_saveSolutionCommand = new DelegateCommand(delegate
				{
					SaveFileDialog saveFileDialog = new SaveFileDialog
					{
						AddExtension = true,
						CheckFileExists = false,
						Filter = "HTML Document (*.html) | *.html",
						InitialDirectory = Environment.GetEnvironmentVariable("CSIDL_COMMON_DOCUMENTS")
					};
					if (saveFileDialog.ShowDialog(this).GetValueOrDefault())
						try
						{
							StringBuilder solutionStringBuilder = new StringBuilder();

							solutionStringBuilder.Append("<!DOCTYPE html>");
							solutionStringBuilder.Append("<html>");
							solutionStringBuilder.Append("<head>");
							solutionStringBuilder.Append("<title>Sudoku solution</title>");
							solutionStringBuilder.Append("<style>");
							solutionStringBuilder.Append("body { font-family: Arial; }");
							solutionStringBuilder.Append("table { border-collapse: collapse; width: 270px; text-align: center; }");
							solutionStringBuilder.Append("table tr { height: 30px; }");
							solutionStringBuilder.Append("table, table tr, table tr td { border: 1px solid black; }");
							solutionStringBuilder.Append("</style>");
							solutionStringBuilder.Append("</head>");
							solutionStringBuilder.Append("<body>");
							solutionStringBuilder.Append("<table>");
							solutionStringBuilder.Append("<caption>Sudoku solution</caption>");
							for (int row = 0; row < 9; row++)
							{
								if (row == 3 || row == 6)
									solutionStringBuilder.Append("<tr style=\"border-top-width: 2px;\">");
								else
									solutionStringBuilder.Append("<tr>");
								for (int column = 0; column < 9; column++)
								{
									if (column == 3 || column == 6)
										if (_sudokuMatrixWithInitialValues[row * 9 + column].Key.IsSet)
											solutionStringBuilder.Append("<td style=\"background-color: SpringGreen; border-left-width: 2px;\">");
										else
											solutionStringBuilder.Append("<td style=\"border-left-width: 2px;\">");
									else
										if (_sudokuMatrixWithInitialValues[row * 9 + column].Key.IsSet)
											solutionStringBuilder.Append("<td style=\"background-color: SpringGreen;\">");
										else
											solutionStringBuilder.Append("<td>");
									solutionStringBuilder.Append(_sudokuMatrix[row][column]);
									solutionStringBuilder.Append("</td>");
								}
								solutionStringBuilder.Append("</tr>");
							}
							solutionStringBuilder.Append("</table>");
							solutionStringBuilder.Append("</body>");
							solutionStringBuilder.Append("</html>");

							using (StreamWriter streamWriter = new StreamWriter(saveFileDialog.FileName))
							{
								streamWriter.WriteLine(solutionStringBuilder.ToString());
							}
						}
						catch
						{
							MessageBox.Show(this, "Could not save sudoku solution!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
						}
				});

			InitializeComponent();
			Loaded += delegate
			{
				_nextSolutionCommand.Execute(null);
			};
		}

		#region INotifyPropertyChanged Members
		public event PropertyChangedEventHandler PropertyChanged;
		#endregion

		public int[][] SudokuMatrix
		{
			get
			{
				return _sudokuMatrix;
			}
		}
		public ICommand NextSolutionCommand
		{
			get
			{
				return _nextSolutionCommand;
			}
		}
		public ICommand SaveSolutionCommand
		{
			get
			{
				return _saveSolutionCommand;
			}
		}

		private IReadOnlyList<KeyValuePair<SudokuPosition, int>> _sudokuMatrixWithInitialValues;
		private readonly int[][] _sudokuMatrix;
		private readonly ICommand _nextSolutionCommand;
		private readonly ICommand _saveSolutionCommand;
	}
}