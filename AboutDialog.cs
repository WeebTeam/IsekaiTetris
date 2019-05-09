/*
 * Made by SharpDevelop.
 * User: user
 * Date: 08.11.2013
 * Time: 18:18
 * 
 * Для изменения этого шаблона используйте Сервис | Настройка | Кодирование | Правка стандартных заголовков.
 * To change this template use Tools | Customize | Coding | Edit standard headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Tetris
{
	/// <summary>
	/// Диалог "О программе"
	/// Dialogue "About the program"
	/// </summary>
	public partial class AboutDialog : Form
	{
		public AboutDialog()
		{
			InitializeComponent();
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			DialogResult=DialogResult.OK;
		}
		
		void LinkLabel1LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			try
			{
				System.Diagnostics.Process.Start("http://vk.com/imaginarius");
			}
			catch
			{
				MessageBox.Show("Your OS has failed to open the link -_-",
				                "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		
		void LinkLabel2LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			try
			{
				System.Diagnostics.Process.Start("http://www.everaldo.com/");
			}
			catch
			{
				MessageBox.Show("Your OS has failed to open the link -_-",
				                "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}
