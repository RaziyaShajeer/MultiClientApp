using OfficeOpenXml;
using SNR_ClientApp.Exceptions;
using SNR_ClientApp.Utils;
using SNR_ClientApp.Windows;
using System.Runtime.InteropServices;
using System.Threading;
namespace SNR_ClientApp
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        /// // Define the Mutex and a unique identifier for it
        private static Mutex mutex = null;
        private const string MutexName = "MyUniqueAppMutex";
        [STAThread]
        static void Main()
        {
            bool createdNew;
            mutex = new Mutex(true, MutexName, out createdNew);

            if (!createdNew)
            {
                // If the mutex already exists, another instance is running
                BringExistingInstanceToFront();
                return;
            }

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            Application.ThreadException +=
    new ThreadExceptionEventHandler(Application_ThreadException);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Add handler for UI thread exceptions
            Application.ThreadException += new ThreadExceptionEventHandler(UIThreadException);

            // Force all WinForms errors to go through handler
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

            // This handler is for catching non-UI thread exceptions
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            ApplicationConfiguration.Initialize();

            Application.Run(new Windows.LoginScreen());
            //Application.Run(new Windows.TallyConfigForm());
        }
        // Method to bring the existing instance to the foreground
        private static void BringExistingInstanceToFront()
        {
            IntPtr handle = FindWindow(null, "LoginScreen"); // Replace with your main form title
            if (handle != IntPtr.Zero)
            {
                ShowWindow(handle, SW_RESTORE);   // Restore the window if minimized
                SetForegroundWindow(handle);      // Bring it to the front
            }
        }

        static void Application_ThreadException(
        object sender, ThreadExceptionEventArgs e)
        {
            // Do logging or whatever here
            LogManager.HandleException(e.Exception, "Something Went Wrong...\n Contact SalesNrich Administrator...");
            try
            {
                throw e.Exception;
            }catch(ServiceException ex)
            {
                MessageBox.Show(ex.Message);
            }catch(Exception ex)
            {
                Application.Exit();
            }
            
        }

        private static void CurrentDomain_UnhandledException(Object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                Exception ex = (Exception)e.ExceptionObject;
                LogManager.HandleException(ex, "Something Went Wrong...\n Contact SalesNrich Administrator...");
                 throw ex;
               
            }
            catch (ServiceException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception exc)
            {
                try
                {
                    MessageBox.Show("Fatal exception happend inside UnhadledExceptionHandler: \n\n"
                        + exc.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                finally
                {
                    Application.Exit();
                }
            }

            // It should terminate our main thread so Application.Exit() is unnecessary here
        }

        private static void UIThreadException(object sender, ThreadExceptionEventArgs t)
        {
            LogManager.WriteLog("Unhandled exception catched ");
            try
            {
                MessageBox.Show("Something Went Wrong...\n Contact SalesNrich Administrator...\n Application is going to close now.");
            }
            catch
            {
                try
                {
                    MessageBox.Show("Fatal exception happend inside UIThreadException handler",
                        "Fatal Windows Forms Error", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Stop);
                }
                finally
                {
                    Application.Exit();
                }
            }

            // Here we can decide if we want to end our application or do something else
            Application.Exit();
        }
        // P/Invoke for bringing a window to the front
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private const int SW_RESTORE = 9;
    }
}