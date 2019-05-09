/*
 * Created by SharpDevelop.
 * User: user
 * Date: 25.10.2013
 * Time: 2:22
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Tetris
{
	/// <summary>
	/// Форма ввода имени пользователя
	/// Username form
	/// </summary>
	public partial class NewRecordDialog : Form
	{
		public string UserName {get; set;}
		public NewRecordDialog()
		{
			InitializeComponent();
			if(UserName!="")
				UNameTextBox.Text=UserName;
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			UNameTextBox.Text=UNameTextBox.Text.Trim(' ');
			if(UNameTextBox.Text=="") return;
			UserName=UNameTextBox.Text;
			DialogResult=DialogResult.OK;
		}
	}
}
