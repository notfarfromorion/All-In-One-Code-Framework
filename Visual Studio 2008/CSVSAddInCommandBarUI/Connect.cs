/***************************************** Module Header *****************************************\
* Module Name:  Connect.cs
* Project:      CSVSAddInCommandBarUI
* Copyright (c) Microsoft Corporation.
* 
* The CSVSAddInCommandBarUI project demostrates how to display Visual Studio add-in in toolbar or 
* menubar. 
* 
* Visual Studio offers three kinds of CommandBar objects:
* 
* 1. Toolbars — Contain one or more menu bars.
* 
* 2. Menu bars — Commands on toolbars, such as File, Edit, and View.
* 
* 3. Shortcut menus (also known as context or popup menus.) — Menus that appear on the screen when 
* you right-click a menu or object (such as a file or project). Submenus cascade off menu commands 
* or off shortcut menus. Shortcut menus are similar to other menus in Visual Studio. However, you 
* access them by pointing to an arrow in a menu bar, or by right-clicking an item in the integrated 
* development environment (IDE).
* 
* This sample displays three commandbars:
* 
* 1. In Menubar -> Tool menu -> All-In-One Command
* 2. In Toolbar -> All-In-One command
* 3. In Task List toolbox -> right click content menu -> All-In-One command
* 
* This source is subject to the Microsoft Public License.
* See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
* All other rights reserved.
* 
* History:
* * 5/4/2009 0:35 AM Hongye Sun Created
\*************************************************************************************************/

#region Using directives
using System;
using Extensibility;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.CommandBars;
using System.Resources;
using System.Reflection;
using System.Globalization;
#endregion


namespace CSVSAddInCommandBarUI
{
    /// <summary>
    /// The object for implementing an Add-in.
    /// </summary>
    /// <seealso class='IDTExtensibility2' />
	public class Connect : IDTExtensibility2, IDTCommandTarget
	{
		/// <summary>
        /// Implements the constructor for the Add-in object. Place your initialization code 
        /// within this method.
        /// </summary>
		public Connect()
		{
		}

		/// <summary>
        /// Implements the OnConnection method of the IDTExtensibility2 interface. Receives 
        /// notification that the Add-in is being loaded.
        /// </summary>
		/// <param term='application'>Root object of the host application.</param>
		/// <param term='connectMode'>Describes how the Add-in is being loaded.</param>
		/// <param term='addInInst'>Object representing this Add-in.</param>
		/// <seealso class='IDTExtensibility2' />
		public void OnConnection(object application, ext_ConnectMode connectMode, 
            object addInInst, ref Array custom)
		{
			_applicationObject = (DTE2)application;
			_addInInstance = (AddIn)addInInst;

			if(connectMode == ext_ConnectMode.ext_cm_UISetup)
			{
                object[] contextGUIDS = new object[] { };
                Commands2 commands = (Commands2)_applicationObject.Commands;
                Command command = null;

                // This try/catch block can be duplicated if you wish to add multiple commands to 
                // be handled by your Add-in, just make sure you also update the QueryStatus/Exec
                // method to include the new command names.
                try
                {
                    ///////////////////////////////////////////////////////////////////////////////
                    // Step3. In the add-in's Connect class and OnConnection() procedure, create 
                    // command with name CSVSAddInCommandBarUI and set its icon ID as 6743, which
                    // is red star instead of its default smiley face icon.
                    // 

                    // Add a command to the Commands collection:
                    command = commands.AddNamedCommand2(
                        _addInInstance,
                        "CSVSAddInCommandBarUI",
                        "CSVSAddInCommandBarUI",
                        "Executes the command for CSVSAddInCommandBarUI",
                        true,
                        6743,
                        ref contextGUIDS,
                        (int)vsCommandStatus.vsCommandStatusSupported
                        + (int)vsCommandStatus.vsCommandStatusEnabled,
                        (int)vsCommandStyle.vsCommandStylePictAndText,
                        vsCommandControlType.vsCommandControlTypeButton);
                }
                catch (System.ArgumentException)
                {
                    // If we are here, then the exception is probably because a command with that 
                    // name already exists. If so there is no need to recreate the command and we  
                    // can safely ignore the exception.
                }

                if (command == null)
                    return;

                AddCommandToToolMenubar(_applicationObject, command);
                AddCommandToToolbar(_applicationObject, command);
                AddCommandToShortcutMenu(_applicationObject, command);
			}
		}


        /// <summary>
        /// Step4. Add command to Tool menubar by getting CommandBar with name "MenuBar" and its 
        /// child CommandBarPopup control "Tools" and adding command into it. 
        /// </summary>
        /// <param name="applicationObject"></param>
        /// <param name="command"></param>
        private void AddCommandToToolMenubar(DTE2 applicationObject, Command command)
        {
            string toolsMenuName = "Tools";

            // Place the command on the tools menu.
            // Find the MenuBar command bar, which is the top-level command bar holding all the 
            // main menu items:
            CommandBar menuBarCommandBar = 
                ((CommandBars)applicationObject.CommandBars)["MenuBar"];

            //Find the Tools command bar on the MenuBar command bar:
            CommandBarControl toolsControl = menuBarCommandBar.Controls[toolsMenuName];
            CommandBarPopup toolsPopup = (CommandBarPopup)toolsControl;

            //Add a control for the command to the tools menu:
            if ((command != null) && (toolsPopup != null))
            {
                CommandBarControl commandBarControl =
                    (CommandBarControl) command.AddControl(toolsPopup.CommandBar, 1);
                commandBarControl.Caption = "All-In-One Add-In";
            }
        }

        /// <summary>
        /// Step5. Add command to toolbar by getting CommandBar "Standard" and adding command into 
        /// its last item, then set its caption.
        /// </summary>
        /// <param name="applicationObject"></param>
        /// <param name="command"></param>
        private void AddCommandToToolbar(DTE2 applicationObject, Command command)
        {
            // Place the command button on the toolbar menu.
            
            // Find the toolbar
            CommandBar toolbarCommandBar = ((CommandBars)applicationObject.CommandBars)["Standard"];

            // Add command button in the toolbar
            CommandBarControl commandBarControl = (CommandBarControl)command.AddControl(
                    toolbarCommandBar, toolbarCommandBar.Controls.Count + 1);
            commandBarControl.Caption = "All-In-One Add-In";
        }

        /// <summary>
        /// Step6. Add command to shortcut menu by getting CommandBar "Task List" and adding  
        /// command into its last item, then set its caption 
        /// </summary>
        /// <param name="applicationObject"></param>
        /// <param name="command"></param>
        private void AddCommandToShortcutMenu(DTE2 applicationObject, Command command)
        {
            // Place the command button on the toolbar menu.

            // Find the toolbar
            CommandBar toolbarCommandBar = ((CommandBars)applicationObject.CommandBars)["Task List"];

            // Add command button in the toolbar
            CommandBarControl commandBarControl = (CommandBarControl)command.AddControl(
                    toolbarCommandBar, toolbarCommandBar.Controls.Count + 1);
            commandBarControl.Caption = "All-In-One Add-In";
        }

		/// <summary>
        /// Implements the OnDisconnection method of the IDTExtensibility2 interface. Receives 
        /// notification that the Add-in is being unloaded.
        /// </summary>
		/// <param term='disconnectMode'>Describes how the Add-in is being unloaded.</param>
		/// <param term='custom'>Array of parameters that are host application specific.</param>
		/// <seealso class='IDTExtensibility2' />
		public void OnDisconnection(ext_DisconnectMode disconnectMode, ref Array custom)
		{
		}

		/// <summary>
        /// Implements the OnAddInsUpdate method of the IDTExtensibility2 interface. Receives 
        /// notification when the collection of Add-ins has changed.<
        /// /summary>
		/// <param term='custom'>Array of parameters that are host application specific.</param>
		/// <seealso class='IDTExtensibility2' />		
		public void OnAddInsUpdate(ref Array custom)
		{
		}

		/// <summary>
        /// Implements the OnStartupComplete method of the IDTExtensibility2 interface. Receives 
        /// notification that the host application has completed loading.</summary>
		/// <param term='custom'>Array of parameters that are host application specific.</param>
		/// <seealso class='IDTExtensibility2' />
		public void OnStartupComplete(ref Array custom)
		{
		}

		/// <summary>
        /// Implements the OnBeginShutdown method of the IDTExtensibility2 interface. Receives 
        /// notification that the host application is being unloaded.</summary>
		/// <param term='custom'>Array of parameters that are host application specific.</param>
		/// <seealso class='IDTExtensibility2' />
		public void OnBeginShutdown(ref Array custom)
		{
		}
		
		/// <summary>
        /// Implements the QueryStatus method of the IDTCommandTarget interface. This is called 
        /// when the command's availability is updated
        /// </summary>
		/// <param term='commandName'>The name of the command to determine state for.</param>
		/// <param term='neededText'>Text that is needed for the command.</param>
		/// <param term='status'>The state of the command in the user interface.</param>
		/// <param term='commandText'>Text requested by the neededText parameter.</param>
		/// <seealso class='Exec' />
		public void QueryStatus(string commandName, vsCommandStatusTextWanted neededText, 
            ref vsCommandStatus status, ref object commandText)
		{
			if(neededText == vsCommandStatusTextWanted.vsCommandStatusTextWantedNone)
			{
				if(commandName == "CSVSAddInCommandBarUI.Connect.CSVSAddInCommandBarUI")
				{
                    status = (vsCommandStatus)vsCommandStatus.vsCommandStatusSupported | 
                        vsCommandStatus.vsCommandStatusEnabled;
					return;
				}
			}
		}

		/// <summary>
        /// Implements the Exec method of the IDTCommandTarget interface. This is called when the 
        /// command is invoked.
        /// </summary>
		/// <param term='commandName'>The name of the command to execute.</param>
		/// <param term='executeOption'>Describes how the command should be run.</param>
		/// <param term='varIn'>Parameters passed from the caller to the command handler.</param>
		/// <param term='varOut'>Parameters passed from the command handler to the caller.</param>
		/// <param term='handled'>Informs the caller if the command was handled or not.</param>
		/// <seealso class='Exec' />
		public void Exec(string commandName, vsCommandExecOption executeOption, ref object varIn, 
            ref object varOut, ref bool handled)
		{
			handled = false;
			if(executeOption == vsCommandExecOption.vsCommandExecOptionDoDefault)
			{
				if(commandName == "CSVSAddInCommandBarUI.Connect.CSVSAddInCommandBarUI")
				{
					handled = true;
                    _applicationObject.ExecuteCommand("View.URL", "cfx.codeplex.com");
					return;
				}
			}
		}

		private DTE2 _applicationObject;
		private AddIn _addInInstance;
	}
}