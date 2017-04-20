﻿using System;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.IO.Compression;
using System.Threading;

namespace InstaPy
{
	public partial class Form1 : Form
	{

		string[] InstaPyfiles = { "__init.py", "clariafai.py", "comment_util.py", "instapy.py", "login_util.py", "like_util.py", "print_log_writer.py", "time_util.py", "unfollow_util.py" };
		bool nostart = false;
		string FILENAME = "quickstart.py";
		bool cant = true;
		bool cant2 = true;
		bool cant3 = true;
		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{

			string path = Directory.GetCurrentDirectory() +@"\instapy";
			
			if (Directory.Exists(path))
			{
				foreach (var item in InstaPyfiles)
				{
					if (!File.Exists(path + @"\" + item))
					{
						missing.Text = "Some InstaPy files are missing.";
						missing.ForeColor = System.Drawing.Color.Red;
						nostart = false;
						break;
					}
				}
				path = Directory.GetCurrentDirectory() + @"\assets";
				if (!File.Exists(path + @"\chromedriver.exe"))
				{
					missing.Text = "Chromedriver is missing.";
					missing.ForeColor = System.Drawing.Color.Red;
					nostart = false;

				}
				else
				{
					nostart = true;
					missing.Text = "All InstaPy files are there.";
					missing.ForeColor = System.Drawing.Color.Green;
				}
			}else
			{
				nostart = false;
				missing.Text = "instapy folder is missing.";
				missing.ForeColor = System.Drawing.Color.Red;
			}
			
			
			// Shows intro message to read all info / usage of the program
			if (Properties.Settings.Default.readmedont)
			{
				MessageBox.Show("Please read info/usage before any program start.", "Read me", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			if (!Properties.Settings.Default.usernpass.Equals(string.Empty))
			{
				
				string[] del = { };
				del = Properties.Settings.Default.usernpass.Split('|');
				username_txt.Text = del[0];
				pass_txt.Text = del[1];
			}
		}
		
		private void button1_Click(object sender, EventArgs e)
		{

		}

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// Shows About page for program
			About bout = new About();
			bout.Show();
			//MessageBox.Show("GUI Tool for InstaPy script."+Environment.NewLine+Environment.NewLine + "Vesrion: 1.0" + Environment.NewLine + "Built in C#." + Environment.NewLine + "MIT License." + Environment.NewLine+Environment.NewLine +"InstaPy is utomation Script for \"farming\" Likes, Comments and Followers on Instagram." + Environment.NewLine + "Implemented in Python using the Selenium module." + Environment.NewLine + "MIT License.", "About",MessageBoxButtons.OK,MessageBoxIcon.Information);
		}

		private void button1_Click_1(object sender, EventArgs e)
		{
			if (!nostart)
			{
				MessageBox.Show("Some files are missing!! Download all files from File menu.","ERROR",MessageBoxButtons.OK,MessageBoxIcon.Error);
			}
			else
			{
				#region IMPORT
				// Import part of python file
				string import = "from instapy import InstaPy" + Environment.NewLine + "import os" + Environment.NewLine;
				File.WriteAllText(FILENAME, import);
				#endregion

				#region USRNAME N PASSWORD

				// Username and Password processing 
				string usernpass = "";
				if (username_txt.Text.Equals(string.Empty) || pass_txt.Text.Equals(string.Empty))
				{
					// Show error message if some of the field is empty
					MessageBox.Show("ERROR: Username or Password are empty !", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);

					// Set focus to Username
					username_txt.Focus();
					cant = false;
				}
				else
				{
					cant = true;
					// If there is something in textboxes fill in line for sesion and login
					usernpass = "session = InstaPy(username='" + username_txt.Text + "', password='" + pass_txt.Text + "')" + Environment.NewLine + "session.login()" + Environment.NewLine;
					File.AppendAllText(FILENAME, usernpass);
				}
				if (remember.Checked)
				{
					string settingsuser = username_txt.Text + "|" + pass_txt.Text;
					Properties.Settings.Default.usernpass = settingsuser;
					Properties.Settings.Default.Save();
				}
				#endregion

				#region LIKE RESTRICTION TAGS
				/*====================================================================================
				*		Like Restriction Tags option
				*			# searches the description for the given words and won't
				*			# like the image if one of the words are in there
				*			
				*====================================================================================*/

				string[] likeRestrictionStrings = { };
				string likeRestrictionLine = "session.set_dont_like([";

				// Checks if option is selected
				if (likerestrict.Checked)
				{
					
					// If selected but no tags shows error message		

					if (likerestrict.Text.Equals(string.Empty))
					{

						MessageBox.Show("ERROR: No tags detected. Write some tags or deselect this option.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);

						likerestrict.Focus();

					}
					else likeRestrictionStrings = likerestrict_txt.Text.Split(',');
					foreach (var item in likeRestrictionStrings)
					{
						// If there is emtpy tag skip it
						if (item.Equals(string.Empty))
						{
							continue;
						}
						else likeRestrictionLine += "'" + item + "', ";
					}
					// Close line with bracket
					likeRestrictionLine = likeRestrictionLine.Remove(likeRestrictionLine.Length - 2, 1) + "])" + Environment.NewLine;

					// Write in file
					File.AppendAllText(FILENAME, likeRestrictionLine);
				}
#endregion

				#region LIKE RESTRICTION USERS
				/*====================================================================================
				*		Like Restriction Users option
				*			# searches the description for the given words and won't
				*			# like the image if one of the words are in there
				*			
				*====================================================================================*/

				string[] likeRestrictionUsersStrings = { };
				string likeRestrictionUsersLine = "session.set_ignore_users([";

				// Checks if option is selected
				if (restrictlikesusers.Checked)
				{
					// If selected but no tags shows error message		

					if (restrlikesusers.Text.Equals(string.Empty))
					{

						MessageBox.Show("ERROR: No people detected. Write some people or deselect this option.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);

						restrlikesusers.Focus();

					}
					else likeRestrictionUsersStrings = restrlikesusers.Text.Split(',');
					foreach (var item in likeRestrictionUsersStrings)
					{
						// If there is emtpy tag skip it
						if (item.Equals(string.Empty))
						{
							continue;
						}
						else likeRestrictionUsersLine += "'" + item + "', ";
					}
					// Close line with bracket
					likeRestrictionUsersLine = likeRestrictionUsersLine.Remove(likeRestrictionUsersLine.Length - 2, 1) + "])" + Environment.NewLine;

					// Write in file
					File.AppendAllText(FILENAME, likeRestrictionUsersLine);
				}
#endregion

				#region IGNORING RESTRICTION
				/*=======================================================================================
				*		Ignoring restriction python function
				*		Process it all tags in textbox or shows error if there is no tags
				*			#will ignore the don't like if the description contains
				*			# one of the given words
				*			
				*========================================================================================*/

				string[] ignoreRestrictionStrings = { };
				string ignoreRestrictionLine = "session.set_ignore_if_contains([";

				//if checked import is done and if not it will skip this block of code
				if (restrictignore.Checked)
				{

					// if checked and there is no tags show error message to input some tags or deselect option
					if (restrictignore_txt.Text.Equals(string.Empty))
					{
						// MessageBox is shown
						MessageBox.Show("ERROR: No tags detected. Write some tags or deselect the option.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);

						//Tags field gets in focus to type
						restrictignore_txt.Focus();
					}
					// if there is tags separated with comma ( , ) process it and add
					else ignoreRestrictionStrings = restrictignore_txt.Text.Split(',');

					// Goes for every tag to add in string
					foreach (var item in ignoreRestrictionStrings)
					{
						// If there is empty tag caused with accident comma (ie. " dog, ") just continue
						if (item.Equals(string.Empty))
						{
							continue;
						}
						// If there is tag add it in line 
						else ignoreRestrictionLine += "'" + item + "', ";
					}

					// Removes processed comma to add closing bracket
					ignoreRestrictionLine = ignoreRestrictionLine.Remove(ignoreRestrictionLine.Length - 2, 1) + "])" + Environment.NewLine;

					File.AppendAllText(FILENAME, ignoreRestrictionLine);
				}
				#endregion

				#region FRIEND EXCLUSION
				/*=================================================================================================
				*		Friend exclusion 
				*		# will prevent commenting on and unfollowing your good friends 
				*		# (the images will still be liked)
				* 
				*=================================================================================================*/


				string[] friendExclusionStrings = { };
				string friendExclusionLine = "session.friend_list = [";

				// If Option is checked process everything
				if (friendexcl.Checked)
				{
					if (friendexcl_txt.Text.Equals(string.Empty))
					{
						// MessageBox is shown
						MessageBox.Show("ERROR: No friends detected. Write some friends or deselect the option.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);

						//Tags field gets in focus to type
						friendexcl_txt.Focus();
					}
					else friendExclusionStrings = friendexcl_txt.Text.Split(',');
					// Goes for every tag to add in string
					foreach (var item in friendExclusionStrings)
					{
						// If there is empty tag caused with accident comma (ie. " dog, ") just continue
						if (item.Equals(string.Empty))
						{
							continue;
						}
						// If there is tag add it in line 
						else friendExclusionLine += "'" + item + "', ";
					}

					// Removes processed comma to add closing bracket
					friendExclusionLine = friendExclusionLine.Remove(friendExclusionLine.Length - 2, 1) + "]" + Environment.NewLine;

					File.AppendAllText(FILENAME, friendExclusionLine);
				}
#endregion

				#region COMMENTING
				/*=============================================================================================
				 *			Commenting
				 *				Enables commenting 
				 *				Set percentage and custom comments
				 *=============================================================================================*/

				string commentLine = "session.set_do_comment(enabled=True, percentage=";
				string commentSetLine = "session.set_comments([";
				string[] comments = { };
				if (comment.Checked)
				{
					// Set percentage for commenting
					commentLine += comment_percent.Value.ToString() + ")" + Environment.NewLine;

					if (comment_cust_txt.Text.Equals(string.Empty))
					{
						// MessageBox is shown
						MessageBox.Show("ERROR: No comments detected. Write some comments or deselect the option.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);

						// Comments field gets in focus to type
						comment_cust_txt.Focus();
					}
					else
					{
						comments = comment_cust_txt.Text.Split(',');
						// Goes for every comment to add in string
						foreach (var item in comments)
						{
							// If there is empty comment caused with accident comma (ie. " nice, ") just continue
							if (item.Equals(string.Empty))
							{
								continue;
							}
							// If there is comment add it in line 
							else {
								if (emojisupport.Checked)
								{
									commentSetLine += "u'" + item + "', ";
								}else commentSetLine += "'" + item + "', ";
							} 
						}
						// Removes processed comma to add closing bracket
						commentSetLine = commentSetLine.Remove(commentSetLine.Length - 2, 1) + "])" + Environment.NewLine;

						File.AppendAllText(FILENAME, commentLine);
						File.AppendAllText(FILENAME, commentSetLine);
					}
				}
				#endregion

				#region FOLLOW
				/*===============================================================================================
				 *			Following
				 *				Set percentage to follow every x/100th user
				 * 
				 * =============================================================================================*/
				if (following.Checked)
				{
					// Set following to true and read percent and times
					string follow = "session.set_do_follow(enabled=True, percentage="
						+ following_percent.Value.ToString() + ", times="
						+ followtimes.Value.ToString() + ")" + Environment.NewLine;

					File.AppendAllText(FILENAME, follow);
				}
				#endregion

				#region UNFOLLOW
				/*====================================================================================================
				 *			Unfollowing
				 *			#unfollows 10 of the accounts you're following -> instagram will only unfollow 10 before
				 *			you'll be 'blocked for 10 minutes'
				 *			
				 *			(if you enter a higher number than 10 it will unfollow 10,
				 *			then wait 10 minutes and will continue then)
				 *			
				 *======================================================================================================*/

				if (unfollow.Checked)
				{
					string unfollow = "session.unfollow_users(amount=" + unfollow_nmbr.Value.ToString() + ")" + Environment.NewLine;

					File.AppendAllText(FILENAME, unfollow);
				}
				#endregion

				#region FOLLOWER NUMBERS
				/*=====================================================================================================
				 *			Interactions based on the number of followers a user has
				 *			
				 *			UPPER FOLLOWER COUNT -> This is used to check the number of followers a user has and
				 *				if this number exceeds the number set then no further interaction happens
				 *				
				 *			LOWER FOLLOWER COUNT -> #This is used to check the number of followers a user has and
				 *				if this number does not pass the number set then no further interaction happens
				 *				
				 *======================================================================================================*/

				if (upperfc.Checked)
				{
					string upper = "session.set_upper_follower_count(limit = " + upperfc_percent.Value.ToString() + ")" + Environment.NewLine;

					File.AppendAllText(FILENAME, upper);
				}

				if (lowerfc.Checked)
				{
					string lower = "session.set_lower_follower_count(limit = " + lowerfc_percent.Value.ToString() + ")" + Environment.NewLine;

					File.AppendAllText(FILENAME, lower);
				}
				#endregion

				#region FOLLOW FROM LIST
				/*========================================================================
				 *			Follow from list
				 * 
				 * ========================================================================*/
				if (followfromlist.Checked)
				{
					string followfromlist2 = "session.follow_by_list(accs, times=1)"+Environment.NewLine;
					string accs = "accs = ['";
					string[] acc = { };
					if (!list_txt.Text.Equals(string.Empty))
					{
						acc = list_txt.Text.Split(',');

						foreach (var item in acc)
						{
							string item2 = item;
							item2 += "\',\'";
							accs += item2;
						}
						accs = accs.Remove(accs.Length - 2,2) + "]" +Environment.NewLine;

						File.AppendAllText(FILENAME, accs);
						File.AppendAllText(FILENAME, followfromlist2);
					}
					else MessageBox.Show("Write down some users.","ATTENTION",MessageBoxButtons.OK,MessageBoxIcon.Warning);

				}
#endregion

				#region LIKE FROM TAGS
				/*========================================================================
				 *			Likes from tags
				 * 
				 * ========================================================================*/

				if (likefromtags.Checked)
				{
					string likesFromTagsLine = "session.like_by_tags([";
					string[] tags = { };
					if (likesfromtags_txt.Text.Equals(string.Empty))
					{
						// MessageBox is shown
						MessageBox.Show("ERROR: No tags detected. Write some tags.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);

						// Comments field gets in focus to type
						likesfromtags_txt.Focus();
						cant2 = false;
					}
					else
					{
						tags = likesfromtags_txt.Text.Split(',');
						cant2 = true;
						// Goes for every tag to add in string
						foreach (var item in tags)
						{
							// If there is empty tag caused with accident comma (ie. " nice, ") just continue
							if (item.Equals(string.Empty))
							{
								continue;
							}
							// If there is tag add it in line 
							else likesFromTagsLine += "'" + item + "', ";
						}
						likesFromTagsLine = likesFromTagsLine.Remove(likesFromTagsLine.Length - 2, 2) + "], amount=" + likes_nmbr.Value.ToString();

						if (likefromtagsphoto.Checked && likefromtagsvideo.Checked || !likefromtagsphoto.Checked && !likefromtagsvideo.Checked)
						{
							// Removes processed comma to add closing bracket
							likesFromTagsLine += ")" + Environment.NewLine;
						}
						else if (likefromtagsphoto.Checked && !likefromtagsvideo.Checked)
						{
							likesFromTagsLine += ", media='Photo')" + Environment.NewLine;
						}
						else if (!likefromtagsphoto.Checked && likefromtagsvideo.Checked)
						{
							likesFromTagsLine += ", media='Video')" + Environment.NewLine;
						}



						File.AppendAllText(FILENAME, likesFromTagsLine);
					}
				}
				#endregion

				#region LIKE FROM IMAGE
				/*========================================================================
				 *			Likes from image
				 * 
				 * ========================================================================*/

				if (likefromimage.Checked)
				{
					if (url.Text.Equals(string.Empty))
					{
						MessageBox.Show("ERROR: URL is invalid. Paste valid URL.","ERROR",MessageBoxButtons.OK,MessageBoxIcon.Error);
						cant3 = false;
					}
					else
					{

						cant3 = true;	
						string likesFromImage = "session.like_from_image(url='" + url.Text + "',amount=" + likesfromimage_nmb.Value.ToString();

						if (likefromimagephoto.Checked && likefromimagevideo.Checked || !likefromimagephoto.Checked && !likefromimagevideo.Checked)
						{
							likesFromImage += ")";
						}
						else if (likefromimagephoto.Checked && !likefromimagevideo.Checked)
						{
							likesFromImage += ", media='Photo')";
						}
						else if (!likefromimagephoto.Checked && likefromimagevideo.Checked)
						{
							likesFromImage += ", media='Video')";
						}
						else
						{
							likesFromImage += ")";
						}

						File.AppendAllText(FILENAME, likesFromImage);
					}
				}
				#endregion

				#region RUN
				/*========================================================================
				 *			Closing file and running it
				 * 
				 * ========================================================================*/
				if (cant && cant2 &&cant3)
				{
					File.AppendAllText(FILENAME, Environment.NewLine + "session.end()");

					File.WriteAllText("Start.bat", "set PYTHONIOENCODING=UTF-8" + Environment.NewLine + "py " + FILENAME);

					System.Diagnostics.Process.Start("Start.bat");
				}
				

				#endregion
			}
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			
		}

		private void infoToolStripMenuItem_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start("https://github.com/timgrossmann/InstaPy#getting-started");

			
		}

		private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			
		}

		private void deleteLoginInfoToolStripMenuItem_Click(object sender, EventArgs e)
		{
			
		}

		private void dontShowReadmeInfoToolStripMenuItem_Click(object sender, EventArgs e)
		{
			
			
		}

		private void downloadInstaPyToolStripMenuItem_Click(object sender, EventArgs e)
		{

			
		}

		private void downloadChromedriverToolStripMenuItem_Click(object sender, EventArgs e)
		{
			
		}

		private void likes_nmbr_ValueChanged(object sender, EventArgs e)
		{
			if(likes_nmbr.Value > 1000)
			{
				MessageBox.Show("ATTENTION: Putting amount of likes above 1000 may BAN your account."+Environment.NewLine+"We are not responsible if your account get BAN.","ATTENTION",MessageBoxButtons.OK,MessageBoxIcon.Warning);
			}
		}

		private void likefromtags_CheckedChanged(object sender, EventArgs e)
		{
			if (likefromtags.Checked)
			{
				panel8.BackColor = System.Drawing.Color.LightGreen;
			}
			else panel8.BackColor = System.Drawing.Color.LightSalmon;
			if (!likefromtags.Checked)
			{

				likefromimage.Checked = true;
			}
		}

		private void likefromimage_CheckedChanged(object sender, EventArgs e)
		{
			if (likefromimage.Checked)
			{
				panel11.BackColor = System.Drawing.Color.LightGreen;
			}
			else panel11.BackColor = System.Drawing.Color.LightSalmon;
			if (!likefromimage.Checked)
			{
				likefromtags.Checked = true;
			}
		}

		private void likerestrict_CheckedChanged(object sender, EventArgs e)
		{
			if (likerestrict.Checked)
			{
				panel1.BackColor = System.Drawing.Color.LightGreen;
			}else panel1.BackColor = System.Drawing.Color.LightSalmon;
			
		}

		private void restrictlikesusers_CheckedChanged(object sender, EventArgs e)
		{
			if (restrictlikesusers.Checked)
			{
				panel10.BackColor = System.Drawing.Color.LightGreen;
			}
			else panel10.BackColor = System.Drawing.Color.LightSalmon;
		}

		private void restrictignore_CheckedChanged(object sender, EventArgs e)
		{
			if (restrictignore.Checked)
			{
				panel2.BackColor = System.Drawing.Color.LightGreen;
			}
			else panel2.BackColor = System.Drawing.Color.LightSalmon;
		}

		private void comment_CheckedChanged(object sender, EventArgs e)
		{
			if (comment.Checked)
			{
				panel3.BackColor = System.Drawing.Color.LightGreen;
			}
			else panel3.BackColor = System.Drawing.Color.LightSalmon;
		}

		private void following_CheckedChanged(object sender, EventArgs e)
		{
			if (following.Checked)
			{
				panel4.BackColor = System.Drawing.Color.LightGreen;
			}
			else panel4.BackColor = System.Drawing.Color.LightSalmon;
		}

		private void friendexcl_CheckedChanged(object sender, EventArgs e)
		{
			if (friendexcl.Checked)
			{
				panel5.BackColor = System.Drawing.Color.LightGreen;
			}
			else panel5.BackColor = System.Drawing.Color.LightSalmon;
		}

		private void followfromlist_CheckedChanged(object sender, EventArgs e)
		{
			if (followfromlist.Checked)
			{
				panel12.BackColor = System.Drawing.Color.LightGreen;
			}
			else panel12.BackColor = System.Drawing.Color.LightSalmon;
		}

		private void upperfc_CheckedChanged(object sender, EventArgs e)
		{
			if (upperfc.Checked || lowerfc.Checked)
			{
				panel6.BackColor = System.Drawing.Color.LightGreen;
			}
			else panel6.BackColor = System.Drawing.Color.LightSalmon;
		}

		private void lowerfc_CheckedChanged(object sender, EventArgs e)
		{
			if (upperfc.Checked || lowerfc.Checked)
			{
				panel6.BackColor = System.Drawing.Color.LightGreen;
			}
			else panel6.BackColor = System.Drawing.Color.LightSalmon;
		}

		private void unfollow_CheckedChanged(object sender, EventArgs e)
		{
			if (unfollow.Checked)
			{
				panel7.BackColor = System.Drawing.Color.LightGreen;
			}
			else panel7.BackColor = System.Drawing.Color.LightSalmon;
		}

		private void deleteCredentialsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			MessageBox.Show("Login info deleted.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
			Properties.Settings.Default.usernpass = "";
			Properties.Settings.Default.Save();
		}

		private void dontShowReadmeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (Properties.Settings.Default.readmedont)
			{
				Properties.Settings.Default.readmedont = false;
				dontShowReadmeToolStripMenuItem.Text = "Do show Readme";
				Properties.Settings.Default.Save();
			}
			else
			{
				Properties.Settings.Default.readmedont = true;
				dontShowReadmeToolStripMenuItem.Text = "Don't show Readme";
				Properties.Settings.Default.Save();
			}
		}

		private void downloadInstaPyFilesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (var client = new WebClient())
			{
				client.DownloadFile("https://github.com/timgrossmann/InstaPy/archive/master.zip", "master.zip");
			}



			MessageBox.Show("Extract .zip file and copy InstaPy-GUI.exe and paste it inside InstaPy-master folder.", "ATTENTION", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			this.Close();
		}

		private void downloadChromedriverToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start("https://sites.google.com/a/chromium.org/chromedriver/downloads");
		}

		private void exitToolStripMenuItem2_Click(object sender, EventArgs e)
		{
			this.Close();

		}
	}
}
